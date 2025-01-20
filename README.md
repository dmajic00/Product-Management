# Product-Management

A Vue.js-based product management app

A web application for managing product inventory. Easily create, update, delete, and view product details with features like sorting and searching.

---

## Table of Contents

1. [Features](#features)
2. [Technologies Used](#technologies-used)
3. [Installation](#installation)
4. [Database Setup](#database-setup)
5. [Usage](#usage)
6. [API Endpoints](#api-endpoints)
7. [Known Issues](#known-issues)

---

## Features

- Add, edit, delete, and view product details.
- Pagination for large product lists.
- Search products by name or price (case-insensitive).
- Sort products by name or price.
- Toast notifications for success and error messages.
- API integration with proper error handling.

---

## Technologies Used

- **Frontend**: Vue.js, Vue Router, Vue Toastification
- **Backend**: ASP.NET Core, Dapper, SQL Server
- **Styling**: CSS
- **API Requests**: Axios

---

## Installation

### Prerequisites

1. Node.js and npm installed.
2. .NET SDK installed.
3. SQL Server instance.

### Backend Setup

1. Clone the repository:
   ```bash
   git clone https://github.com/dmajic00/Product-Management.git
   cd Product-Management-app/ProductAPI
   ```
2. Set up the database:
   - Create a database in SQL Server.
   - Update the connection string in `appsettings.json` to match your database configuration.
3. Make sure your applicationUrl in `launchSettings.json` matches the frontend url.

```json
 "applicationUrl": "https://localhost:7020;http://localhost:5074"
```

4. Run the backend:
   ```bash
   dotnet run
   ```

### Frontend Setup

1. Navigate to the frontend directory:
   ```bash
   cd ../PRODUCT_FRONTEND
   ```
2. Install dependencies:
   ```bash
   npm install
   ```
3. Start the development server:
   ```bash
   npm run dev
   ```

The app will be available at `http://localhost:5074`.

---

## Database Setup

1. **Create the Database**:

   - Open SQL Server Management Studio (SSMS).
   - Create a new database for the application, `ProductDB`.

2. **Run the Schema Script**:

   - Use the following SQL script to create the `Products` table:
     ```sql
     CREATE TABLE Products (
         ProductId INT PRIMARY KEY IDENTITY(1,1),
         Name NVARCHAR(255) NOT NULL,
         Price DECIMAL(18,2) NOT NULL,
         Description NVARCHAR(MAX),
         Quantity INT NOT NULL,
         CreatedAt DATETIME NOT NULL DEFAULT GETDATE(),
         UpdatedAt DATETIME NULL
     );
     ```

3. **Add Indexes**:

   - Create indexes for improved query performance:
     ```sql
     CREATE NONCLUSTERED INDEX IX_Products_Name ON Products (Name);
     CREATE NONCLUSTERED INDEX IX_Products_Price ON Products (Price);
     ```

4. **Update Connection String**:

   - Open `appsettings.json` in the backend project.
   - Update the `ConnectionStrings` section to match your database settings:
     ```json
     "ConnectionStrings": {
         "DefaultConnection": "Server=YOUR_SERVER;Database=ProductDB;User Id=YOUR_USER;Password=YOUR_PASSWORD;"
     }
     ```

5. **Test the Database Connection**:
   - Run the backend to ensure it connects to the database and queries the indexes efficiently:
     ```bash
     dotnet run
     ```

---

## Usage

1. Navigate to the app in your browser (`http://localhost:5074`).
2. Use the product list to view, search, and sort products.
3. Click "Add Product" to create a new product.
4. Use the "Edit" and "Delete" buttons to manage existing products.
5. Toast notifications will provide feedback on your actions.

---

## API Endpoints

### Product Endpoints

- `GET /api/products` - Fetch all products (supports pagination, sorting, and search).
- `GET /api/products/{id}` - Fetch a single product by ID.
- `POST /api/products` - Create a new product.
- `PUT /api/products/{id}` - Update an existing product.
- `DELETE /api/products/{id}` - Delete a product.

### Error Responses

- **404 Not Found**: Resource not found.
- **409 Conflict**: Duplicate product name.
- **500 Internal Server Error**: Server-side error.

---

## Known Issues

- Case-insensitive search relies on frontend normalization; consider backend improvements.
- Error handling for rare network issues may need additional testing.
