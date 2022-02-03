#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR /src
COPY ["Idco.Balances.Api/Idco.Balances.Api.csproj", "Idco.Balances.Api/"]
RUN dotnet restore "Idco.Balances.Api/Idco.Balances.Api.csproj"
COPY . .
WORKDIR "/src/Idco.Balances.Api"
RUN dotnet build "Idco.Balances.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Idco.Balances.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Idco.Balances.Api.dll"]