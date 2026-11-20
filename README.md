
# CRM System – Customer Relationship Management API

A RESTful Customer Relationship Management (CRM) API built with ASP.NET Core 8 to manage customers, sales orders, products, and staff access.

The system provides authentication and role-based authorization, allowing administrators and sales staff to manage business operations through a structured API.

## 🚀 Features

### 🔐 Authentication & Authorization
- User authentication using JWT Bearer tokens.
- ASP.NET Core Identity for user management.
- Role-based authorization.
- Admin and Sales roles.
- Secure password requirements.
- Retrieve the authenticated user's profile.
- JWT claims containing user identity, roles, and resource identifiers.

### 👥 Client Management
- Create new clients.
- Retrieve client records.
- Sort clients by name.
- Update client information.
- Delete client records.
- Associate clients with their creating users.

### 📦 Product Management
- Create products.
- Retrieve product listings.
- Update product information.
- Delete products.
- Manage product details:
  - Name
  - Description
  - Price
  - Stock
  - Category
- Associate products with users.

### 🧾 Order Management
- Create sales orders.
- Retrieve order listings.
- Sort orders by different fields.
- Update order information and status.
- Delete orders.
- Track order details:
  - Order Number
  - Product
  - Price
  - Order Date
  - Status
- Associate orders with users.

### 🏗️ Architecture & Design Patterns
- Generic Repository Pattern for reusable data access.
- Specification Pattern for reusable query filtering, sorting, and related-data inclusion.
- Entity Framework Core for database operations.
- AutoMapper for mapping between DTOs and entities.
- Separation of concerns across Core, Repository, and API projects.

## 🛠️ Technologies

| Technology | Purpose |
|---|---|
| C# | Backend development |
| ASP.NET Core 8 Web API | REST API development |
| Entity Framework Core 8 | ORM |
| SQL Server | Relational database |
| ASP.NET Core Identity | Authentication and user management |
| JWT Bearer Authentication | Token-based authentication |
| AutoMapper | Object mapping |
| Generic Repository Pattern | Data access abstraction |
| Specification Pattern | Query composition |
| Swagger / OpenAPI | API documentation |

## 🏛️ Solution Structure

```text
CRM-System/
│
├── ASP.Authentication/
│   ├── Controllers/
│   │   ├── AccountController.cs
│   │   ├── ClientController.cs
│   │   ├── OrdersController.cs
│   │   └── ProductController.cs
│   │
│   ├── DTOs/
│   ├── Helper/
│   │   └── MappingProfil.cs
│   ├── JwtHandler.cs
│   ├── Program.cs
│   └── appsettings.json
│
├── Core/
│   ├── Entities/
│   ├── Repository/
│   └── Specification/
│
├── Reposatory/
│   ├── Configuration/
│   ├── Data/
│   ├── Migrations/
│   └── Repository/
│
├── Sercise/
│
└── ASP.Authentication.sln
```

## 🔄 Application Workflow

1. Staff users authenticate using their email and password.
2. The API validates credentials using ASP.NET Core Identity.
3. A JWT access token is generated with the user's identity and roles.
4. The client sends the token with subsequent protected requests.
5. The API validates the token and checks the user's permissions.
6. Authorized users perform customer, product, and order operations.
7. Entity Framework Core persists changes in SQL Server.

## 🔑 User Roles

| Role | Access |
|---|---|
| Admin | Manage clients, products, and orders |
| Sales | Manage products and sales orders |

Access is controlled through role-based authorization attributes on API endpoints.

## 📌 API Endpoints

All endpoints use the `/api` prefix.

### Account

| Method | Endpoint | Description |
|---|---|---|
| POST | `/api/account/login` | Authenticate a user |
| POST | `/api/account/register-staff` | Register a staff account |
| GET | `/api/account/Profile` | Retrieve the authenticated user's profile |

### Clients

| Method | Endpoint | Description | Access |
|---|---|---|---|
| GET | `/api/client?sort=asc` | Retrieve clients with sorting | Admin |
| POST | `/api/client` | Create a client | Admin |
| PUT | `/api/client` | Update a client | Admin |
| DELETE | `/api/client` | Delete a client | Admin |

### Products

| Method | Endpoint | Description | Access |
|---|---|---|---|
| GET | `/api/product` | Retrieve products | Admin, Sales |
| POST | `/api/product` | Create a product | Admin, Sales |
| PUT | `/api/product` | Update a product | Admin, Sales |
| DELETE | `/api/product?id={id}` | Delete a product | Admin |

### Orders

| Method | Endpoint | Description | Access |
|---|---|---|---|
| GET | `/api/orders?sort=Date` | Retrieve orders with sorting | Admin, Sales |
| POST | `/api/orders` | Create an order | Admin, Sales |
| PUT | `/api/orders` | Update order information and status | Admin, Sales |
| DELETE | `/api/orders` | Delete an order | Admin |

**Authentication Header**

Protected endpoints require a valid JWT:

```http
Authorization: Bearer YOUR_ACCESS_TOKEN
```

## ⚙️ Getting Started

### Prerequisites

- .NET 8 SDK
- SQL Server
- Visual Studio 2022 or Visual Studio Code
- Git

### 1. Clone the Repository

```bash
git clone https://github.com/MohamedBadawy23/CRM-System.git
```

### 2. Navigate to the Project

```bash
cd CRM-System
```

### 3. Configure the Database

Update the connection strings in your local configuration or User Secrets.

The application expects:

- `ConnectionStrings:DefaultConnection` for Development.
- `ConnectionStrings:SqlServerConnection` for other environments.

Example:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=.;Database=CRMDb;Trusted_Connection=True;TrustServerCertificate=True;"
  }
}
```

### 4. Configure JWT

Configure the following JWT settings securely:

```json
{
  "JWT": {
    "Key": "YOUR_SECRET_KEY",
    "ValidIssuer": "YOUR_ISSUER",
    "ValidAudience": "YOUR_AUDIENCE",
    "DurationInDays": 1
  }
}
```

Never commit real credentials, database passwords, or JWT signing keys to source control.

### 5. Run the Application

```bash
dotnet restore
dotnet run --project ASP.Authentication/ASP.Authentication.csproj
```

The application applies pending Entity Framework Core migrations during startup.

### 6. Explore Swagger

Once the application is running, open the Swagger UI using the URL displayed in the application launch output.

Use Swagger to explore and test the available endpoints.

## 🗄️ Database Design

The application uses SQL Server and Entity Framework Core.

Main entities include:

- `User` – Staff user information and Identity data.
- `Clients_Management` – Customer information.
- `Orders_Management` – Sales order information.
- `Product` – Product catalog and stock information.

Entity configurations are applied through Entity Framework Core's configuration mechanism.

## 🔒 Security Considerations

The project implements:

- JWT token validation.
- ASP.NET Core Identity.
- Role-based endpoint authorization.
- Password complexity requirements.
- User identity and role claims.

For production readiness, review resource-level authorization, token and secret management, CORS restrictions, and input validation.

## 👨‍💻 Author

**Mohamed Badawy**

GitHub: [MohamedBadawy23](https://github.com/MohamedBadawy23)

Project Repository: [CRM-System](https://github.com/MohamedBadawy23/CRM-System)

---

⭐ If you find this project useful, feel free to explore the repository!
