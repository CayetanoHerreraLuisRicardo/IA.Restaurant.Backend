using IA.Restaurant.Logic.Order;
using IA.Restaurant.Utils.GenericCrud;
using IA.Restaurant.Utils.Models;
using IA.Restaurant.Utils.QueryFilter;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

namespace AI.Restaurant.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IGenericCrudLogic<ProductModel> _logic;
        private readonly IOrderLogic _logicOrder;
        /// <summary>
        /// contructor ProductsController
        /// </summary>
        /// <param name="logic"></param>
        public ProductsController(IGenericCrudLogic<ProductModel> logic, IOrderLogic logicOrder)
        {
            _logic = logic;
            _logicOrder = logicOrder;
        }
        /// <summary>
        /// get product by id
        /// </summary>
        /// <param name="id">
        ///id del producto
        /// </param>
        /// <remarks>Aquí es donde podras consultar el producto por id</remarks>
        /// <response code="200">Operación exitosa</response>
        /// <response code="400">Indica que el servidor no puede procesar la solicitud debido a que se percibe como un error del cliente</response>
        /// <response code="401">Indica que la petición (request) no ha sido ejecutada porque carece de credenciales válidas de autenticación para el recurso solicitado</response>
        /// <response code="403">Indica que el servidor ha entendido nuestra petición, pero se niega a autorizarla</response>
        /// <response code="500">Error en el servidor </response>
        [HttpGet("{idProduct:int}", Name = "GetProductById")]
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(ProductModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> GetProductById([FromRoute] int idProduct)
        {
            ProductModel product = await _logic.ReadById(idProduct);
            if (product == null)
                return NotFound();
            return Ok(product);
        }
        /// <summary>
        /// get products by query
        /// </summary>
        /// <remarks>you call get list of products</remarks>
        /// <param name="queryfilter">query filter to apply, query example: where IdProduct > 0   [{"PropertyName": "IdProduct","Value": 0, "Comparison": 3}]</param>
        /// <response code="200">Operación exitosa</response>
        /// <response code="400">Indica que el servidor no puede procesar la solicitud debido a que se percibe como un error del cliente</response>
        /// <response code="401">Indica que la petición (request) no ha sido ejecutada porque carece de credenciales válidas de autenticación para el recurso solicitado</response>
        /// <response code="403">Indica que el servidor ha entendido nuestra petición, pero se niega a autorizarla</response>
        /// <response code="500">Error en el servidor </response>
        [HttpGet]
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(List<ProductModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> GetAll([FromQuery] string queryfilter = "[{\"PropertyName\": \"IdProduct\",\"Value\": 0, \"Comparison\": 3}]")
        {
            return Ok(await _logic.Read(queryfilter.BuildFilterExpression()));
        }
        /// <summary>
        /// create product
        /// </summary>
        /// <param name="body">
        /// item
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
        [ProducesResponseType(typeof(ProductModel), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> Create([FromBody] ProductModel body)
        {
            if (!ModelState.IsValid)
                return BadRequest();
            List<FilterExpression> queryfilter = new List<FilterExpression>{
                new FilterExpression
                {
                    PropertyName = nameof(ProductModel.Sku),
                    Comparison = Comparison.Equal,
                    Value = body.Sku
                }
            };
            IEnumerable<ProductModel> products = await _logic.Read(queryfilter);
            if (!products.Any())
            {
                
                body = await _logic.Create(body);
                /// send the uri of the new resource created in the headers
                return CreatedAtAction(actionName: "GetProductById", controllerName: "Products", routeValues: new { idProduct = body.IdProduct }, body);
            }
            return BadRequest();
        }
        /// <summary>
        /// update product
        /// </summary>
        /// <param name="body">
        /// filtro para obtener elementos
        /// </param>
        /// <remarks>Aquí es donde podras crear la programación masiva de vacaciones de varios empleados al mismo tiempo</remarks>
        /// <response code="200">Operación exitosa</response>
        /// <response code="400">Indica que el servidor no puede procesar la solicitud debido a que se percibe como un error del cliente</response>
        /// <response code="401">Indica que la petición (request) no ha sido ejecutada porque carece de credenciales válidas de autenticación para el recurso solicitado</response>
        /// <response code="403">Indica que el servidor ha entendido nuestra petición, pero se niega a autorizarla</response>
        /// <response code="500">Error en el servidor </response>
        [HttpPut("{id:int}")]
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(ProductModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> Put([FromRoute] int id, [FromBody] ProductModel body)
        {
            if (!ModelState.IsValid)
                return BadRequest();
            if (await _logic.ReadById(id) == null)
                return NotFound();

            List<FilterExpression> queryfilter = new List<FilterExpression>{
                new FilterExpression
                {
                    PropertyName = nameof(ProductModel.IdProduct),
                    Comparison = Comparison.NotEqual,
                    Value = id,
                    Conjunction = Conjunction.And
                },
                new FilterExpression
                {
                    PropertyName = nameof(ProductModel.Sku),
                    Comparison = Comparison.Equal,
                    Value = body.Sku,
                    Conjunction = Conjunction.And
                }
            };
            IEnumerable<ProductModel> repeatedSku = await _logic.Read(queryfilter);
            if (!repeatedSku.Any())
                return Ok(await _logic.Update(id, body));
            return BadRequest();
        }
        /// <summary>
        /// Crear Programacion masiva de vacaciones 
        /// </summary>
        /// <param name="body">
        /// filtro para obtener elementos
        /// </param>
        /// <remarks>Aquí es donde podras crear la programación masiva de vacaciones de varios empleados al mismo tiempo</remarks>
        /// <response code="200">Operación exitosa</response>
        /// <response code="400">Indica que el servidor no puede procesar la solicitud debido a que se percibe como un error del cliente</response>
        /// <response code="401">Indica que la petición (request) no ha sido ejecutada porque carece de credenciales válidas de autenticación para el recurso solicitado</response>
        /// <response code="403">Indica que el servidor ha entendido nuestra petición, pero se niega a autorizarla</response>
        /// <response code="500">Error en el servidor </response>
        [HttpDelete("{id:int}")]
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(ProductModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> Delete([FromRoute] int id)
        {
            ProductModel item = await _logic.ReadById(id);
            if (item == null)
                return NotFound();
            List<FilterExpression> queryfilter = new List<FilterExpression>{
                new FilterExpression
                {
                    PropertyName = nameof(ProductModel.IdProduct),
                    Comparison = Comparison.Equal,
                    Value = id
                }
            };
            /// check if there are orders linked to this product
            IEnumerable<ProductModel> productsOrder =  _logicOrder.GetProducts(queryfilter);
            if (productsOrder.Any())
                return BadRequest();

            await _logic.Delete(item);
            return Ok();
        }
    }
}
