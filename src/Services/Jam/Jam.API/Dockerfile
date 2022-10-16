#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Services/Jam/Jam.API/Jam.API.csproj", "Services/Jam/Jam.API/"]
COPY ["MessagesAndEvents/MessagesAndEvents/MessagesAndEvents.csproj", "MessagesAndEvents/MessagesAndEvents/"]
COPY ["Services/Jam/Jam.Domain/Jam.Domain.csproj", "Services/Jam/Jam.Domain/"]
COPY ["Services/Jam/Jam.Infrastructure/Jam.Infrastructure.csproj", "Services/Jam/Jam.Infrastructure/"]
RUN dotnet restore "Services/Jam/Jam.API/Jam.API.csproj"
COPY . .
WORKDIR "/src/Services/Jam/Jam.API"
RUN dotnet build "Jam.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Jam.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Jam.API.dll"]