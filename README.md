# Sudoku Solver API

This project features an API designed to solve **Sudoku puzzles** programmatically. Built using **ASP.NET Core** on *
*.NET 9.0**, it leverages a modular and scalable architecture suitable for modern web-based solutions.

## Features

- **Solve Sudoku Puzzles**: Submit a Sudoku grid to get the computed solution.
- **Generate New Puzzles**: Generate Sudoku puzzles of varying difficulty (Easy, Medium, Hard, Extreme).
- **Validation Logic**: Validate completed grids for correctness.
- **JSON-based Communication**: Clear and simple request/response format using JSON.
- **CORS Support**: Enables integration with external frontend clients.
- **Extensible Design**: Underlying services can be extended for additional features, including other puzzle types.

## Technologies Used

- **Framework**: ASP.NET Core on .NET 9.0
- **Language**: C# 13.0
- **Architecture**: Web API pattern with dependency injection (DI)
- Configurations include development-specific features such as **OpenAPI (Swagger)** for API documentation.

## Getting Started

### Prerequisites

Ensure the following tools are installed:

- **.NET SDK 9.0**: [Download here](https://dotnet.microsoft.com/download)
- A code editor like **JetBrains Rider**, **Visual Studio**, or **VS Code**.
- An API testing tool such as **Postman** or **curl**.

### Installation Steps

1. Clone the Repository:
   ```bash
   git clone [https://github.com/letStayFoolish/sudoku-solver-api.git](https://github.com/letStayFoolish/sudoku-solver-api.git)
   
   cd sudoku-solver-api
   ```

2. Restore Dependencies:
   ```bash
   dotnet restore
   ```

3. Build the Application:
   ```bash
   dotnet build
   ```

4. Run the Application:
   ```bash
   dotnet run
   ```

The server will now be running locally at:

- **HTTP**: `http://localhost:5154`
- **HTTPS**: `https://localhost:5155`

## API Endpoints

### 1. **Generate Sudoku Puzzle**

- **Method**: `GET`
- **URL**: `/api/sudoku/generate?difficulty={level}`
    - `difficulty`: Options are `Easy`, `Medium`, or `Hard` (default: `Hard`).

- **Response**:
  ```json
   [
      [5, 3, 0, 0, 7, 0, 0, 0, 0],
      [6, 0, 0, 1, 9, 5, 0, 0, 0],
      ...
    ]
  ```

### 2. **Solve a Sudoku Puzzle**

- **Method**: `POST`
- **URL**: `/api/sudoku/solved`

- **Request Body**:
  ```json
  [
      [5, 3, 0, 0, 7, 0, 0, 0, 0],
      [6, 0, 0, 1, 9, 5, 0, 0, 0],
      ...
  ]
  ```

- **Response**:

```json
[
  [
    5,
    3,
    4,
    6,
    7,
    8,
    9,
    1,
    2
  ],
  [
    6,
    7,
    2,
    1,
    9,
    5,
    3,
    4,
    8
  ]
]
```

### 3. **Health Check**

- **Method**: `GET`
- **URL**: `/api/health`

- **Response**:
  ```json
  {
    "status": "Healthy"
  }
  ```

[//]: # (## Project Structure)


## Extending the Application

Future enhancements could include:
- A front-end client for interactive Sudoku solving.
- Support for additional puzzle formats (e.g., custom grids).
- Session management for storing puzzles and solutions.

## License

This project is licensed under the MIT License. See `LICENSE` file for details.

Happy coding! 🧩
