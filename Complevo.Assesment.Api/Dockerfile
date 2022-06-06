#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Complevo.Assesment.Api/Complevo.Assesment.Api.csproj", "Complevo.Assesment.Api/"]
RUN dotnet restore "Complevo.Assesment.Api/Complevo.Assesment.Api.csproj"
COPY . .
WORKDIR "/src/Complevo.Assesment.Api"
RUN dotnet build "Complevo.Assesment.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Complevo.Assesment.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Complevo.Assesment.Api.dll"]