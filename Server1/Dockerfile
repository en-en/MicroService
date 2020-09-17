FROM microsoft/dotnet:2.2-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /src
COPY ["Server1/Server1.csproj", "Server1/"]
RUN dotnet restore "Server1/Server1.csproj"
COPY . .
WORKDIR "/src/Server1"
RUN dotnet build "Server1.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "Server1.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Server1.dll"]