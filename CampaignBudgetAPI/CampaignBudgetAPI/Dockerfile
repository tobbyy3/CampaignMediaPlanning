FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["CampaignBudgetAPI.csproj", "./"]
RUN dotnet restore "CampaignBudgetAPI.csproj"
COPY . .
RUN dotnet publish "CampaignBudgetAPI.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "CampaignBudgetAPI.dll"]
