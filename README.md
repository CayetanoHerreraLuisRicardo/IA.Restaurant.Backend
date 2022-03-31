## IA.Restaurant.Backend
### Proyecto ASP.NET Core Wep API con .NET 6

Para poder correr la aplicación desde visual studio necesitas tener instalado el [SDK 6.0](https://dotnet.microsoft.com/en-us/download) da clic en este enlace y descarga la version correspondiente a la tu S.O. 

Para este proyecto se uso:
- Arquitectura en Capas,
- EF core como ORM para la la conexión a la base de datos, esta preparada para que se utilice cualquier SGBD (para este proyecto solo se consideró MySQL y SQL Server)
- Se implementaron los patrones de diseño Repository y UnitOfWork
- Se diseñó y documentación la API REST siguiendo la especificación [OpenApi/Swagger](https://swagger.io/tools/swagger-ui/) entrar en el enlace para mayor información. 
- Para documentar la API en nuestro proyecto ASP.NET Core Web Api se utilizó [Swashbuckle](https://docs.microsoft.com/es-es/aspnet/core/tutorials/getting-started-with-swashbuckle?view=aspnetcore-6.0&tabs=visual-studio) 
- Este proyecto aplica una parte de los principio SOLID
  
### ⚡️ Estructura del proyecto
- IA.Restaurant.Api => Proyecto WepApi que contiene la capa de presentación este es el proyecto principal de nuestra solución IA.sln
- IA.Restaurant.Data => Proyecto Libreria de Clase que contiene la Capa de acceso a datos 
- IA.Restaurant.Entities => Proyecto Libreria de Clase que contiene la Capa de entidades que vienen siendo el mapeo de nuestras tablas de base de datos
- IA.Restaurant.Logic => Proyecto Libreria de Clase que contiene la capa de Logica de negocio
- IA.Restaurant.Utils => Proyecto Libreria de Clase que contiene todas nuestras utilerias o clases genericas que se estaran utilizando por los demas proyectos dentro de la solución.

![Estructura del proyecto](https://github.com/CayetanoHerreraLuisRicardo/IA.Restaurant.Frontend/blob/main/assets/images/image2.png)

### ⚡️ Ejecutar Script para crear la base de datos
Este script es para correrlo en SQL Server este mismo script se encuentra en este repositorio se llama db.sql
```sh
--Script para crear la las tablas y realizar los insert de los status 
CREATE TABLE [dbo].[products](
	[id_product] [int] IDENTITY(1,1) NOT NULL,
	[sku] [varchar](20) NOT NULL,
	[name] [varchar](50) NOT NULL,
	[unit_price] [float] NOT NULL,
	[stock] [int] NOT NULL,
	[deleted] [bit] NOT NULL,
	[creation_date] [datetime] NOT NULL,
	[modification_date] [datetime] NOT NULL,
 CONSTRAINT [PK_products] PRIMARY KEY CLUSTERED 
(
	[id_product] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
CREATE TABLE [dbo].[status](
	[id_status] [int] NOT NULL,
	[name] [varchar](50) NOT NULL,
	[creation_date] [datetime] NOT NULL,
	[modification_date] [datetime] NOT NULL,
 CONSTRAINT [PK_status] PRIMARY KEY CLUSTERED 
(
	[id_status] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
CREATE TABLE [dbo].[orders](
	[id_order] [int] NOT NULL,
	[creation_date] [datetime] NOT NULL,
	[modification_date] [datetime] NOT NULL,
	[id_status] [int] NOT NULL,
 CONSTRAINT [PK_orders] PRIMARY KEY CLUSTERED 
(
	[id_order] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
CREATE TABLE [dbo].[order_details](
	[unit_price] [float] NOT NULL,
	[quantity] [int] NOT NULL,
	[creation_date] [datetime] NOT NULL,
	[modification_date] [datetime] NOT NULL,
	[id_order] [int] NOT NULL,
	[id_product] [int] NOT NULL,
)
GO
ALTER TABLE [dbo].[orders]  WITH CHECK ADD  CONSTRAINT [FK_ORDERS-STATUS] FOREIGN KEY([id_status])
REFERENCES [dbo].[status] ([id_status])
ALTER TABLE [dbo].[order_details]  WITH CHECK ADD  CONSTRAINT [FK_ORDER_DETAILS-ORDERS] FOREIGN KEY([id_order])
REFERENCES [dbo].[orders] ([id_order])
ALTER TABLE [dbo].[order_details]  WITH CHECK ADD  CONSTRAINT [FK_ORDER_DETAILS-PRODUCTS] FOREIGN KEY([id_product])
REFERENCES [dbo].[products] ([id_product])
INSERT INTO [dbo].[status]
           ([id_status]
           ,[name]
           ,[creation_date]
           ,[modification_date])
     VALUES
           (1
           ,'Pending'
           ,GETDATE()
           ,GETDATE()),
			(2
           ,'In Process'
           ,GETDATE()
           ,GETDATE()),
			(3
           ,'Completed'
           ,GETDATE()
           ,GETDATE()),
			(4
           ,'Delivered'
           ,GETDATE()
           ,GETDATE()),
			(5
           ,'Canceled'
           ,GETDATE()
           ,GETDATE())
```
### ⚡️ Modificar appsettings.json de proyecto IA.Restaurant.Api
```sh
  "ConnectionStrings": {
    "MYSQLContext": "Data Source=localhost; Initial Catalog=IARestaurant; User ID=user;Password=pass;",
    "MSSQLContext": "Data Source=localhost; Initial Catalog=IARestaurant; User ID=user;Password=pass;"
  },
```
Cambiar los datos de conexión
## A continuación se agregan pasos para gerar el publish estos pasos se pueden omitir si levantas el proyecto desde Visual Studio
### ⚡️ Generar el publish de nuestra proyecto
#### Ubicarnos dentro de nuestro proyecto IA.Restaurant.Api 
Considerando que el proyecto ya cargo correctamente todas sus dependencias y se cargaron todas las referencias en cada uno de los proyectos y que el proyecto se haya construido correctamente procedemos a generar el publish desde el el mismo IDE Visual Studio dando clic derecho sobre el proyecto IA.Restaurant.Api y le damos en Publish/Publicar de esta manera el mismo Visual Studio te ira guiando hasta generar el publish segun la configuración que elijas.
La segunda manera es usando el comando dotnet desde linea de comandos
```sh
    $ dotnet publish -c Release
```
![Publish](https://github.com/CayetanoHerreraLuisRicardo/IA.Restaurant.Frontend/blob/main/assets/images/image3.png)
### ⚡️ Levantar nuestra API
Nos ubicamos dentro de la carpeta donde se genero el publish por default te lo va a generar en IA.Restaurant.Api\bin\Release\net6.0\publish  
```sh
    $ dotnet IA.Restaurant.Api.dll
```
![RunApp](https://github.com/CayetanoHerreraLuisRicardo/IA.Restaurant.Frontend/blob/main/assets/images/image4.png)
Tambien si estas en un S.O. Windows podemos ejecutar el IA.Restaurant.Api.exe 
### ⚡️ Probar los servicios desde la interfaz Swagger/OpenApi
Si todo sale bien, vamos al navegador e ingresamos a http://localhost:5000 o bien con el puerto que nos marque en la consola, para este ejemplo nos mandará un 404 ya que esta ruta nuesta mapeada en nuestra aplicación. Para ver que este funcionando correctamente abrimos http://localhost:5000/swagger/ o http://localhost:5000/swagger/index.html cualquiera de las dos deberia de abrirte la documentación de la API y se te abrira el cliente Swagger tal como si fuera un cliente como POSTMAN pero con una interfaz mas bonita y todo mas amigagle no tienes que configurar nada solo basta con leer la documentación de la API y enviar los datos que te pide, no te preocupes por el cuerpo de solicitud a enviar ahi mismo esta el cuerpo de la solicitud con un ejemplo solo basta con cambiar los datos y listo podras hacer pruebas en los diferentes endpoints. !Ojo! no olvides leer la documentación del swagger 
![Swagger](https://github.com/CayetanoHerreraLuisRicardo/IA.Restaurant.Frontend/blob/main/assets/images/image5.png)
### ⚡️ Probar toda la aplicación completa Frontend(AngularJs) y Backend (ASP.NET Core Wep API)
Ahora si podras interactuar con el backend y el frontend aqui te dejo el repo del repositorio de proyecto frontend desarrollado con  [AngularJs](https://github.com/CayetanoHerreraLuisRicardo/IA.Restaurant.Backend) en este repositorio encontraras la descripción de como iniciar o configurar el proyecto frontend.
![App](https://github.com/CayetanoHerreraLuisRicardo/IA.Restaurant.Frontend/blob/main/assets/images/image1.png)
### ⚡️ Datos extras
- Tambien se genero el archivo IA.Restaurant.Api.postman_collection.json que se exportó desde el cliente POSTMAN.
- Tambien se agregó el archivo ApiDoc.json de la documentación de la API generada con Swagger/OpenApi 