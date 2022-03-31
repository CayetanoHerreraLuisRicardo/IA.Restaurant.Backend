using IA.Restaurant.Logic.Order;
using IA.Restaurant.Utils.Models;
using IA.Restaurant.Utils.QueryFilter;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

namespace AI.Restaurant.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderLogic _logic;
        /// <summary>
        /// contructor OrdersController
        /// </summary>
        /// <param name="logic"></param>
        public OrdersController(IOrderLogic logic)
        {
            _logic = logic;
        }
        /// <summary>
        /// get orders by query
        /// </summary>
        /// <remarks>you call get list of orders</remarks>
        /// <param name="queryfilter">query filter to apply, query example: where IdStatus = 1   [{"PropertyName": "IdStatus","Value": 1, "Comparison": 0}]</param>
        /// <response code="200">Operación exitosa</response>
        /// <response code="400">Indica que el servidor no puede procesar la solicitud debido a que se percibe como un error del cliente</response>
        /// <response code="401">Indica que la petición (request) no ha sido ejecutada porque carece de credenciales válidas de autenticación para el recurso solicitado</response>
        /// <response code="403">Indica que el servidor ha entendido nuestra petición, pero se niega a autorizarla</response>
        /// <response code="500">Error en el servidor </response>
        [HttpGet]
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(List<OrderModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> GetAll([FromQuery] string queryfilter = "[{\"PropertyName\": \"IdStatus\",\"Value\": 1, \"Comparison\": 0}]")
        {
            return Ok(await _logic.Get(queryfilter.BuildFilterExpression()));
        }
        /// <summary>
        /// get order by id
        /// </summary>
        /// <param name="idOrder">
        ///id del producto
        /// </param>
        /// <remarks>Aquí es donde podras consultar el producto por id</remarks>
        /// <response code="200">Operación exitosa</response>
        /// <response code="400">Indica que el servidor no puede procesar la solicitud debido a que se percibe como un error del cliente</response>
        /// <response code="401">Indica que la petición (request) no ha sido ejecutada porque carece de credenciales válidas de autenticación para el recurso solicitado</response>
        /// <response code="403">Indica que el servidor ha entendido nuestra petición, pero se niega a autorizarla</response>
        /// <response code="500">Error en el servidor </response>
        [HttpGet("{idOrder:int}", Name = "GetOrderById")]
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(OrderModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> GetOrderById([FromRoute] int idOrder)
        {
            List<FilterExpression> queryfilter = new List<FilterExpression>{
                new FilterExpression
                {
                    PropertyName = nameof(OrderModel.IdOrder),
                    Comparison = Comparison.Equal,
                    Value = idOrder
                }
            };
            OrderModel? order = (await _logic.Get(queryfilter)).FirstOrDefault();
            if(order == null)
                return NotFound();
            return Ok(order);
        }
        /// <summary>
        /// create order
        /// </summary>
        /// <param name="body">
        /// order with the list of products
        /// </param>
        /// <remarks>Aquí es donde podras crear la programación masiva de vacaciones de varios empleados al mismo tiempo</remarks>
        /// <response code="200">Operación exitosa</response>
        /// <response code="400">Indica que el servidor no puede procesar la solicitud debido a que se percibe como un error del cliente</response>
        /// <response code="401">Indica que la petición (request) no ha sido ejecutada porque carece de credenciales válidas de autenticación para el recurso solicitado</response>
        /// <response code="403">Indica que el servidor ha entendido nuestra petición, pero se niega a autorizarla</response>
        /// <response code="500">Error en el servidor </response>
        [HttpPost]
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(OrderModel), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> Create([FromBody] List<ProductModel> body)
        {
            if (!ModelState.IsValid)
                return BadRequest();
            OrderModel order = await _logic.Create(body);
            if (!order.lstProduct.Any())
                return BadRequest();
            // we send the uri of the new resource created in the headers
            return CreatedAtAction(actionName: "GetOrderById", controllerName: "Orders", routeValues: new { idOrder = order.IdOrder }, order);
        }
        /// <summary>
        /// update product
        /// </summary>
        /// <param name="id"></param>
        /// <param name="body">
        /// filtro para obtener elementos
        /// </param>
        /// <remarks>Aquí es donde podras crear la programación masiva de vacaciones de varios empleados al mismo tiempo</remarks>
        /// <response code="200">Operación exitosa</response>
        /// <response code="400">Indica que el servidor no puede procesar la solicitud debido a que se percibe como un error del cliente</response>
        /// <response code="401">Indica que la petición (request) no ha sido ejecutada porque carece de credenciales válidas de autenticación para el recurso solicitado</response>
        /// <response code="403">Indica que el servidor ha entendido nuestra petición, pero se niega a autorizarla</response>
        /// <response code="500">Error en el servidor </response>
        [HttpPatch("{id:int}")]
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(OrderStatusModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> UpdateStatus([FromRoute] int id, [FromBody] OrderStatusModel body)
        {
            if (!ModelState.IsValid)
                return BadRequest();
            return Ok(await _logic.UpdateStatus(id, body));
        }
    }
}
