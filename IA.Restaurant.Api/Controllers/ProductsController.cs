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
        /// <param name="logicOrder"></param>
        public ProductsController(IGenericCrudLogic<ProductModel> logic, IOrderLogic logicOrder)
        {
            _logic = logic;
            _logicOrder = logicOrder;
        }
        /// <summary>
        /// obtener producto por id
        /// </summary>
        /// <param name="id">
        ///id del producto
        /// </param>
        /// <remarks>Aquí es donde podras consultar un producto por id</remarks>
        /// <response code="200">Operación exitosa</response>
        /// <response code="400">Indica que el servidor no puede procesar la solicitud debido a que se percibe como un error del cliente</response>
        /// <response code="401">Indica que la petición (request) no ha sido ejecutada porque carece de credenciales válidas de autenticación para el recurso solicitado</response>
        /// <response code="403">Indica que el servidor ha entendido nuestra petición, pero se niega a autorizarla</response>
        /// <response code="500">Error en el servidor </response>
        [HttpGet("{id:int}", Name = "GetProductById")]
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(ProductModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> GetProductById([FromRoute] int id)
        {
            ProductModel product = await _logic.ReadById(id);
            if (product == null)
                return NotFound();
            return Ok(product);
        }
        /// <summary>
        /// consultar la lista de productos aplicando un filtro
        /// </summary>
        /// <remarks>
        /// `Ejemplo #1` consultar los productos con `Sku` igual a DPY-001-A necesitas enviar en el parametro `queryfilter` el siguiente valor:
        ///
        ///     [{
        ///         "PropertyName": "Sku",
        ///         "Value": "DPY-001-A", 
        ///         "Comparison": 0}
        ///     ]
        ///     
        /// `Ejemplo #2` consultar los productos con `Stock` menor a 10 necesitas enviar en parametro `queryfilter` el siguiente valor:
        ///
        ///     [{
        ///         "PropertyName": "Stock",
        ///         "Value": 10, 
        ///         "Comparison": 1}
        ///     ]
        ///     
        /// `Ejemplo #3` consultar los productos con `UnitPrice` menor o igual a 100 necesitas enviar en parametro `queryfilter` el siguiente valor:
        ///
        ///     [{
        ///         "PropertyName": "UnitPrice",
        ///         "Value": 100, 
        ///         "Comparison": 2}
        ///     ]
        ///     
        /// `Ejemplo #4` consultar los productos con `Stock` mayor a 0 necesitas enviar en parametro `queryfilter` el siguiente valor:
        ///
        ///     [{
        ///         "PropertyName": "Stock",
        ///         "Value": 0, 
        ///         "Comparison": 3}
        ///     ]
        ///
        /// `Ejemplo #5` consultar los productos con `UnitPrice` mayor o igual a 50 necesitas enviar en parametro `queryfilter` el siguiente valor:
        ///
        ///     [{
        ///         "PropertyName": "UnitPrice",
        ///         "Value": 50, 
        ///         "Comparison": 4}
        ///     ]
        ///     
        /// `Ejemplo #6` consultar los productos con `Sku` diferente de DPY-001-A necesitas enviar en parametro `queryfilter` el siguiente valor:
        ///
        ///     [{
        ///         "PropertyName": "Sku",
        ///         "Value": "DPY-001-A", 
        ///         "Comparison": 5}
        ///     ]
        ///     
        /// `Ejemplo #7` este ejemplo es aplicando mas de un filtro, por ejemplo si nos interesa consultar cuales productos tienen `Sku` diferente de DPY-001-A que tengan `Stock` mayor a 0 y `UnitPrice` menor o igual a 100 necesitas enviar en parametro `queryfilter` el siguiente valor:
        ///
        ///     [{
        ///         "PropertyName": "Sku",
        ///         "Value": "DPY-001-A", 
        ///         "Comparison": 5,
        ///         "Conjunction": 0},
        ///      {
        ///         "PropertyName": "Stock",
        ///         "Value": 0, 
        ///         "Comparison": 3,
        ///         "Conjunction": 0},
        ///      {
        ///         "PropertyName": "UnitPrice",
        ///         "Value": 100, 
        ///         "Comparison": 2,
        ///         "Conjunction": 0}
        ///     ]
        /// 
        /// ¿Estas listo? lo siguiente es darle clic en `Try it out` hacer la prueba con el valor que tiene por defecto el campo `queryfilter` o bien copiar uno de los ejemplos de arriba y jugar con los valores para realizar las consultas
        /// 
        /// </remarks>
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
                // send the uri of the new resource created in the headers
                return CreatedAtAction(actionName: "GetProductById", controllerName: "Products", routeValues: new { idProduct = body.IdProduct }, body);
            }
            return BadRequest();
        }
        /// <summary>
        /// update product
        /// </summary>
        /// <param name="id">
        /// id del producto
        /// </param>
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
        /// <param name="id">
        /// id del producto
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
            // check if there are orders linked to this product
            IEnumerable<ProductModel> productsOrder =  _logicOrder.GetProducts(queryfilter);
            if (productsOrder.Any())
                return BadRequest();

            await _logic.Delete(item);
            return Ok();
        }
    }
}
