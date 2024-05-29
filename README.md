
# C# Server Application with MySQL Integration

## Project Overview

This project is a server application developed using the C# .NET framework. It integrates with a MySQL database and includes the following components:

- **Models**: Represent the data structure of the application.
- **Controllers**: Handle CRUD (Create, Read, Update, Delete) operations on the database.
- **Batch Services**: Perform scheduled batch processing tasks.

## Features

- **MySQL Database Integration**: Set up a MySQL database with a `Users` table.
- **CRUD Operations**: Implement methods to add, retrieve, update, and delete user data.
- **Batch Processing**: Schedule tasks such as sending emails to users at specified intervals.
- **HTTP Server**: A simple HTTP server to handle incoming requests and map them to controller methods.

## Getting Started

1. **Clone the Repository**:
   ```sh
   git clone https://github.com/Miri-Shtul/MsApps.git
   cd MsApps
   ```

2. **Set Up MySQL Database**:
   - Install MySQL Server.
   - Create a database named `MyDatabase`.
   - Create a `Users` table with `ID`, `Name`, `Email`, and `Password` columns.

3. **Run the Application**:
   - Open the project in Visual Studio.
   - Ensure MySQL Connector/NET is installed.
   - Build and run the application.

## Testing

- **Server Testing**: Use a console application to test CRUD operations.
- **Batch Service Testing**: Manually run and observe the batch processing task.

## Conclusion

This project demonstrates the integration of a C# server application with a MySQL database, showcasing the implementation of essential server-side functionalities.
