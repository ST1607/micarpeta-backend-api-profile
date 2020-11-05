#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["MiCarpeta.Profile/MiCarpeta.Profile.Presentation.csproj", "MiCarpeta.Profile/"]
COPY ["MiCarpeta.Domain/MiCarpeta.Domain.csproj", "MiCarpeta.Domain/"]
COPY ["MiCarpeta.Domain.Entities/MiCarpeta.Domain.Entities.csproj", "MiCarpeta.Domain.Entities/"]
COPY ["MiCarpeta.Repository/MiCarpeta.Repository.csproj", "MiCarpeta.Repository/"]
COPY ["MiCarpeta.Common/MiCarpeta.Common.csproj", "MiCarpeta.Common/"]
COPY ["MiCarpeta.Application/MiCarpeta.Application.csproj", "MiCarpeta.Application/"]
RUN dotnet restore "MiCarpeta.Profile/MiCarpeta.Profile.Presentation.csproj"
COPY . .
WORKDIR "/src/MiCarpeta.Profile"
RUN dotnet build "MiCarpeta.Profile.Presentation.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MiCarpeta.Profile.Presentation.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MiCarpeta.Profile.Presentation.dll"]
