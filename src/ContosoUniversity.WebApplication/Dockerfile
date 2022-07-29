FROM mcr.microsoft.com/dotnet/aspnet:3.1 as base

WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:3.1 as build
WORKDIR /src
COPY ContosoUniversity.WebApplication/ContosoUniversity.WebApplication.csproj ContosoUniversity.WebApplication/
RUN dotnet restore ContosoUniversity.WebApplication/ContosoUniversity.WebApplication.csproj
COPY . .
WORKDIR /src/ContosoUniversity.WebApplication
RUN dotnet build ContosoUniversity.WebApplication.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish ContosoUniversity.WebApplication.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "ContosoUniversity.WebApplication.dll"]