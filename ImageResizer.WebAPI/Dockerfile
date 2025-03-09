FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["ImageResizer.WebAPI/ImageResizer.WebAPI.csproj", "ImageResizer.WebAPI/"]
RUN dotnet restore "ImageResizer.WebAPI/ImageResizer.WebAPI.csproj"
COPY . .
WORKDIR "/src/ImageResizer.WebAPI"
RUN dotnet build "ImageResizer.WebAPI.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "ImageResizer.WebAPI.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ImageResizer.WebAPI.dll"]
