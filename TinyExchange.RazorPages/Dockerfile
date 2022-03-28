FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["TinyExchange/TinyExchange.csproj", "TinyExchange/"]
RUN dotnet restore "TinyExchange/TinyExchange.csproj"
COPY . .
WORKDIR "/src/TinyExchange"
RUN dotnet build "TinyExchange.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TinyExchange.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TinyExchange.dll"]
