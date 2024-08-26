FROM mcr.microsoft.com/dotnet/aspnet:8.0.8 AS base-env

USER app
EXPOSE 8080

WORKDIR /app

# Configure ASP.NET Core
ENV ASPNETCORE_ENVIRONMENT=Production
ENV ASPNETCORE_URLS=http://*:8080
ENV ASPNETCORE_HTTP_PORTS=
ENV ASPNETCORE_HTTPS_PORTS=

# Configure OpenTelemetry .NET Auto-Instrumentation
ENV OTEL_DOTNET_AUTO_HOME=/otel-dotnet-auto

# Just activate following lines to troubleshoot the OpenTelemetry .NET Auto-Instrumentation (Enable Console Exporter disable exporte to provider)
# ENV OTEL_DOTNET_AUTO_TRACES_CONSOLE_EXPORTER_ENABLED=true
# ENV OTEL_DOTNET_AUTO_METRICS_CONSOLE_EXPORTER_ENABLED=true
# ENV OTEL_DOTNET_AUTO_LOGS_CONSOLE_EXPORTER_ENABLED=true
# ENV OTEL_TRACES_EXPORTER=none
# ENV OTEL_METRICS_EXPORTER=none
# ENV OTEL_LOGS_EXPORTER=none



FROM mcr.microsoft.com/dotnet/sdk:8.0.401 AS build-env

# Install Auto Insturmentation
WORKDIR /otel-dotnet-auto

RUN apt-get update && apt-get install -y unzip
ENV OTEL_DOTNET_AUTO_HOME=/otel-dotnet-auto

ENV OTEL_VERSION=1.7.0
ADD https://github.com/open-telemetry/opentelemetry-dotnet-instrumentation/releases/download/v${OTEL_VERSION}/otel-dotnet-auto-install.sh otel-dotnet-auto-install.sh

RUN chmod +x ./otel-dotnet-auto-install.sh
RUN sh otel-dotnet-auto-install.sh


# Build the application
WORKDIR /src

# Copy just the project files and restore the nuget packages
COPY ./src/Demo.Api.Users/*.csproj ./Demo.Api.Users/

# Restore nuget packages
RUN dotnet restore ./Demo.Api.Users/*.csproj

# Copy all the source code and build
COPY ./src/Demo.Api.Users/ ./Demo.Api.Users/

# Build and publish the application. Used the "--no-restore" and "--no-build" to benefit the layer caches
RUN dotnet build ./Demo.Api.Users/*.csproj \
    -c Release \
    --no-restore

RUN dotnet publish ./Demo.Api.Users/*.csproj \
    -c Release \
    --no-build \
    --no-restore \
    -o /publish



FROM base-env AS run-env

COPY --from=build-env /publish .
COPY --from=build-env /otel-dotnet-auto /otel-dotnet-auto

ENTRYPOINT ["/bin/bash", "-c", "source /otel-dotnet-auto/instrument.sh && dotnet Api.Users.dll"]
