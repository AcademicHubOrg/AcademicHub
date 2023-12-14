FROM mcr.microsoft.com/dotnet/sdk:7.0-alpine AS build-env
WORKDIR /src

COPY ./Materials ./Materials
COPY ./AcademicHub.Common ./AcademicHub.Common
RUN cd Materials && dotnet restore
RUN cd Materials && dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:7.0-alpine
WORKDIR /app
COPY --from=build-env /src/Materials/out ./
ENTRYPOINT ["dotnet", "./Materials.dll"]