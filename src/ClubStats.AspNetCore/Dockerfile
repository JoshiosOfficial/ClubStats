FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/ClubStats.AspNetCore/ClubStats.AspNetCore.csproj", "ClubStats.AspNetCore/"]
RUN dotnet restore "src/ClubStats.AspNetCore/ClubStats.AspNetCore.csproj"
COPY . .
WORKDIR "/src/ClubStats.AspNetCore"
RUN dotnet build "ClubStats.AspNetCore.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ClubStats.AspNetCore.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ClubStats.AspNetCore.dll"]
