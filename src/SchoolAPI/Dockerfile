FROM microsoft/aspnetcore:2.0 AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/aspnetcore-build:2.0 AS build
WORKDIR /src
COPY ["src/SchoolAPI/SchoolAPI.csproj", "src/SchoolAPI/"]
RUN dotnet restore "src/SchoolAPI/SchoolAPI.csproj"
COPY . .
WORKDIR "/src/src/SchoolAPI"
RUN dotnet build "SchoolAPI.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "SchoolAPI.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "SchoolAPI.dll"]