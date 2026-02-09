# Endava Products API

A simple **.NET 9 Web API** for managing products and categories.  
This project demonstrates clean architecture principles, dependency injection, and basic validation.

---

## Overview

The API allows you to:

1. Fetch filtered products.
2. Update product details (name, price, category).

## API Endpoints

### 1. Fetch Products

**GET** `/Products`  

**Query Parameters:** `ProductsFilterRequest`
- `Name` (optional)
- `CategoryId` (optional)
- `MinPrice` (optional)
- `MaxPrice` (optional)

**Responses:**
- `200 OK` – Returns a list of filtered products.
- `400 Bad Request` – If `MinPrice > MaxPrice`.

---

### 2. Update Product

**PUT** `/Products/{id}`  

**Route Parameters:**
- `id` (GUID) – Product ID to update

**Body:** `UpdateProductRequest`
- `Name` (optional)
- `Price` (optional)
- `CategoryId` (optional)

**Responses:**
- `204 No Content` – Product updated successfully
- `400 Bad Request` – Validation errors or invalid category
- `404 Not Found` – Product with given ID not found

---

## Unit Tests

Run 
```bash
dotnet test
```

---

## Setup

1. Clone the repository.
2. Build and run the project:

```bash
dotnet build
dotnet run --project .\src\Endava.Products.Api\
```
3. Access Swagger UI at: `http://localhost:5005`