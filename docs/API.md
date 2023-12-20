# Academic Hub Microservices API Documentation

## Stream Microservice
---

### Endpoints

- **`GET /healthz`**
    - **Description:** Health Check - Checks if the Stream service is operational.
    - **Response:** Status code indicating service health.

- **`GET /courseStreams/list`**
    - **Description:** List Course Streams - Retrieves a list of all available course streams.
    - **Response:** JSON array of course streams.

- **`POST /courseStreams/add`**
    - **Description:** Add Course Stream - Adds a new course stream to the system.
    - **Response:** Status code and details of the added course stream.

## Template Microservice
---

### Endpoints

- **`GET /healthz`**
    - **Description:** Health Check - Checks if the Template service is operational.
    - **Response:** Status code indicating service health.

- **`GET /courseTemplates/list`**
    - **Description:** List Course Templates - Retrieves a list of all course templates.
    - **Response:** JSON array of course templates.

- **`POST /courseTemplates/add`**
    - **Description:** Add Course Template - Adds a new course template.
    - **Response:** Status code and details of the added course template.

- **`GET /courseTemplates/{id}`**
    - **Description:** Get Course Template by ID - Retrieves details of a specific course template using its ID.
    - **Response:** JSON object containing course template details.

## Identity Microservice
---

### Endpoints

- **`GET /healthz`**
    - **Description:** Health Check - Checks if the Identity service is operational.
    - **Response:** Status code indicating service health.

- **`GET /users/list`**
    - **Description:** List Users - Retrieves a list of all registered users.
    - **Response:** JSON array of user details.

- **`POST /users/add`**
    - **Description:** Add User - Registers a new user in the system.
    - **Response:** Status code and details of the added user.

- **`POST /users/makeAdmin`**
    - **Description:** Make User Admin - Elevates a user's role to admin.
    - **Response:** Status code and confirmation of role update.

- **`GET /login`**
    - **Description:** User Login - Initiates the user login process.
    - **Response:** Redirect to login interface or user dashboard.

- **`GET /after-signin`**
    - **Description:** Post-Signin Redirect - Handles redirection after a successful sign-in.
    - **Response:** Redirect to a specified post-login resource.

## Materials Microservice
---

### Endpoints

- **`GET /healthz`**
    - **Description:** Health Check - Checks if the Materials service is operational.
    - **Response:** Status code indicating service health.

- **`GET /materials/list`**
    - **Description:** List Materials - Retrieves a list of all teaching materials.
    - **Response:** JSON array of materials.

- **`GET /materials/id`**
    - **Description:** Get Material by ID - Retrieves a specific material by its material ID.
    - **Response:** JSON object with material details.

- **`GET /materials/course`**
    - **Description:** Get Materials by Course ID - Retrieves materials related to a specific course ID.
    - **Response:** JSON array of materials for the specified course.

- **`POST /materials/add`**
    - **Description:** Add Material - Adds a new teaching material.
    - **Response:** Status code and details of the added material.
