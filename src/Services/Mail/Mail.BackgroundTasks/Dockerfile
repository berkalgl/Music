#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Services/Mail/Mail.BackgroundTasks/Mail.BackgroundTasks.csproj", "Services/Mail/Mail.BackgroundTasks/"]
COPY ["MessagesAndEvents/MessagesAndEvents/MessagesAndEvents.csproj", "MessagesAndEvents/MessagesAndEvents/"]
RUN dotnet restore "Services/Mail/Mail.BackgroundTasks/Mail.BackgroundTasks.csproj"
COPY . .
WORKDIR "/src/Services/Mail/Mail.BackgroundTasks"
RUN dotnet build "Mail.BackgroundTasks.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Mail.BackgroundTasks.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Mail.BackgroundTasks.dll"]