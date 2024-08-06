FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["WebGestao/WebGestao.csproj", "WebGestao/"]
COPY ["Infrastructure/Infrastructure.csproj", "Infrastructure/"]
COPY ["Core/Core.csproj", "Core/"]
COPY ["Authentication/Authentication.csproj", "Authentication/"]
RUN dotnet restore "WebGestao/WebGestao.csproj"
COPY . .
WORKDIR "/src/WebGestao"
RUN dotnet build "WebGestao.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "WebGestao.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
COPY entrypoint.sh .
ENV ASPNETCORE_URLS=http://host.docker.internal:8080
ENTRYPOINT ["./entrypoint.sh"]