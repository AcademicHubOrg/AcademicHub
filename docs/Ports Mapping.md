# Ports Mapping Documentation

## Academic Hub - Route Mapping

This document describes the ports usage for each microservice in the Academic Hub's Docker environment.

| Container Name            | External Port | Internal Port |
| ------------------------- | ------------- | ------------- |
| Academic Hub Frontend     | 3000          | 3000          |
| Academic Hub Course Template | 52998        | 80            |
| Academic Hub Materials    | 52996         | 80            |
| Academic Hub Identity     | 52999         | 80            |
| Academic Hub Course Stream | 52997        | 80            |
| Postgres Container        | -             | 5432          |
