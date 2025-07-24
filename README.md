# Sudoku Solver API

This project is a web-based API providing a solution to solve Sudoku puzzles programmatically. It is implemented using **ASP.NET Core** and .NET 9.0, leveraging a modern and scalable architecture.

## Features

- **Solve Sudoku Puzzles**: Accept Sudoku puzzle grids through the API and return their solutions.
- **Customizable & Extensible**: Easily extend the services for other puzzle-related functionalities.
- **JSON-based API**: All requests and responses utilize JSON format, ensuring consistency and clarity.
- **CORS Support**: Configured to allow access from any origin, enabling smooth integration with front-end clients.
- **Environment-specific Configurations**: Development-specific configurations such as OpenAPI documentation and debugging tools.

## Setup Instructions

### Prerequisites

Ensure you have the following installed:

- **.NET 9.0 SDK**: [Download here](https://dotnet.microsoft.com/download)
- Any code editor such as **JetBrains Rider**, **Visual Studio**, or **VS Code**.
- A tool to test HTTP requests such as **Postman** or **curl**.

### Installation

1. Clone this repository:
   ```bash
   git clone <your-repository-url>
   cd <repository-folder>
   ```

2. Restore dependencies:
   ```bash
   dotnet restore
   ```

3. Build the project:
   ```bash
   dotnet build
   ```

4. Run the application:
   ```bash
   dotnet run
   ```

It will start the application, typically available at `http://localhost:5000` (for HTTP) and `https://localhost:5001` (for HTTPS).

## API Endpoints

### Available Endpoints

1. **Solve a Sudoku Puzzle**
    - **Method**: `POST`
    - **Endpoint**: `/api/sudoku/solve`
    - **Request Body**:
      ```json
      {
        "grid": [
           [5, 3, 0, 0, 7, 0, 0, 0, 0],
           [6, 0, 0, 1, 9, 5, 0, 0, 0],
           [0, 9, 8, 0, 0, 0, 0, 6, 0],
           [8, 0, 0, 0, 6, 0, 0, 0, 3],
           [4, 0, 0, 8, 0, 3, 0, 0, 1],
           [7, 0, 0, 0, 2, 0, 0, 0, 6],
           [0, 6, 0, 0, 0, 0, 2, 8, 0],
           [0, 0, 0, 4, 1, 9, 0, 0, 5],
           [0, 0, 0, 0, 8, 0, 0, 7, 9]
        ]
      }
      ```
    - **Response Body**:
      ```json
      {
        "solution": [
           [5, 3, 4, 6, 7, 8, 9, 1, 2],
           [6, 7, 2, 1, 9, 5, 3, 4, 8],
           ...
        ]
      }
      ```

2. **Health Check**
    - **Method**: `GET`
    - **Endpoint**: `/api/health`
    - **Response**:
      ```json
      {
        "status": "Healthy"
      }
      ```

## Configuration

### CORS Policy

The application is configured with a global **CORS policy** allowing all origins, headers, and methods: