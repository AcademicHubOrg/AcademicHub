# Ports Mapping Documentation

## Academic Hub - Route Mapping

This document describes the ports usage for each microservice in the Academic Hub's Docker environment.

### Microservices and Ports

Below is the output from the `docker ps` command, showing the active containers and their respective port mappings.

#### 1. Academic Hub Frontend
- **Container ID:** `c439a1309c2c`
- **Image:** `***/web:latest`
- **Ports:** `3000:3000` (both TCP)
- **Name:** `academic_hub_frontend`

#### 2. Academic Hub Course Template
- **Container ID:** `35eb07626f9b`
- **Image:** `***/course-template:latest`
- **Ports:** `52998:80` (both TCP)
- **Name:** `academic_hub_coursetemplate`

#### 3. Academic Hub Materials
- **Container ID:** `b33aa668c052`
- **Image:** `***/materials:latest`
- **Ports:** `52996:80` (both TCP)
- **Name:** `academic_hub_materials`

#### 4. Academic Hub Identity
- **Container ID:** `7023749693cb`
- **Image:** `***/identity:latest`
- **Ports:** `52999:80` (both TCP)
- **Name:** `academic_hub_identity`

#### 5. Academic Hub Course Stream
- **Container ID:** `702fadb9d04d`
- **Image:** `***/course-stream:latest`
- **Ports:** `52997:80` (both TCP)
- **Name:** `academic_hub_coursestream`

#### 6. PostgreSQL Container
- **Container ID:** `624c710d8f9a`
- **Image:** `bitnami/postgresql:latest`
- **Ports:** `5432` (TCP)
- **Name:** `postgres-container`
