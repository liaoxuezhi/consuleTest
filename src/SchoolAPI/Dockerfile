FROM microsoft/dotnet:2.2-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /src
COPY ["SchoolAPI.csproj", "src/SchoolAPI/"]
RUN dotnet restore "src/SchoolAPI/SchoolAPI.csproj"
COPY . .
WORKDIR "/src/SchoolAPI"
RUN dotnet build "SchoolAPI.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "SchoolAPI.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "SchoolAPI.dll"]