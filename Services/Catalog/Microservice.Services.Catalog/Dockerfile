FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim as build
WORKDIR /app
EXPOSE 80
COPY Shared/Microservice.Shared/*.csproj Shared/FreeCourse.Shared/
COPY Services/Catalog/Microservice.Services.Catalog/*.csproj Services/Catalog/Microservice.Services.Catalog/
RUN dotnet restore Services/Catalog/Microservice.Services.Catalog/*.csproj
COPY . .
RUN dotnet publish Services/Catalog/Microservice.Services.Catalog/*.csproj -c Release -o out
FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim as runtime
WORKDIR /app
COPY --from=build /app/out .
ENTRYPOINT [ "dotnet","Microservice.Services.Catalog.dll" ]