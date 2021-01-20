# Log Viewer

## About the Project

The Log Viewer is a demo web application to save the Nginx and Apache Web Server access logs in the database through a simple and friendly interface.

Main features:
- Create, edit, query, and delete logs
- Import the Nginx and Apache access log files for batch create


## Build With

|   **Program**     |   **Version**     |
|-------------------|-------------------|
| .NET Core         |   3.1             |
| PostgreSQL        |   13.1            |
| Docker            |   19.03           |
| Docker Compose    |   1.27.4          |
| Angular CLI       |   11.0.7          |
| Nginx (Docker)    |   1.19.6-alpine   |
| NodeJS            |   14.15           |

## Prerequisites

This application requires .NET Core 3.1, Docker 19+, NodeJS and Angular CLI installed on your OS.

## Running the application

### Running with Docker

To run the Log Viewer appllcation with Docker, you need only this programs installed at your OS:

|   **Program**     |   **Version**     |
|-------------------|-------------------|
| Docker            |   19.03           |
| Docker Compose    |   1.27.4          |

If the requirements are satisfied, please execute the following command do up the stack:

```bash
docker-compose build
docker-compose up -d
```

The docker-compose create these instances:

| **Instance**                  | **Description**                                                                               |
|-------------------------------|-----------------------------------------------------------------------------------------------|
| **logviewer_logviewer-api**   | LogViewer API - Available at [http://localhost:5000/api](http://localhost:5000/api)<br>[Swagger Docs](http://localhost:5000/swagger)   |
| **logviewer_web**             | LogViewer Front - Available at [http://localhost:4500](http://localhost:4500)                 |
| **logviewer_db**              | LogViewer Database - PostgreSQL using the 5432/tcp port                                       |

### Running from source

#### Backend

Run the project in the Visual Studio 2019+ for debugging, execute the unit tests and understand the application logic applyed for this PoC.

The backend source code has the following layers:

|               **Layer**           |               **Main objective** |
|-----------------------------------|--------------------|
| **LogViewer**                     | The principal project, here are located the controllers and middlewares of the application. |
| **LogViewer.Business**            | This project provides the application's logic, validations and communication between the Controllers, and the repository (database) layers. |
| **LogViewer.Repository**          | This project provides an abstraction of the database connection and CRUD operations. |
| **LogViewer.Models**              | This class library contains the POCO objects returned by business and API layers to clients. |
| **LogViewer.Infrastructure**      | This class library contains mainly the helpers, constants, and common classes used by entire projects. |
| **LogViewerTests**                | Contains all unit tests   |


Before start the application for the first time, check the database connection string located at ***appsettings.development.json*** file, and change it if necessary.

After start the debugging, a new browser window will be open with the Swagger documentation of the API. You can test all endpoints.

#### Frontend

The frontend application is located at **logviewer-front** directory and can be opened with Visual Studio Code or any text editor of your preference.

The is application is a single Angular project with consumes the backend API.

Install the dependencies with the command:

```bash
npm install
```

To execute the unit tests, execute the next command:
```bash
ng test
```

To run this project, execute the following command:

```bash
npm start
```
After the compile step, the frontend is available at [http://localhost:4200](http://localhost:4200).
