# Travel Booking Application

---
## Developer Information
### Team devDynamos

| **Name**               | **Email**                                  | **Facebook**                                                                                      | **LinkedIn**                                            | **Codeforces**                                               |
|------------------------|--------------------------------------------|---------------------------------------------------------------------------------------------------|---------------------------------------------------------|--------------------------------------------------------------|
| Rabiul Islam Bipul      | rabiulislambipul.per@gmail.com             | [Facebook Profile](https://www.facebook.com/rabiulbipul/)                                          | [LinkedIn](https://www.linkedin.com/in/)                | [Codeforces](https://codeforces.com/profile/rabiulbipul)      |
| MD. Jonayat Hossain     | jonayathossainooo@gmail.com                | [Facebook Profile](https://www.facebook.com/profile.php?id=100014249430261)                        | [LinkedIn](https://www.linkedin.com/in/jonayat-hossain-2152a721a) | [Codeforces](https://codeforces.com/profile/Jonayat_Hossain)   |

---
 
## Overview
This project simplifies the process of traveling by allowing users to book hotels at preferred locations through packages. It offers an efficient, user-friendly platform for managing travel bookings, ensuring a seamless experience for travelers.

## Demonstration Video
Watch the [Demonstration Video](https://www.youtube.com/watch?v=YBWl9S_u53Q) to see how the Travel Booking Application works.
 
## Main Features
- **Package Selection**: Customers can choose from pre-designed travel packages or create custom packages based on their preferences.
- **User Authentication & Roles**: Secure login system using ASP.NET Identity Framework ensures user data safety. Role-based access provides different levels of permissions for users and admins.
- **Booking Management**: Users can view, manage, and modify bookings. Admins manage packages, hotels, and bookings.
- **Responsive Design**: Optimized for a seamless experience across different devices using Bootstrap.
 
## Technologies Used
- **.NET Framework**: Core technology for building the web application and handling backend logic.
- **ASP.NET Identity Framework**: Provides user authentication and authorization functionalities.
- **Bootstrap**: Ensures a responsive and mobile-friendly front-end design.
- **Postman**: Used for testing API endpoints and ensuring that communication between the client and server is functioning correctly.
 
## Setup Instructions
 
### Prerequisites
Ensure the following are installed:
- Visual Studio (latest version recommended with .NET Framework support)
- .NET Framework (version compatible with your project)
- SQL Server (for local database management)
- Git (for cloning the repository)
- Postman (optional, for API testing)
 
### How to Clone the Project
1. Open a terminal or Git Bash.
2. Navigate to the directory where you want to clone the project.
3. Use the following command to clone the repository:
    ```bash
    git clone https://github.com/your-repository-link.git
    ```
4. Navigate to the project folder:
    ```bash
    cd project-folder-name
    ```
 
### Build the Project
1. Open the project in Visual Studio:
    - Click **File > Open > Project/Solution** and select the `.sln` file from the cloned project folder.
2. Restore the NuGet packages:
    - Go to **Tools > NuGet Package Manager > Manage NuGet Packages for Solution** and click **Restore** to install all dependencies.
3. Rebuild the solution:
    - Click **Build > Rebuild Solution** to compile all files and ensure no build errors.
 
### Set Up the Database (Code-First Approach)
1. **Configure the Database Connection**:
    - Open `appsettings.json` (or `Web.config`) and ensure the connection string points to your local SQL Server. Example:
        ```json
        "ConnectionStrings": {
            "DefaultConnection": "Server=your-server-name;Database=your-database-name;Trusted_Connection=True;"
        }
        ```
2. **Apply Migrations**:
    - If you haven’t created any migrations yet, run the following command in the Package Manager Console:
        ```bash
        Add-Migration InitialCreate
        ```
    - To apply the migrations and create the database, run:
        ```bash
        Update-Database
        ```
 
### Run the Project Locally
1. In Visual Studio, press **F5** or click **Start Debugging** to run the project. This will launch the application in your browser.
2. Ensure that the database connection works and the website loads correctly, allowing users to interact with the hotel booking system.
 
### Testing API Endpoints (Optional)
Use Postman to send requests to your local server and test API endpoints. This ensures that routes and backend logic are functioning as expected.
 
## Project Structure
The project follows the **MVC (Model-View-Controller)** architecture along with a layered structure for separating concerns and improving maintainability.
 
### 1. MVC Architecture
- **Model**: Represents data and business logic, including entity classes such as `Hotel`, `Package`, and `Location`.
- **View**: Responsible for displaying the UI. Razor Views (`.cshtml`) are used for rendering dynamic content.
- **Controller**: Handles user inputs, interacts with the Model, and renders the appropriate View, e.g., `AccountController`, `PackageController`, and `HotelController`.
 
### 2. Layered Architecture
- **Presentation Layer (PL)**: Contains all views and client-side logic (HTML, CSS, Bootstrap).
- **Data Access Layer (DAL)**: Manages database interactions using Entity Framework.
- **Entities/Models**: Represents the database schema with classes such as `Hotel`, `Booking`, `Package`.
 
### 3. Additional Components
- **ViewModels**: Data transfer objects used to pass data between the view and controller.
- **DTOs (Data Transfer Objects)**: Used for communication between layers or systems, especially in external system interactions.
- **Configuration Files**: Stores settings such as connection strings and authentication.
 
## API Documentation
 
### Base URL
`https://yourdomain.com/api`
 
### Endpoints
1. **Get All Packages**
    - `GET /api/packages`
    - Retrieves a list of available packages.
 
2. **Get Package by ID**
    - `GET /api/packages/{id}`
    - Retrieves details of a specific package by its ID.
 
3. **Get Top Packages**
    - `GET /api/top-packages/{count}`
    - Retrieves the top `{count}` packages.
 
4. **Add a Package**
    - `POST /api/addpackages`
    - Adds a new package to the system.
 
5. **Update a Package**
    - `PUT /api/updatepackages/{id}`
    - Updates details of an existing package.
 
6. **Delete a Package**
    - `DELETE /api/deletepackages/{id}`
    - Deletes a specific package.
 
## Database Schema
Designed using the **Code-First Approach** in Entity Framework.
 
### Key Tables
- **Hotels**: Stores hotel details (`HotelId`, `HotelName`, etc.).
- **Locations**: Stores location details (`LocationId`, `LocationName`, etc.).
- **Profiles (Users)**: Stores user details (`ProfileId`, `Name`, etc.).
- **Packages**: Stores package details (`PackageId`, `PackageName`, etc.).
- **Invoices**: Stores booking and payment details (`InvoiceId`, `ProfileId`, etc.).
 
---



 
