FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["src/API/API.csproj", "src/API/"]
COPY ["src/Application/Application.csproj", "src/Application/"]
COPY ["src/Domain/Domain.csproj", "src/Domain/"]
COPY ["src/Infra.Data/Infra.Data.csproj", "src/Infra.Data/"]
COPY ["src/Infra.Ioc/Infra.Ioc.csproj", "src/Infra.Ioc/"]
RUN dotnet restore "src/API/API.csproj"

COPY . .
RUN dotnet publish "src/API/API.csproj" -c Release -o /app/publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app

COPY --from=build /app/publish .

ENV ASPNETCORE_HTTP_PORTS=8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "API.dll"]
