# ProyectoRedes
Proyecto Final Redes

El proyecto consiste en un sistema de envío de archivos, implementando la arquitectura cliente/servidor, por medio de un Socket stream TCP.

Se implementaron:

- Validacion de usuario del lado del servidor, almacenando los datos de usuario en una base de datos SQL.
- Sistema de cifrado de la contraseña con Salt y Master_Key para no almacenar las contraseñas en texto plano.

- Hilos, para el manejo de las conexiones entrantes de los clientes, cada cliente tiene su propio hilo de ejecución.

- Semaforo que solo permite enviar archivos a 2 usuarios al mismo tiempo, dejando los otros usuarios en espera.

- Patrón Singleton para gestión de una unica instancia de la conección a la base de datos.


Requerimientos:

Visual Studio con .Net Framework 4.7.2

SQL Server con localdb.

1- Ejecutar los scripts que se encuentran en _SCRIPT_DB:

Estos scrips crean la db local con la tabla y datos para realizar pruebas de validación

2- Abrir el proyecto ProyectoProgramacionSobreRedes

3- Dentro del Servidor, editar el archivo ConnSingleton

buscar: @"Data Source=<AGREGAR SERVER NAME>

Reemplazar el valor "data source=<AGREGAR SERVER NAME>" Según correspondan en la configuración locall de tu db
