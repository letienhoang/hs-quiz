# Holwn Quiz Project

# Application URLs:
- Identity: https://localhost:5001
- Identity Admin: https://localhost:5002
- Identity Admin Api: https://localhost:5003
- Identity Admin UI: https://localhost:5103
- Exam API: https://localhost:6001
- Exam Admin: https://localhost:6101
- Exam Portal: https://localhost:6101

# Docker Command Examples
## Build with command
- docker run -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=Abc123456$" -p 1434:1433 -d mcr.microsoft.com/mssql/server:2022-latest
- docker run -d --name mongodb -e MONGO_INITDB_ROOT_USERNAME=mongoadmin -e MONGO_INITDB_ROOT_PASSWORD=Abc123456$ -p 27017:27017 mongo

## Build with docker compose
- docker compose up

## Check container
- docker ps or docker container ls
- docker compose logs --follow 

## Bundling and Minification

The following Gulp commands are available:

- `gulp fonts` - copy fonts to the `dist` folder
- `gulp styles` - minify CSS, compile SASS to CSS
- `gulp scripts` - bundle and minify JS
- `gulp clean` - remove the `dist` folder
- `gulp build` - run the `styles` and `scripts` tasks
- `gulp watch` - watch all changes in all sass files

## EF Core & Data Access

- The solution uses these `DbContexts`:

  - `AdminIdentityDbContext`: for Asp.Net Core Identity
  - `AdminLogDbContext`: for logging
  - `IdentityServerConfigurationDbContext`: for IdentityServer configuration store
  - `IdentityServerPersistedGrantDbContext`: for IdentityServer operational store
  - `AdminAuditLogDbContext`: for Audit Logging
  - `IdentityServerDataProtectionDbContext`: for dataprotection

### Run entity framework migrations:

> NOTE: Initial migrations are a part of the repository.

  - It is possible to use powershell script in folder `build/add-migrations.ps1`.
  - This script take two arguments:
    - --migration (migration name)
    - --migrationProviderName (provider type - available choices: All, SqlServer, MySql, PostgreSQL)

- For example: 
`.\add-migrations.ps1 -migration DbInit -migrationProviderName SqlServer`

