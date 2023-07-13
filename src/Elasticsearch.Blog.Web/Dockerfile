FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Elasticsearch.Blog.Web/Elasticsearch.Blog.Web.csproj", "Elasticsearch.Blog.Web/"]
RUN dotnet restore "Elasticsearch.Blog.Web/Elasticsearch.Blog.Web.csproj"
COPY . .
WORKDIR "/src/Elasticsearch.Blog.Web"
RUN dotnet build "Elasticsearch.Blog.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Elasticsearch.Blog.Web.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Elasticsearch.Blog.Web.dll"]
