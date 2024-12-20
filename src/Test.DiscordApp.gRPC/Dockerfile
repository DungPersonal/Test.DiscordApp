# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:9.0-alpine AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:9.0-alpine AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Directory.Build.props", "."]
COPY ["src/Test.DiscordApp.gRPC/Test.DiscordApp.gRPC.csproj", "src/Test.DiscordApp.gRPC/"]
COPY ["src/SharedKernel.Protos/SharedKernel.Protos.csproj", "src/SharedKernel.Protos/"]
COPY ["src/Test.DiscordApp.Application/Test.DiscordApp.Application.csproj", "src/Test.DiscordApp.Application/"]
COPY ["src/SharedKernel.Model/SharedKernel.Model.csproj", "src/SharedKernel.Model/"]
COPY ["src/SharedKernel.Utility/SharedKernel.Utility.csproj", "src/SharedKernel.Utility/"]
COPY ["src/Test.DiscordApp.Domain/Test.DiscordApp.Domain.csproj", "src/Test.DiscordApp.Domain/"]
COPY ["src/Test.DiscordApp.Infrastructure/Test.DiscordApp.Infrastructure.csproj", "src/Test.DiscordApp.Infrastructure/"]
RUN dotnet restore "./src/Test.DiscordApp.gRPC/Test.DiscordApp.gRPC.csproj"
COPY . .
WORKDIR "/src/src/Test.DiscordApp.gRPC"
RUN dotnet build "./Test.DiscordApp.gRPC.csproj" -c $BUILD_CONFIGURATION -o /app/build --no-restore
RUN apt-get update && apt-get install -y --no-install-recommends \
    ca-certificates libc6 libgcc-s1 libicu72 libssl3 libstdc++6 tzdata \
    && apt-get clean && rm -rf /var/lib/apt/lists/*

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Test.DiscordApp.gRPC.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false --no-restore

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Test.DiscordApp.gRPC.dll"]