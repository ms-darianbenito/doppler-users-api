#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:6.0 AS build
WORKDIR /src
COPY ["Doppler.UsersApi.csproj", ""]
RUN dotnet restore "./Doppler.UsersApi.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "Doppler.UsersApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Doppler.UsersApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Doppler.UsersApi.dll"]
