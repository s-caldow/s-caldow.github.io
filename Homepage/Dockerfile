FROM microsoft/aspnetcore:2.0 AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/aspnetcore-build:2.0 AS build
WORKDIR /src
COPY Homepage/Homepage.csproj Homepage/
RUN dotnet restore Homepage/Homepage.csproj
COPY . .
WORKDIR /src/Homepage
RUN dotnet build Homepage.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish Homepage.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Homepage.dll"]
