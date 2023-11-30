FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Identity/Identity/Identity.csproj", "Identity/"]
COPY ["Identity/Identity.Data/Identity.Data.csproj", "Identity.Data/"]
COPY ["Identity/Identity.Core/Identity.Core.csproj", "Identity.Core/"]
COPY ["AcademicHub.Common/CustomExceptions/CustomExceptions.csproj", "AcademicHub.Common/CustomExceptions/"]


# Restore NuGet packages
RUN dotnet restore "Identity/Identity.csproj"

# Copy source files
COPY ["Identity/Identity/*.cs", "Identity/Identity/"]
COPY ["Identity/Identity.Data/*.cs", "Identity/Identity.Data/"]
COPY ["Identity/Identity.Core/*.cs", "Identity/Identity.Core/"]
COPY ["AcademicHub.Common/CustomExceptions/*.cs", "AcademicHub.Common/CustomExceptions/"]

WORKDIR "/src/Identity"
RUN dotnet build "Identity.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Identity.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Identity.dll"]
