# AcademicHub

The course of Software Engineering Practices Project

```bash
git clone https://github.com/AcademicHubOrg/AcademicHub.git
```

# How to start working with the AcademicHub services(Identity, CourseTemplate, CourseStream and others)?

## Infrastructure Setup

We use' Docker' to manage infrastructure to avoid complex infrastructure setups.
Each service has its own `docker-compose` file that should be used to run all required infra using a single command.

> [!NOTE]
> Make sure that you dont have more than 1 container on the same port.
> Default port for services is `5440`

## Run Service's Infrastructure

Run `docker-compose` to setup all the required infra.

```bash
docker-compose up -d
```

## Database

### Prerequisites

- **`dotnet ef` tools:** If you don't have it, please
  visit [installation guide](https://learn.microsoft.com/en-us/ef/core/cli/dotnet).

### Migrations

To set the database properly after docker-compose run is finished and all containers are green - apply migrations to
update database schema to the appropriate state.

1. Navigate to the Data folder:

  ```bash
  cd .\SolutionName.Data\
  ```

2. Apply migrations
  ```bash
  dotnet ef database update
  ```

### Local Environment Credentials
> [!NOTE]
> Default credentials for local environment forms following way:
> **DATABASE:** '${SolutionName}'
> **USER:** '${SolutionName}user'
> **PASSWORD:** '${SolutionName}user'

### Setup Validation

Here what should you see Docker has started up successfully and database was updated.

![image](https://github.com/podkolzzzin/AcademicHub/assets/94047397/c0469c30-beed-447c-ba0e-d1bed468cf78)

After that you are ready to work with the service

> TBA: Add detailed guideline how to validate configured setup

# How to start working with the AcademicHub React app?

TBA

```bash
instructions should be here
```
