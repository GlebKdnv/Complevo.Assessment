# Complevo.Assessment
This is an example application.
The project demonstrates an implementation of RESTful API on a resource named Product.

API is documented. Documentation is implemented with *OpenApi*.

The application uses *docker-compose* orchestrator, including a container with SQL Server for persistence. This approach makes deployment quick and stable and removes dependence on the deployment environment and server configuration.

API is tested with integration tests. Tests run without SQL container or SQL instance DB because "In memory DB" is used.

The application uses .NET 6.0 as the framework

There are two branches.
- **master** branch is without authentication
- **Auth** branch uses authentication with JWT tokens

besides that, they are identical

## Setup and execution
the application could be run with the command:
```
docker-compose up
```
executed on the root of the application folder.

The application could be debugged in Visual Studio while running in docker or as a stand-alone program, but it's configured to be connected to SQL Db container which must be active in both cases.

On the first run, the application will create DB in the SQL container.
## Routes
When run with *docker-compose*

REST endpoints are located [localhost:5443/Api/products](https://localhost:5443/Api/products)

Login endpoint (for **Auth** branch only) address is [localhost:5443/api/Account/Login](https://localhost:5443/api/Account/Login)
There is a single account, created for demonstration purposes:
```
UserName = User1
Password = password1
```
## OpenApi
When executed in *docker-compose* orchestrator application exposes it's *OpenApi Swagger Interface* at address [localhost:5443/swagger](https://localhost:5443/swagger/index.html)
