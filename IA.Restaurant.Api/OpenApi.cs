using Microsoft.OpenApi.Models;
using System.Reflection;

namespace IA.Restaurant.Api
{
    internal class OpenApi
    {
        internal static void ConfigureServices(IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "API Restaurant",
                    Description = "Documentación de la API usando la especificación OpenAPI.\n\n" +
                    "Esta api actualmente cuenta con la version 1, el cual incluye: \n\n " +
                    "\t - CRUD de productos \n" +
                    "\t\t Crear producto \n" +
                    "\t\t Consultar lista de productos \n" +
                    "\t\t Consultar producto por id \n" +
                    "\t\t Actualizar producto por id \n" +
                    "\t\t Eliminar producto por id \n" +
                    "\t - Para Ordenes se hicieron los servicios solicitados: \n" +
                    "\t\t Dar de alta una orden \n" +
                    "\t\t Cambio de estatus de la orden \n" +
                    "\t\t Se agregó un servicio extra para Consultar una orden por id :). \n\n\n" +
                    "Para poder aplicar los filtros en los endpoints GET de Products y Orders se agregaron ejemplos en cada uno de estos endpoints para poder hacer las pruebas, de cualquier manera a continuación se describe como se arma el filtro o queryparam, tambien se describe para que sirve cada unos de ellos. \n\n" +
                    "Los endpoints GET(para consultar productos y ordenes) espera recibir un query param con un valor en forma string (el valor que se envia en los parametros queryFilter) esta cadena debe armarse de la siguiente forma: \n\n" +
                    "\t [{\"PropertyName\": \"Sku\",\"Value\": \"DPY-001-A\", \"Comparison\": 0}]. \n\n\n" +
                    "Practicamente le estamos pasando un arreglo o coleccion de filtros, para este ejemplo es un solo filtro va hacer el filtro para buscar todos los registros que tengan en Sku = DPY-001-A \n\n" +
                    "\t Para el valor de Comparison se utiliza para saber que comparación se tiene que aplicar los valores que acepta actualemnte son: \n" +
                    "\t\t 0 (=) Igual al valor que se envie en PropertyName \n" +
                    "\t\t 1 (<) Menor que el valor que se envie en PropertyName \n" +
                    "\t\t 2 (>=) Menor o igual al valor que se envie en PropertyName \n" +
                    "\t\t 3 (>) Mayor que el valor que se envie en PropertyName \n" +
                    "\t\t 4 (>=) Mayor o Igual al valor que se envie en PropertyName\n" +
                    "\t\t 5 (<>) Distinto o diferente al valor que se envie en PropertyName\n\n" +
                    " Por ultimo si requieres enviar varios filtros al mismo tiempo necesitas enviar otra propiedad para indicarle que conjugacion se tendrá que aplicar \n\n" +
                    "\t\t 0 (AND) \n" +
                    "\t\t 1 (OR) \n\n" +
                    "!Para esta primera version no se agregaron mensajes de custom para los errores que se pudieran presentar!\n\n" +
                    "Para los endpoits POST(crear un nuevo recurso), PUT(actualizar un recurso), PATCH(modificacion parcial de un recurso), DELETE(eliminar unn recurso) solo basta con enviar los datos que se te solicitan."
                    ,
                    Contact = new OpenApiContact
                    {
                        Name = "Ricardo Cayetano",
                        Email = "richych92@gmail.com",
                        Url = new Uri("https://about.me/richy_92"),
                    },
                    License = new OpenApiLicense
                    {
                        Name = "UNLICENCE"
                    }
                }); ;
                //Locate the XML file being generated by ASP.NET...
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.XML";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

                //... and tell Swagger to use those XML comments.
                c.IncludeXmlComments(xmlPath);
            });
        }

        internal static void Configure(IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                //c.RoutePrefix = "swagger";
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "API Restaurant v1");
            });
        }
    }
}
