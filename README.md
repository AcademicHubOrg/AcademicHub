# AcademicHub
The course of Software Engineering Practices Project

```bash
git clone https://github.com/podkolzzzin/AcademicHub.git
```

# How to start working with the AcademicHub services(Identity, CourseTemplate, CourseStream and others)?

## Docker setup
Make sure that u dont have more than 1 container on the same port. Default port for sevrices is 3440


```bash
docker-compose up -d
```

## Update db
Navigate to the Data folder:

```bash
cd .\SolutionName.Data\
```

```bash
dotnet ef database update
```

Here what should you see Docker has started up successfully and database was updated.

![image](https://github.com/podkolzzzin/AcademicHub/assets/94047397/c0469c30-beed-447c-ba0e-d1bed468cf78)

Create DB using postgresql, specifying port and using username: solutionname + user

After that you are ready to work with the service

# How to start working with the AcademicHub React app?
```bash
instructions should be here
```
