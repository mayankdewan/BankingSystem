# Use the official ASP.NET Core runtime as a base image
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

# Use the SDK image to build the app
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["BankingSystem/BankingSystem.csproj", "BankingSystem/"]
RUN dotnet restore "BankingSystem/BankingSystem.csproj"
COPY . .
WORKDIR "/src/BankingSystem"
RUN dotnet build "BankingSystem.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BankingSystem.csproj" -c Release -o /app/publish

# Use the base image and copy the build output
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BankingSystem.dll"]