FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /

# Copy csproj and restore as distinct layers
COPY ["Directory.Build.props", "/"]
COPY ["Directory.Build.targets", "/"]
COPY ["dotnet.ruleset", "/"]
COPY ["stylecop.json", "/"]
COPY ["src/Host/Host.csproj", "src/Host/"]
COPY ["src/Application/Application.csproj", "src/Application/"]
COPY ["src/Domain/Domain.csproj", "src/Domain/"]
COPY ["src/Shared/Shared.csproj", "src/Shared/"]
COPY ["src/Infrastructure/Infrastructure.csproj", "src/Infrastructure/"]
COPY ["src/Migrators/Migrators.MySQL/Migrators.MySQL.csproj", "src/Migrators/Migrators.MySQL/"]
COPY ["src/Migrators/Migrators.PostgreSQL/Migrators.PostgreSQL.csproj", "src/Migrators/Migrators.PostgreSQL/"]
COPY ["src/Migrators/Migrators.Oracle/Migrators.Oracle.csproj", "src/Migrators/Migrators.Oracle/"]
ENV HUSKY=0
RUN dotnet restore "src/Host/Host.csproj" --disable-parallel

# Copy everything else and build
COPY . .
WORKDIR "/src/Host"
RUN dotnet dev-certs https
RUN dotnet publish "Host.csproj" -c Release -o /app/publish

# Build the runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app

COPY --from=build /app/publish .
COPY --from=build /root/.dotnet/corefx/cryptography/x509stores/my/* /root/.dotnet/corefx/cryptography/x509stores/my/

# Setting the timezone is needed for oracle driver
ENV TZ=CET

ENV ASPNETCORE_URLS=https://+:5050;http://+:5060
EXPOSE 5050
EXPOSE 5060

ENTRYPOINT ["dotnet", "appk.WebApi.Host.dll"]