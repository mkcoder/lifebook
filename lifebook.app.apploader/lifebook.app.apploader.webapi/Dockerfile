FROM microsoft/dotnet:2.2-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /src
COPY lifebook.app.apploader.webapi/lifebook.app.apploader.webapi.csproj lifebook.app.apploader.webapi/
COPY lifebook.app.apploader.services/lifebook.app.apploader.services.csproj lifebook.app.apploader.services/
RUN dotnet restore lifebook.app.apploader.webapi/lifebook.app.apploader.webapi.csproj
COPY . .
WORKDIR /src/lifebook.app.apploader.webapi
RUN dotnet build lifebook.app.apploader.webapi.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish lifebook.app.apploader.webapi.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "lifebook.app.apploader.webapi.dll"]
