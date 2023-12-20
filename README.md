# AcademicHub

The course of Software Engineering Practices Project

```bash
git clone https://github.com/AcademicHubOrg/AcademicHub.git
```

# How to start working with the AcademicHub services(Identity, CourseTemplate, CourseStream and others)?

## Infrastructure Setup

We use' Docker' to manage infrastructure to avoid complex infrastructure setups.
Each service has its own `docker-compose` file that should be used to run all required infra using a single command.

## Run Service's Infrastructure

Run `docker-compose` to setup all the required infra.

```bash
docker-compose up -d
```


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
