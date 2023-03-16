#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Services/Budget/Budget.API/Budget.API.csproj", "Services/Budget/Budget.API/"]
COPY ["Services/Budget/Budget.Application/Budget.Application.csproj", "Services/Budget/Budget.Application/"]
COPY ["Services/Budget/Budget.Infrastructure/Budget.Infrastructure.csproj", "Services/Budget/Budget.Infrastructure/"]
COPY ["Services/Budget/Budget.Domain/Budget.Domain.csproj", "Services/Budget/Budget.Domain/"]
COPY ["Services/Identity/Identity.API/Identity.API.csproj", "Services/Identity/Identity.API/"]
RUN dotnet restore "Services/Budget/Budget.API/Budget.API.csproj"
COPY . .
WORKDIR "/src/Services/Budget/Budget.API"
RUN dotnet build "Budget.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Budget.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Budget.API.dll"]