FROM mcr.microsoft.com/dotnet/aspnet:3.1 as base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:3.1 as build
WORKDIR /src
COPY ContosoUniversity.API/ContosoUniversity.API.csproj ContosoUniversity.API/
RUN dotnet restore ContosoUniversity.API/ContosoUniversity.API.csproj
COPY . .
WORKDIR /src/ContosoUniversity.API
RUN dotnet build ContosoUniversity.API.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish ContosoUniversity.API.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "ContosoUniversity.API.dll"]
