FROM mcr.microsoft.com/dotnet/sdk:7.0-alpine AS build-env
WORKDIR /src

COPY ./Identity ./Identity
COPY ./AcademicHub.Common ./AcademicHub.Common
RUN cd Identity && dotnet restore
RUN cd Identity && dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:7.0-alpine
WORKDIR /app
COPY --from=build-env /src/Identity/out ./
ENTRYPOINT ["dotnet", "./Identity.dll"]