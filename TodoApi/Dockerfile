# ---------- Build ----------
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src/TodoApi

# (optional) faster CI
ENV DOTNET_CLI_TELEMETRY_OPTOUT=1
ENV NUGET_XMLDOC_MODE=skip

# 1) copy project file and restore (cache-friendly)
COPY ./TodoApi.csproj ./
RUN dotnet restore

# If you have these, copy them BEFORE restore:
# COPY ./Directory.Packages.props /src/
# COPY ./NuGet.config /src/

# 2) copy the rest of the source and restore again (fixes partial restore issues)
COPY . .
RUN dotnet restore

# 3) publish (allow restore just in case; also disable native apphost to shrink image)
RUN dotnet publish -c Release -o /app /p:UseAppHost=false

# ---------- Runtime ----------
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080
COPY --from=build /app .
ENTRYPOINT ["dotnet", "TodoApi.dll"]
