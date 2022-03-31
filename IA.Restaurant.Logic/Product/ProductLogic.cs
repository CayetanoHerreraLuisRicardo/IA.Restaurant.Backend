using AutoMapper;
using IA.Restaurant.Data;
using IA.Restaurant.Entities.Tables;
using IA.Restaurant.Utils.GenericCrud;
using IA.Restaurant.Utils.Models;
using IA.Restaurant.Utils.QueryFilter;
using System.Linq.Expressions;
using System.Transactions;

namespace IA.Restaurant.Logic.Product
{
    public class ProductLogic : IGenericCrudLogic<ProductModel>
    {
        public readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public ProductLogic(IUnitOfWork unitOfWork, IMapper iMapper)
        {
            this._unitOfWork = unitOfWork;
            this._mapper = iMapper;
        }
        /// <inheritdoc/>
        public async Task<ProductModel> Create(ProductModel item)
        {
            Products itemEntity = _mapper.Map<ProductModel, Products>(item);
            using TransactionScope scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
            await _unitOfWork.ProductRepository.InsertAsync(itemEntity);
            await _unitOfWork.SaveAsync();
            scope.Complete();
            scope.Dispose();
            item.IdProduct = itemEntity.IdProduct;
            return item;
        }
        /// <inheritdoc/>
        public async Task Delete(ProductModel item)
        {
            Products productEntity = _mapper.Map<ProductModel, Products>(item);
            productEntity.Deleted = true;
            using TransactionScope scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
            /// apply logical delete
            await _unitOfWork.ProductRepository.UpdateAsync(productEntity, x => x.IdProduct == item.IdProduct);
            //_unitOfWork.ProductRepository.Delete(item.IdProduct);
            await _unitOfWork.SaveAsync();
            scope.Complete();
            scope.Dispose();
        }
        /// <inheritdoc/>
        public async Task<ProductModel> ReadById(int id)
        {
            Products? product = _unitOfWork.ProductRepository.GetIQueryable(x => x.IdProduct == id && x.Deleted == false).FirstOrDefault();
            await Task.CompletedTask;
            return _mapper.Map<Products, ProductModel>(product);
        }

        public async Task<IEnumerable<ProductModel>> Read(List<FilterExpression> filters)
        {
            Expression<Func<Products, bool>>? expression = filters != null ? ExpressionBuilder.ConstruyendoExpresion<Products>(filters.Where(x => typeof(Products).GetProperties().Any(p => p.Name.ToLower() == x.PropertyName.ToLower())).ToList()): (t=> true);
            await Task.CompletedTask;
            return _mapper.Map<IEnumerable<Products>, IEnumerable<ProductModel>>(_unitOfWork.ProductRepository.GetIQueryable(expression));
        }

        /// <inheritdoc/>
        public async Task<ProductModel> Update(int id, ProductModel item)
        {
            item.IdProduct = id;
            using TransactionScope scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
            Products productEntity = _mapper.Map<ProductModel, Products>(item);
            await _unitOfWork.ProductRepository.UpdateAsync(productEntity, x => x.IdProduct == item.IdProduct);
            await _unitOfWork.SaveAsync();
            scope.Complete();
            scope.Dispose();
            return await this.ReadById(id);
        }
    }
}
