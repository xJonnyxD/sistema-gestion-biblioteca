FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY BibliotecaWeb.csproj ./
RUN dotnet restore

COPY . ./
RUN dotnet publish -c Release -o /app --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app

COPY --from=build /app ./

ENV ASPNETCORE_HTTP_PORTS=8080
EXPOSE 8080

# La app corre como usuario no-root; debe poder escribir las imágenes subidas.
RUN mkdir -p /app/wwwroot/images && chown -R $APP_UID:0 /app/wwwroot/images

USER $APP_UID

ENTRYPOINT ["dotnet", "BibliotecaWeb.dll"]
