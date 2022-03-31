using AutoMapper;
using IA.Restaurant.Data;
using IA.Restaurant.Entities.Tables;
using IA.Restaurant.Utils.Models;
using IA.Restaurant.Utils.QueryFilter;
using System.Linq.Expressions;
using System.Transactions;

namespace IA.Restaurant.Logic.Order
{
    public class OrderLogic : IOrderLogic
    {
        public readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public OrderLogic(IUnitOfWork unitOfWork, IMapper iMapper)
        {
            this._unitOfWork = unitOfWork;
            this._mapper = iMapper;
        }
        public async Task<OrderModel> Create(List<ProductModel> item)
        {
            OrderModel order = new OrderModel();
            using TransactionScope scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
            Orders newItem = new Orders();
            Orders lastItem = (_unitOfWork.OrderRepository.GetIQueryable()).OrderByDescending(x => x.IdOrder).FirstOrDefault();
            int idOrder = lastItem != null ? lastItem.IdOrder + 1 : 1;
            newItem.IdStatus = 1;
            newItem.IdOrder = idOrder;
            List<OrderDetails> lstOrderDetails = new List<OrderDetails>();
            List<Products> lstProducts = new List<Products>();
            foreach (var p in item)
            {
                OrderDetails od = new OrderDetails();
                Products product = _unitOfWork.ProductRepository.GetByID(p.IdProduct);
                if (product != null && product.Stock >= p.Stock && !product.Deleted)
                {
                    od.IdOrder = idOrder;
                    od.IdProduct = p.IdProduct;
                    od.UnitPrice = p.UnitPrice;
                    od.Quantity = p.Stock;
                    lstOrderDetails.Add(od);
                    product.Stock -= p.Stock;
                    lstProducts.Add(product);
                }
            }
            if (lstProducts.Any())
            {
                await _unitOfWork.OrderDetailRepository.BulkInsert(lstOrderDetails);
                _unitOfWork.ProductRepository.BulkUpdate(lstProducts);
                await _unitOfWork.OrderRepository.InsertAsync(newItem);
            }

            await _unitOfWork.SaveAsync();
            scope.Complete();
            scope.Dispose();
            order.IdOrder = newItem.IdOrder;
            order.IdStatus = newItem.IdStatus;
            order.lstProduct = _mapper.Map<List<Products>, List<ProductModel>>(lstProducts);
            return order;
        }
        public async Task<List<OrderModel>> Get(List<FilterExpression> filter)
        {
            Expression<Func<Orders, bool>> orderExpression = filter != null ? ExpressionBuilder.ConstruyendoExpresion<Orders>(filter.Where(x => typeof(Orders).GetProperties().Any(p => p.Name.ToLower() == x.PropertyName.ToLower())).ToList()) : (t => true);
            Expression<Func<OrderDetails, bool>> orderDetailExpression = filter != null ? ExpressionBuilder.ConstruyendoExpresion<OrderDetails>(filter.Where(x => typeof(OrderDetails).GetProperties().Any(p => p.Name.ToLower() == x.PropertyName.ToLower())).ToList()) : (t => true);
            Expression<Func<Status, bool>> statusExpression = filter != null ? ExpressionBuilder.ConstruyendoExpresion<Status>(filter.Where(x => typeof(Status).GetProperties().Any(p => p.Name.ToLower() == x.PropertyName.ToLower())).ToList()) : (t => true);

            List<OrderModel> lstOrders = (from o in _unitOfWork.OrderRepository.GetIQueryable(orderExpression)
                                          join s in _unitOfWork.StatusRepository.GetIQueryable(statusExpression)
                                          on o.IdStatus equals s.IdStatus
                                          select new OrderModel
                                          {
                                              IdOrder = o.IdOrder,
                                              IdStatus = s.IdStatus,
                                              Status = s.Name,
                                              //lstProduct = GetProducts(_mapper.Map<Orders, OrderModel>(o)).ToList()
                                          }).ToList();
            lstOrders.Select(c =>
            {
                List<FilterExpression> queryfilter = new List<FilterExpression>{
                    new FilterExpression
                    {
                        PropertyName = nameof(OrderModel.IdOrder),
                        Comparison = Comparison.Equal,
                        Value = c.IdOrder
                    }
                };
                c.lstProduct = this.GetProducts(queryfilter).ToList();
                return c;
            }).ToList();
            return await Task.FromResult(lstOrders);
        }
        public async Task<OrderStatusModel> UpdateStatus(int id, OrderStatusModel item)
        {
            Orders order = _unitOfWork.OrderRepository.GetByID(id);
            using TransactionScope scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
            order.IdStatus = item.IdStatus;
            await _unitOfWork.OrderRepository.UpdateAsync(order, x => x.IdOrder == id);
            await _unitOfWork.SaveAsync();
            scope.Complete();
            scope.Dispose();
            return await Task.FromResult(item);
        }
        public IEnumerable<ProductModel> GetProducts(List<FilterExpression> filters)
        {
            Expression<Func<OrderDetails, bool>> odExpression = filters != null ? ExpressionBuilder.ConstruyendoExpresion<OrderDetails>(filters.Where(x => typeof(OrderDetails).GetProperties().Any(p => p.Name.ToLower() == x.PropertyName.ToLower())).ToList()) : (t => true);
            Expression<Func<Products, bool>> productExpression = filters != null ? ExpressionBuilder.ConstruyendoExpresion<Products>(filters.Where(x => typeof(Products).GetProperties().Any(p => p.Name.ToLower() == x.PropertyName.ToLower())).ToList()) : (t => true);
            IEnumerable<ProductModel> products = (from od in _unitOfWork.OrderDetailRepository.GetIQueryable(odExpression)
                                                  join p in _unitOfWork.ProductRepository.GetIQueryable(productExpression)
                                                  on od.IdProduct equals p.IdProduct
                                                  select new ProductModel
                                                  {
                                                      IdProduct = p.IdProduct,
                                                      Name = p.Name,
                                                      Stock = od.Quantity,
                                                      Sku = p.Sku
                                                  });
            return products;
        }
    }
}
