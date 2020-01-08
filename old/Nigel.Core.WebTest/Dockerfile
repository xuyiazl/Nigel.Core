FROM mcr.microsoft.com/dotnet/core/aspnet:3.0-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.0-buster AS build
WORKDIR /src
COPY ["Nigel.Core.WebTest/Nigel.Core.WebTest.csproj", "Nigel.Core.WebTest/"]
RUN dotnet restore "Nigel.Core.WebTest/Nigel.Core.WebTest.csproj"
COPY . .
WORKDIR "/src/Nigel.Core.WebTest"
RUN dotnet build "Nigel.Core.WebTest.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Nigel.Core.WebTest.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Nigel.Core.WebTest.dll"]