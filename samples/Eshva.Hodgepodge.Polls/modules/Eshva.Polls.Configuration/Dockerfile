FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
ARG configuration=Debug
WORKDIR /src
COPY . .
RUN dotnet publish "Eshva.Hodgepodge.Polls/modules/Eshva.Polls.Configuration/Eshva.Polls.Configuration.WebApp/Eshva.Polls.Configuration.WebApp.csproj" -c ${configuration} -o /app

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1
WORKDIR /app
EXPOSE 80
COPY --from=build /app .
ENTRYPOINT ["dotnet", "Eshva.Polls.Configuration.WebApp.dll"]
