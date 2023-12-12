FROM mcr.microsoft.com/dotnet/sdk:7.0-alpine AS build-env
WORKDIR src

COPY ./CourseStream ./CourseStream
COPY ./AcademicHub.Common ./AcademicHub.Common
RUN cd CourseStream && dotnet restore
RUN cd CourseStream && dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:7.0-alpine
COPY --from=build-env /src/CourseStream/out ./
ENTRYPOINT ["dotnet", "./CourseStream.dll"]