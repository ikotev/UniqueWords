# UniqueWords
This is a demo project trying to solve common challenge - insert new entity into a DB Table only if the entity does not exist yet. It has to work in highly concurrent system without unique key violation errors or deadlocks.

Two strategies are implemented:
* Synchronize addition of new items with DB locks
* Synchronize addition of new items with back-end service

## Technologies
* .NET Standard 2.1
* ASP .NET Core 3.1
* Entity Framework Core 3.1

### Other things you can find in the project
* The project is implemented following the principles of Clean Architecture
* Repository Pattern
* ASP.NET CORE template
