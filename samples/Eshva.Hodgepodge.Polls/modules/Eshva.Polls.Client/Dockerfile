FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
ARG configuration=Debug
WORKDIR /src
COPY . .
RUN dotnet publish "Eshva.Polls.Client.WebApp/Eshva.Polls.Client.WebApp.csproj" -c ${configuration} -o /app

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1
WORKDIR /app
EXPOSE 80
COPY --from=build /app .
ENTRYPOINT ["dotnet", "Eshva.Polls.Client.WebApp.dll"]
