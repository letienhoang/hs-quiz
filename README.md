# Holwn Quiz Project

# Application URLs:
- Identity: https://localhost:5001
- Exam API: https://localhost:5002
- Exam Admin: https://localhost:6001
- Exam Portal: https://localhost:6002

# Docker Command Examples
## Build with command
- docker run -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=Abc123456$" -p 1434:1433 -d mcr.microsoft.com/mssql/server:2022-latest
- docker run -d --name mongodb -e MONGO_INITDB_ROOT_USERNAME=mongoadmin -e MONGO_INITDB_ROOT_PASSWORD=Abc123456$ -p 27017:27017 mongo

## Build with docker compose
- docker compose up

## Check container
- docker ps or docker container ls
- docker compose logs --follow 

