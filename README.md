1- Introducción
---------------
EjercicioWebApp está desarrollado con Visual Studio 2017. La solución consta de tres proyetos:

- EjercicioWebApp (proyecto principal). Es un proyecto Web MVC 5 que contiene toda la navegación de usuarios del ejercicio.
- EjercicioWebApi. Es un proyecto Web MVC con WebApi 2. Publica los métodos para operaciones CRUD de la entidad User.
				   Debería estar escuchando en "http://localhost:50665". 
- Ejercicio.Domain. Proyecto biblioteca de clases con las entidades/modelos que han de manejar tanto el proyecto web, como el proyecto con la WebApi.
					Ambos proyectos tienen dependencia de este.
					
He separado en dos proyectos diferentes la WebApp y la WebApi para simular que la WebApi puede estar publicada 
en un sitio web distinto a la WebApp, y ahí la gracia de la comunicación a través de WebApi.



2- Probar
---------
Si EjercicioWebApp está definido como proyecto de inicio bastaría con compilar y ejecutar la
solución para que aparezca la pantalla de login.

Inicialmente hay 4 usuarios dados de alta para probar:

UserName: user1		Password: user1		Roles: PAGE_1	

UserName:user2		Password: user2		Roles: PAGE_2	

UserName:user3		Password: user3		Roles: PAGE_3	

UserName:admin		Password: admin		Roles: PAGE_1;PAGE_2;PAGE_3;ADMIN



3- Proyecto Ejercicio.Domain
----------------------------
Define la entidad User, que utilizan la WebApp y la WebApi. 
La clase User tiene los atributos UserName, Password, Roles e Id. He añadido el campo Id como clave primaria
para simular que trabajamos con registros de una tabla. Podría haber utilizado el campo UserName como clave
primaria, pero me ha parecido más apropiado introducir el campo Id para permitir editar el UserName.



4- Proyecto EjercicioWebApp
---------------------------
La WebApp utiliza autenticación Forms para el control de roles de usuarios.
Para ello se define en el Web.Config la URL para hacer login y el timeout de la sesión (5 minutos).

<authentication mode="Forms">
  <forms loginUrl="~/Home/Login" timeout="5" />
</authentication>
	
Tiene varias clases relevantes:

- WebApiClient. Maneja todas las peticiones a la WebApi para operaciones CRUD, que se realizan de forma síncrona (por simplicidad lo he hecho así en el ejercicio)
                He querido encapsular todos los accesos a la WebApi desde una única clase, que sea la única que tenga esa responsabilidad.
				En cada operación se comprueban los códigos de respuesta devueltos por la WebApi y se lanza una
				excepción si ha habido algún problema.
				
- CustomAuthorizeAttribute. Permite que si un usuario está autenticado y no tiene el rol indicado para el recurso
							al que está accediendo, se le redirija a una página de error apropiada.
							
- ErrorController. Controller para las páginas de error customizadas. He creado este controller por si hubiera
				   más de una página. De momento sólo la de acceso prohibido "El usuario no tiene permisos para ver esta página"
				
- HomeController. Es el controlador (junto con las vistas asociadas) para toda la navegación de los usuarios: Login, Page1, Page2, Page3, LogOff, Página de bienvenida (UserDashBoard).
				  Utiliza la clase User (de Ejercicio.Domain) como modelo.
				  En el Login se recupera el usuario a partir del UserName, mediante el WebApiClient. Si el login es correcto se crea la cookie de autenticación.
				  Cada acción del controller tiene su propio tag [CustomAuthorize] según el rol que tenga acceso  al recurso.
				  
- Global.asax.cs. En el evento Application_PostAuthenticateRequest se asignan el usuario y sus roles. 
				  Los roles se recuperan de la WebApi (llamada a través de WebApiClient) a partir del
				  UserName, que se extrae de la cookie del usuario autenticado.
				  
- UsersController. Es el controlador para todo el mantenimiento de usuarios. Mediante el tag [CustomAuthorize(Roles = "ADMIN")]
				   se asegura que sólo el rol de administración tiene acceso. 
				   Utiliza la clase User (de Ejercicio.Domain) como modelo.
				   Las distintas acciones y vistas permiten todas las operaciones CRUD de usuarios conta la WebApi 
				   (a través de WebApiClient).


				   
5- Proyecto EjercicioWebApi
---------------------------
Clases a comentar:

- Repository\InMemoryDB.cs. Para la persistencia de los datos de usuarios he utilizado una clase para 
							tenerlos en memoria. Se trata de un patrón singleton que permite que todas
							las instancias de WebApp vean los mismos datos de la WebApi (mientras esta última
							no se reinicie).
							
- UserController.cs. ApiController que publica todas las operaciones CRUD sobre el modelo User. 
					 Retorna los distintos códigos de respuesta http en función del resultado de la 
					 operación realizada.
