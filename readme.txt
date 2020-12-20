Overview
--------
Demonstrates how to setup JWT Token authentication for a Web API.
Authentication is configured in Startup.cs to use JWT authentication. A JWT 
token is an encrypted JSON string which contains a list of claims. The 
example adds two claims 'user' and 'expiresOn'. The server creates a JWT 
token on successful authentication & returns it to the client in the response.
The client is responsible for passing this token in 'Authorization' header in 
the format 'Bearer <token>'.  

JWTTokenService class
---------------------
Is responsible to validating the username and password and creating a JWT 
token. The JSON is encrypted with a key which is computed once at application 
startup by the JWTTokenService as the JWTTokenService is registered as a singleton 
service. Users are specified in the appsettings.json file. 

AuthenticationController class
------------------------------
Has a Post() method which expects the caller to send the username and password 
in the post body as a Json string:
{ "Username": "siddharth", "Password": "password" }
It makes use of the JWTTokenService class to validate the username and password 
and return the generated JWT token. 

WeatherForecastController class
-------------------------------
Has a Get() method protected by the 'Authorization' attribute.


