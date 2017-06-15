1- Introducci�n
---------------
EjercicioWebApp est� desarrollado con Visual Studio 2017. La soluci�n consta de tres proyetos:

- EjercicioWebApp (proyecto principal). Es un proyecto Web MVC 5 que contiene toda la navegaci�n de usuarios del ejercicio.
- EjercicioWebApi. Es un proyecto Web MVC con WebApi 2. Publica los m�todos para operaciones CRUD de la entidad User.
				   Deber�a estar escuchando en "http://localhost:50665". 
- Ejercicio.Domain. Proyecto biblioteca de clases con las entidades/modelos que han de manejar tanto el proyecto web, como el proyecto con la WebApi.
					Ambos proyectos tienen dependencia de este.
					
He separado en dos proyectos diferentes la WebApp y la WebApi para simular que la WebApi puede estar publicada 
en un sitio web distinto a la WebApp, y ah� la gracia de la comunicaci�n a trav�s de WebApi.



2- Probar
---------
Si EjercicioWebApp est� definido como proyecto de inicio bastar�a con compilar y ejecutar la
soluci�n para que aparezca la pantalla de login.

Inicialmente hay 4 usuarios dados de alta para probar:

UserName 	Password	Roles	
user1		user1		PAGE_1	
user2		user2		PAGE_2	
user3		user3		PAGE_3	
admin		admin		PAGE_1;PAGE_2;PAGE_3;ADMIN



3- Proyecto Ejercicio.Domain
----------------------------
Define la entidad User, que utilizan la WebApp y la WebApi. 
La clase User tiene los atributos UserName, Password, Roles e Id. He a�adido el campo Id como clave primaria
para simular que trabajamos con registros de una tabla. Podr�a haber utilizado el campo UserName como clave
primaria, pero me ha parecido m�s apropiado introducir el campo Id para permitir editar el UserName.



4- Proyecto EjercicioWebApp
---------------------------
La WebApp utiliza autenticaci�n Forms para el control de roles de usuarios.
Para ello se define en el Web.Config la URL para hacer login y el timeout de la sesi�n (5 minutos).

<authentication mode="Forms">
  <forms loginUrl="~/Home/Login" timeout="5" />
</authentication>
	
Tiene varias clases relevantes:

- WebApiClient. Maneja todas las peticiones a la WebApi para operaciones CRUD, que se realizan de forma s�ncrona (por simplicidad lo he hecho as� en el ejercicio)
                He querido encapsular todos los accesos a la WebApi desde una �nica clase, que sea la �nica que tenga esa responsabilidad.
				En cada operaci�n se comprueban los c�digos de respuesta devueltos por la WebApi y se lanza una
				excepci�n si ha habido alg�n problema.
				
- CustomAuthorizeAttribute. Permite que si un usuario est� autenticado y no tiene el rol indicado para el recurso
							al que est� accediendo, se le redirija a una p�gina de error apropiada.
							
- ErrorController. Controller para las p�ginas de error customizadas. He creado este controller por si hubiera
				   m�s de una p�gina. De momento s�lo la de acceso prohibido "El usuario no tiene permisos para ver esta p�gina"
				
- HomeController. Es el controlador (junto con las vistas asociadas) para toda la navegaci�n de los usuarios: Login, Page1, Page2, Page3, LogOff, P�gina de bienvenida (UserDashBoard).
				  Utiliza la clase User (de Ejercicio.Domain) como modelo.
				  En el Login se recupera el usuario a partir del UserName, mediante el WebApiClient. Si el login es correcto se crea la cookie de autenticaci�n.
				  Cada acci�n del controller tiene su propio tag [CustomAuthorize] seg�n el rol que tenga acceso  al recurso.
				  
- Global.asax.cs. En el evento Application_PostAuthenticateRequest se asignan el usuario y sus roles. 
				  Los roles se recuperan de la WebApi (llamada a trav�s de WebApiClient) a partir del
				  UserName, que se extrae de la cookie del usuario autenticado.
				  
- UsersController. Es el controlador para todo el mantenimiento de usuarios. Mediante el tag [CustomAuthorize(Roles = "ADMIN")]
				   se asegura que s�lo el rol de administraci�n tiene acceso. 
				   Utiliza la clase User (de Ejercicio.Domain) como modelo.
				   Las distintas acciones y vistas permiten todas las operaciones CRUD de usuarios conta la WebApi 
				   (a trav�s de WebApiClient).


				   
Proyecto EjercicioWebApi
------------------------
Clases a comentar:

- Repository\InMemoryDB.cs. Para la persistencia de los datos de usuarios he utilizado una clase para 
							tenerlos en memoria. Se trata de un patr�n singleton que permite que todas
							las instancias de WebApp vean los mismos datos de la WebApi (mientras esta �ltima
							no se reinicie).
							
- UserController.cs. ApiController que publica todas las operaciones CRUD sobre el modelo User. 
					 Retorna los distintos c�digos de respuesta http en funci�n del resultado de la 
					 operaci�n realizada.
							