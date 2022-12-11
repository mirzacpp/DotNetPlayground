#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

#Depending on the operating system of the host machines(s) that will build or run the containers, the image specified in the FROM statement may need to be changed.
#For more information, please see https://aka.ms/containercompat

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Simplicity.Templates.Api/Simplicity.Templates.Api.csproj", "Simplicity.Templates.Api/"]
COPY ["Simplicity.Swashbuckle.AspNetCore/Simplicity.Swashbuckle.AspNetCore.csproj", "Simplicity.Swashbuckle.AspNetCore/"]
COPY ["Simplicity.AspNetCore.Mvc/Simplicity.AspNetCore.Mvc.csproj", "Simplicity.AspNetCore.Mvc/"]
COPY ["Simplicity.Commons/Simplicity.Commons.csproj", "Simplicity.Commons/"]
RUN dotnet restore "Simplicity.Templates.Api/Simplicity.Templates.Api.csproj"
COPY . .
WORKDIR "/src/Simplicity.Templates.Api"
RUN dotnet build "Simplicity.Templates.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Simplicity.Templates.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Simplicity.Templates.Api.dll"]