# Human Capital Management Application

## 📄 Overview

This is a **Human Capital Management System** built with **ASP.NET Core MVC**, **Entity Framework Core**, and **ASP.NET Core Identity**. The application provides full CRUD functionality for managing employees with **role-based access control**.

## 🎯 Project Purpose

This project simulates a Human Capital Management (HCM) system designed for small and medium-sized companies. It enables HR departments to efficiently manage employee records, departments, and salaries while enforcing role-based access and modern software practices.

### ✅ Features

- Role-based access for **HR Admin**, **Manager**, and **Employee**
- Authentication and authorization with **ASP.NET Core Identity**
- Employee CRUD operations (Create, Read, Update, Delete)
- Department, Designation, and Employee Type management
- **Pagination**, **search filters**, and **sorting**
- Account seeding based on existing employee data
- Responsive UI with **Bootstrap 5** and **Bootstrap Icons**
- Clear separation of concerns with MVC architecture

## 🧩 Extended Features

- IBAN field with encrypted storage and format validation
- Country field integrated with **API Ninjas Working Days** API to calculate working days per employee by country and month
- Dynamic working days display in the employee profile

---

## 🛠️ Technologies Used

- ASP.NET Core MVC (.NET 8)
- Entity Framework Core with Code-First Migrations
- ASP.NET Core Identity
- SQL Server
- Bootstrap 5 + Bootstrap Icons
- jQuery (for dynamic designations)
- LINQ & Dependency Injection
- **API Ninjas Working Days API** (for calculating monthly working days by country)

---

## 🏁 Getting Started

### 1. Clone the repository

```bash
git clone https://github.com/ivoiliev0394/HUMAN-CAPITAL-MANAGEMENT-APPLICATION_UKG.git
cd HUMAN-CAPITAL-MANAGEMENT-APPLICATION_UKG
```

### 2. Configure Database

- In `appsettings.json`, update your connection string:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=YOUR_SERVER;Database=HumanCapitalManagementAppDatabase;Trusted_Connection=True;TrustServerCertificate=True;"
}
```
### 2. Configure API Ninjas Working Days – API Key
🔐 API Ninjas Working Days – API Key
This project integrates with the [API Ninjas Working Days API](https://api-ninjas.com/api/workingdays) to calculate the number of working days per country and month.

#### 🔑 API Key Setup

To use this feature, you'll need an **API key** from API Ninjas.  
Set your API key either:

- As an environment variable:

```bash
API_NINJAS_KEY=your_api_key_here
```

- Or in `appsettings.json`:

```json
{
  "ApiNinjas": {
    "Key": "your_api_key_here"
  }
}
```

#### ⚠️ API Limitations

- **Free accounts** only allow **monthly queries**.
- To support **year-based queries** (e.g., `&year=2025`) or increase request limits, you must have a **Premium API key**.
- For more accuracy (e.g., public holidays), local filtering may still be required.

### 4. Apply Database Migrations

```bash
dotnet ef database update
```

### 5. Run the Application

```bash
dotnet run
```

Navigate to [https://localhost:5001](https://localhost:5001) in your browser.

---

## 👥 Default Roles and Access Rights

| Role         | Access Level                                                   |
| ------------ | -------------------------------------------------------------- |
| **HR Admin** | Full access to all employees (CRUD)                            |
| **Manager**  | Access to employees in their department (List, Update, Delete) |
| **Employee** | Access to their own profile (Details view only)                |

---

## 🎁 Seeded Accounts

The application auto-generates Identity accounts based on seeded `Employee` data.

| Employee ID | Role     |
| ----------- | -------- |
| 1-2         | HR Admin |
| 3-6         | Manager  |
| 7+          | Employee |

Default passwords are seeded in the database along with Employee data.

---

## 📂 Folder Structure

```
├── Controllers/
├── Data/
├── Models/
├── Migrations/
├── Services/
├── Utilities/
├── ViewModels/
├── Views/
└── wwwroot/
```

---

## 📊 Database Schema

![Database Schema](./Schema.png)

---

## 📌 Author

Made by [Ivailo Iliev](https://github.com/ivoiliev0394) | Contact: [ivailo.iliev9999@gmail.com](mailto\:ivailo.iliev9999@gmail.com)

