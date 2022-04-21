## IA.Restaurant.Frontend
### Proyecto desarrollado con AngularJS v1.5.0

Para poder montar la aplicación angularjs puedes subir la carpeta en el servidor de tu preferencia. En este proyecto dejé preparado un js llamado server.js que nos servira para montar la aplicación que funge como nuestro servidor, para esto solo necesitas tener instalado NodeJs y npm

### ⚡️ Montar Aplicacion AngularJs con Express.js
#### 1.- Ubicarse en la capeta raiz del directorio e instalamos los modulos para levantar nuestro servicio con express, ejecutar lo siguiente:
```sh
    $ npm install
```
#### 2.- Levantar nuestro servicio express, el cual levanta un servicio 
```sh
    $ node server
```

Si todo sale bien, vamos al navegador e ingresamos a http://localhost:3002 o bien con el puerto en la que se inicio nuestra aplicación express.

#### 3.- Levantar nuestra API 
Ahora si podras interactuar con la aplicacion web. Lo que faltaria es probar es que nuestra aplicación angularjs se comunique bien con nuestra API REST, el proyecto backend es un proyecto ASP.NET Core Wep API, se uso la version .NET 6. Este repositorio lo puedes encontrar [aqui](https://github.com/CayetanoHerreraLuisRicardo/IA.Restaurant.Backend) en este repositorio encontraras la descripción de como iniciar o configurar el proyecto backend para poder la prueba completa.

Dare por hecho que ya tienes levantada la API, lo que haria falta por configurar en nuestra apliacación Angularjs es el puerto con el que se levanto nuestra API, para eso necesitas modificar una contante global de nuestra aplicación Angularjs, para esto nos vamos al archivo app.js y veras el siguiente código:
```sh
application.constant('DOMAIN', {
  BASE_URL: 'http://localhost:5166/api',
  cache: false
});
```

Si el puerto con el que esta levantada nuestra API es diferente, basta con poner la correcta, y listo ya con esto deberas poder usar la aplicación.
****
![Angular Google Maps](https://github.com/CayetanoHerreraLuisRicardo/IA.Restaurant.Frontend/blob/main/assets/images/image1.png)