FROM mcr.microsoft.com/dotnet/sdk:7.0-alpine AS build-env
WORKDIR /src

COPY ./CourseTemplate ./CourseTemplate
COPY ./AcademicHub.Common ./AcademicHub.Common
RUN cd CourseTemplate && dotnet restore
RUN cd CourseTemplate && dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:7.0-alpine
WORKDIR /app
COPY --from=build-env /src/CourseTemplate/out ./
ENTRYPOINT ["dotnet", "./CourseTemplate.dll"]