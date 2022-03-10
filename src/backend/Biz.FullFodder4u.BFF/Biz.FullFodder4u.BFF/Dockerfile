#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Biz.FullFodder4u.BFF/Biz.FullFodder4u.BFF/Biz.FullFodder4u.BFF.csproj", "Biz.FullFodder4u.BFF/Biz.FullFodder4u.BFF/"]
RUN dotnet restore "Biz.FullFodder4u.BFF/Biz.FullFodder4u.BFF/Biz.FullFodder4u.BFF.csproj"
COPY . .
WORKDIR "/src/Biz.FullFodder4u.BFF/Biz.FullFodder4u.BFF"
RUN dotnet build "Biz.FullFodder4u.BFF.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Biz.FullFodder4u.BFF.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Biz.FullFodder4u.BFF.dll"]