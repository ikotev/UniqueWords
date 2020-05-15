# UniqueWords
This is a demo project trying to solve common challenge: insertion of a new item into a DB Table only if the item does not exist yet. It has to work in highly concurrent system without unique key violation errors or deadlocks.

Two strategies are implemented:
- Synchronize addition of new items with DB locks
- Synchronize addition of new items with back-end service

## Technologies
* .NET Standard 2.1
* ASP .NET Core 3.1
* Entity Framework Core 3.1
