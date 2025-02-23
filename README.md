[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://github.com/crisanchez/CleanArchitectureAppK/blob/master/LICENSE)
[![Docker Image CI](https://github.com/DosEspaciosDev/template-dotnet-webapi/actions/workflows/docker-image-validation.yml/badge.svg)](https://github.com/DosEspaciosDev/template-dotnet-webapi/actions/workflows/docker-image-validation.yml)

## Contents

<!--TOC-->
  - [Purpose](#purpose)
    - [Features included](#features-included)
  - [General rules for contributing](#general-rules-for-contributing)
    - [Getting Started](#getting-started)
    - [Running the application](#running-the-application)
  - [Configurations](#configurations)
    - [General Structure](#general-structure)
  - [Tutorials](#tutorials)
    - [Generate changelog](#generate-changelog)
    - [Create a new repository using this repository](#create-a-new-repository-using-this-repository)
      - [How to merge changes from template repository](#how-to-merge-changes-from-template-repository)
    - [Adding migrations for EF Core](#adding-migrations-for-ef-core)
<!--/TOC-->

## Purpose

The purpose is to have a quick and complete starting point for the development of `.net core 8 API Projects` that include the most essential features needed.

### Features included
- [x] Built on .NET 8.0
- [x] Follows Clean Architecture Principles
- [x] Domain Driven Design
- [x] Specification pattern
<details>
    <summary>Click to see the full list</summary>

- [x] Husky.Net for git hook tasks
- [x] MediatR - CQRS
- [x] ApiClient abstraction
- [x] Multidatabase support (MySQL, Oracle & PostgreSQL)
- [x] Uses Entity Framework Core as DB Abstraction
- [x] Flexible Repository Pattern
- [x] Dapper Integration for Optimal Performance
- [x] Serilog Integration
- [x] OpenAPI - Supports Client Service Generation
- [x] Mapster Integration for Quicker Mapping
- [x] API Versioning (Allows versioned & version neutral controllers)
- [x] In Memory Response Caching
- [ ] Distributed Response Caching with REDIS
- [x] Fluent Validation for Request validation
- [x] Code Analysis & StyleCop Integration with Rulesets
- [x] JSON Based Localization
- [x] Test Projects (xUnit)
- [x] SignalR Notifications
- [ ] JWT & Cognito Authentication
</details>

## General rules for contributing
 
 There exists a hook when committing that checks for right formatted committed messages.
 
 https://github.com/conventional-changelog/commitlint

## Getting Started

> Make sure you have your development environment ready (.net 8 SDK // IDE // database server)

The solution comes with some examples to quick test & try

### Running the application
> We will assume that you will run this part using Visual Studio Code

Open a command prompt on the repository folder and run the following command

``` code . ```

This will open the solution.

Now, let's set up a valid connection string. Navigate to `src/Host/Configurations` and open up `database.json`, provide a valid MySQL connection string.


Open a command prompt on the repository folder and run the solution

```
 cd src/Host
 dotnet build
 dotnet run
```

The you will see something like this:
![Console_dotnet_run](https://user-images.githubusercontent.com/37070054/162247883-d9702a18-f849-43e3-ac4e-b7f69a1b9065.PNG)

As you can see from the logs, the Migrations that already come out-of-the-box with the application gets applied. Note that you do not have to manually update the database using code.

If you want to see the swagger definition, just open your browser and navigate to `https://localhost:7185/swagger/index.html`

![Swagger]()

Here you have all the available endpoints in the demo.
> Please note that for the Twitter section to work, you need to provide your own twitter credential in the apiclientsettings.json configuration file 

## Configurations
Within the Host boot project there is a folder called “Configurations”. where there are all the configuration files, one for each area.

### General Structure
```
├── Host.csproj
│   ├── Configurations
│   |   ├── apiclientsettings.json
│   |   ├── cache.json
│   |   ├── cors.json
│   |   ├── database.json
│   |   ├── logger.json
│   |   ├── middleware.json
│   |   ├── openapi.json
│   |   ├── security.json
│   |   ├── securityheaders.json
│   |   └── signalr.json
|   ├── appsettings.json
|
```
The Startup class inside the folder is responsible for loading all the configuration files described above.

## Tutorials
### Generate changelog
In conjunction with commitlint, we can auto-generate significant Changelogs using a [Changelog tool](https://github.com/lob/generate-changelog)
> You will need to have node installed to follow this steps

First, you need to restore the npm packages, in the root directory run `npm install`

This will restore the changelog tool, after that you can simply run `changelog`

after that you will have a Changelog.md file created with the contents. For more usage information review the [cli documentation](https://github.com/lob/generate-changelog)

### Adding migrations for EF Core
Current supported databases are:
- MySQL
- PostgreSQL
- Oracle

Each provider has its own Migration project:
- Migrators/Migrators.MySQL
- Migrators/Migrators.PostgreSQL
- Migrators/Migrators.Oracle

The configuration for the database is found in the `database.json` file.

If you need to create/update migrations for MySQL you will need:
1. Mysql Server Up & Running (with docker installed you could run `docker run --name defaultmysql -e MYSQL_ROOT_PASSWORD=my-secret-pw -e MYSQL_DATABASE=db -p 3306:3306 mysql:lastest`)
2. Have a valid connection string to the MySQL Server updated on `database.json` configuration file. This also assumes that you have updated `"DBProvider": "mysql"` too.

Below are some sample configurations for MySQL Provider. The above is applicable to all the other DB Provider.

database.json:
```
{
  "DatabaseSettings": {
    "UsePersistence": true,
    "DBProvider": "mysql",
    "ConnectionString": "Server=localhost;Port=3306;Database=db;Uid=root;Pwd=my-secret-pw;"
  }
}
```

The Provider values for other supported DBs are as follows.
- PostgreSQL - postgresql
- Oracle - oracle

For initializing the database, you need to go to the Host project directory and run:
`dotnet ef migrations add InitialCreate --project .././Migrators/Migrators.<DBProvider>/ --context ApplicationDbContext -o Migrations/Application`

Every time you need to update the schema, you would run something like:
`dotnet ef migrations add <CommitMessage> --project .././Migrators/Migrators.<DBProvider>/ --context ApplicationDbContext -o Migrations/Application`

where
- <CommitMessage> should be replaced by an appropriate name that describes the Migration
- <DBProvider> should be replaced by your selected Database Provider (MSSQL, MySQL, Oracle or PostgreSQL)

Once the process is completed you would be able see new Migration cs files that represent your new additions / modifications at the table level added to the respective Migrator project.

You do not have to do anything extra to apply the migrations to your database. The application does it for you during the startup. GLHF!<!--TOC-->
