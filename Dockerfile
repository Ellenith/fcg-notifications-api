FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY FCG.NotificationsAPI/*.csproj FCG.NotificationsAPI/
RUN dotnet restore FCG.NotificationsAPI/FCG.NotificationsAPI.csproj
COPY . .
RUN dotnet publish FCG.NotificationsAPI/FCG.NotificationsAPI.csproj -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
RUN mkdir -p Logs
COPY --from=build /app/publish .
EXPOSE 8080
ENTRYPOINT ["dotnet", "FCG.NotificationsAPI.dll"]