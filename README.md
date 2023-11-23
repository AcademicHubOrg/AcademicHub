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

# Academic Hub Microservices API Documentation

## Stream Microservice

#### `GET /healthz`
> **Health Check**  
> Checks if the Stream service is operational.  
> **Response:** Status code indicating service health.

#### `GET /courseStreams/list`
> **List Course Streams**  
> Retrieves a list of all available course streams.  
> **Response:** JSON array of course streams.

#### `POST /courseStreams/add`
> **Add Course Stream**  
> Adds a new course stream to the system.  
> **Response:** Status code and details of the added course stream.

## Template Microservice

#### `GET /healthz`
> **Health Check**  
> Checks if the Template service is operational.  
> **Response:** Status code indicating service health.

#### `GET /courseTemplates/list`
> **List Course Templates**  
> Retrieves a list of all course templates.  
> **Response:** JSON array of course templates.

#### `POST /courseTemplates/add`
> **Add Course Template**  
> Adds a new course template.  
> **Response:** Status code and details of the added course template.

#### `GET /courseTemplates/{id}`
> **Get Course Template by ID**  
> Retrieves details of a specific course template using its ID.  
> **Response:** JSON object containing course template details.

## Identity Microservice

#### `GET /healthz`
> **Health Check**  
> Checks if the Identity service is operational.  
> **Response:** Status code indicating service health.

#### `GET /users/list`
> **List Users**  
> Retrieves a list of all registered users.  
> **Response:** JSON array of user details.

#### `POST /users/add`
> **Add User**  
> Registers a new user in the system.  
> **Response:** Status code and details of the added user.

#### `POST /users/makeAdmin`
> **Make User Admin**  
> Elevates a user's role to admin.  
> **Response:** Status code and confirmation of role update.

#### `GET /login`
> **User Login**  
> Initiates the user login process.  
> **Response:** Redirect to login interface or user dashboard.

#### `GET /after-signin`
> **Post-Signin Redirect**  
> Handles redirection after a successful sign-in.  
> **Response:** Redirect to a specified post-login resource.

## Materials Microservice

#### `GET /healthz`
> **Health Check**  
> Checks if the Materials service is operational.  
> **Response:** Status code indicating service health.

#### `GET /materials/list`
> **List Materials**  
> Retrieves a list of all teaching materials.  
> **Response:** JSON array of materials.

#### `GET /materials/id`
> **Get Material by ID**  
> Retrieves a specific material by its material ID.  
> **Response:** JSON object with material details.

#### `GET /materials/course`
> **Get Materials by Course ID**  
> Retrieves materials related to a specific course ID.  
> **Response:** JSON array of materials for the specified course.

#### `POST /materials/add`
> **Add Material**  
> Adds a new teaching material.  
> **Response:** Status code and details of the added material.


# How to start working with the AcademicHub React app?

TBA

```bash
instructions should be here
```
