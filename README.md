# AcademicHub

The course of Software Engineering Practices Project

```bash
git clone https://github.com/AcademicHubOrg/AcademicHub.git
```

## How to start working with the AcademicHub services(Identity, CourseTemplate, CourseStream and others)?

## Infrastructure Setup

We use' Docker' to manage infrastructure to avoid complex infrastructure setups.
Each service has its own `docker-compose` file that should be used to run all required infra using a single command.

## Run Service's Infrastructure

Run `docker-compose` to setup all the required infra.

```bash
docker-compose up -d
```

## Check if the Infrastructure set up successfully

Run `docker-ps` to see all the running containers.

```bash
docker-ps
```

Here is the output that you should see if you done everything correctly:
```bash
CONTAINER ID   IMAGE                                COMMAND                  CREATED         STATUS                  PORTS                                       NAMES
c439a1309c2c   ***/web:latest               "docker-entrypoint.s…"   3 seconds ago   Up Less than a second   0.0.0.0:3000->3000/tcp, :::3000->3000/tcp   academic_hub_frontend
35eb07626f9b   ***/course-template:latest   "dotnet ./CourseTemp…"   3 seconds ago   Up Less than a second   0.0.0.0:52998->80/tcp, :::52998->80/tcp     academic_hub_coursetemplate
b33aa668c052   ***/materials:latest         "dotnet ./Materials.…"   3 seconds ago   Up Less than a second   0.0.0.0:52996->80/tcp, :::52996->80/tcp     academic_hub_materials
7023749693cb   ***/identity:latest          "dotnet ./Identity.d…"   4 seconds ago   Up Less than a second   0.0.0.0:52999->80/tcp, :::52999->80/tcp     academic_hub_identity
702fadb9d04d   ***/course-stream:latest     "dotnet CourseStream…"   4 seconds ago   Up Less than a second   0.0.0.0:52997->80/tcp, :::52997->80/tcp     academic_hub_coursestream
624c710d8f9a   bitnami/postgresql:latest    "/opt/bitnami/script…"   2 days ago      Up 2 days               5432/tcp                                    postgres-container
```

## Additional Info

For example, to run only the Identity service:

```bash
docker-compose up identity
```

This command will build the image for the identity service and start it up in the container.

> [!NOTE]
> Always check that the postgres-container is up and running before starting any single service. 
> Failing to do so will result in error on starting your service.