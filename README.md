# Banking Operations System

This is an ASP.NET 8 web api designed to manage CRUD operations for Partners, Merchants and Transactions. The application provides features such as querying data, exporting data to CSV files and importing Transactions from xml files.

## Table of Contents

- [Prerequisites](#prerequisites)
- [Installation](#installation)
- [Application Tech Stack](#application-tech-stack)
- [API Documentation](#api-documentation)

## Prerequisites

Before you begin, ensure you have the following installed:

- [.NET SDK 8](https://dotnet.microsoft.com/download)
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads)
- [Git](https://git-scm.com/downloads)
- IDE of your choise ([Visual Studio](https://visualstudio.microsoft.com), [Rider](https://www.jetbrains.com/rider/download/#section=windows), [VS Code](https://code.visualstudio.com))

## Application Tech Stack
- .NET 8, ASP.NET Core 8
- SQL Server (database is created locally)
- Entity Framework Core 9
- Fluent Validation

## Installation

Follow these steps to set up the project locally:

1. Clone the repository:
   ```bash 
   git clone https://github.com/ShadyObeyd/BankingSystemOperations.git

2. Navigate to the project directory:
   ```bash
   cd BankingSystemOperations

3. Restore dependencies:
   ```bash
   dotnet restore

4. Build the application:
   ```bash
   dotnet build

5. Navigate to Data folder:
   ```bash
   cd src/BankingSystemOperations.Data

6. Apply migrations:
   ```bash
   dotnet ef database update

## API Documentation
This section describes the available endpoints and their functionality.

### Partners

#### 1. `GET /Partners`
- **Description**: Fetches partners as a paginated result, using `pageNumber` and `pageSize` passed as query parameters.
- **Response**:
  ```json
  {
    "items": [
      {
        "id": "0baaad0b-a4a7-425d-96e6-61f3be7641b8",
        "name": "Partner 1"
      },
      {
        "id": "2f73b7da-47f8-4853-ac67-560e2ba62ed9",
        "name": "Partner 2"
      }
    ],
    "totalCount": 2,
    "totalPages": 1
  }

#### 2. `POST /Partners`
- **Description**: Creates a new partner and saves it into the database. Accepts a request body:
  ```json
  {
    "name": "Partner name"
  }

- **Response**: Returns 201 Created.

#### 3. `PUT /Partners`
- **Description**: Updates a partner and saves the changes into the database. Accepts a request body:
  ```json
  {
    "id": "a1829edd-6ff7-44be-ac15-37cb8a21c379",
    "name": "Partner name"
  }

- **Response**: Returns 204 No Content.

#### 4. `GET /Partners{id}`
- **Description**: Fetches a single partner by the requested id passed as a route parameter.
- **Response**:
  ```json
  {
    "id": "0c35c3fc-b59b-4c11-977a-e38a7ce807ea",
    "name": "Test Partner"
  }

#### 5. `DELETE /Partners{id}`
- **Description**: Deletes a partner from the database via an id passed as a route parameter.
- **Response**: Returns 200 Ok.

#### 6. `GET /Partners/ExportCsv`
- **Description**: Feteches all partners and puts them in a csv file.
- **Response**: Returns a `Partners.csv` file, which can be downloaded from swagger.

#### 7. `GET /Partners/{id}/Merchants`
- **Description**: Feteches merchants belonging to a partner as a paginated result. The merchants are filtered by partner id provided as a route parameter.
- **Response**: Returns a paginated reuslt of merchants:
  ```json
  {
    "items": [
      {
        "id": "bb2e7ad5-fe2f-4bf0-a14b-6c645b0340be",
        "name": "Test merchant",
        "boardingDate": "2024-12-12T17:14:22.009",
        "url": "",
        "country": "Bulgaria",
        "firstAddress": "Some address",
        "secondAddress": "Some address 2",
        "partnerId": "0c35c3fc-b59b-4c11-977a-e38a7ce807ea"
      }
    ],
    "totalCount": 1,
    "totalPages": 1
  }

### Merchants

#### 8. `GET /Merchants`
- **Description**: Fetches merchants as a paginated result, using `pageNumber` and `pageSize` passed as query parameters.
- **Response**: Returns a paginated reuslt of merchants:
  ```json
  {
    "items": [
        {
          "id": "bb2e7ad5-fe2f-4bf0-a14b-6c645b0340be",
          "name": "Test merchant",
          "boardingDate": "2024-12-12T17:14:22.009",
          "url": "",
          "country": "Bulgaria",
          "firstAddress": "Some address",
          "secondAddress": "Some address 2",
          "partnerId": "0c35c3fc-b59b-4c11-977a-e38a7ce807ea"
        },
        {
          "id": "2995dd89-c25f-476d-b5f2-0269ba8dec00",
          "name": "Test merchant",
          "boardingDate": "2024-12-12T17:14:22.009",
          "url": "",
          "country": "Bulgaria",
          "firstAddress": "Some address",
          "secondAddress": "Some address 2",
          "partnerId": "0c35c3fc-b59b-4c11-977a-e38a7ce807ea"
        }
    ],
    "totalCount": 2,
    "totalPages": 1
  }

#### 9. `POST /Merchants`
- **Description**: Creates a new partner and saves it into the database. Accepts a request body:
  ```json
  {
    "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    "name": "string",
    "boardingDate": "2024-12-12T20:42:06.373Z",
    "url": "string",
    "country": "string",
    "firstAddress": "string",
    "secondAddress": "string",
    "partnerId": "3fa85f64-5717-4562-b3fc-2c963f66afa6"
  }

- **Response**: Returns 201 Created.

#### 10. `PUT /Merchants`
- **Description**: Updates a merchant and saves the changes into the database. Accepts a request body:
  ```json
  {
    "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    "name": "string",
    "boardingDate": "2024-12-12T20:42:06.373Z",
    "url": "string",
    "country": "string",
    "firstAddress": "string",
    "secondAddress": "string",
    "partnerId": "3fa85f64-5717-4562-b3fc-2c963f66afa6"
  }

- **Response**: Returns 204 No Content.

#### 11. `GET /Merchants/{id}`
- **Description**: Fetches a single merchant by the requested id passed as a route parameter.
- **Response**:
  ```json
  {
    "id": "bb2e7ad5-fe2f-4bf0-a14b-6c645b0340be",
    "name": "Test merchant",
    "boardingDate": "2024-12-12T17:14:22.009",
    "url": null,
    "country": "Bulgaria",
    "firstAddress": "Some address",
    "secondAddress": null,
    "partnerId": "0c35c3fc-b59b-4c11-977a-e38a7ce807ea"
  }

#### 12. `DELETE /Merchants/{id}`
- **Description**: Deletes a merchant from the database via an id passed as a route parameter.
- **Response**: Returns 200 Ok.


#### 13. `GET /Merchants/ExportCsv`
- **Description**: Feteches all merchants and puts them in a csv file.
- **Response**: Returns a `Merchants.csv` file, which can be downloaded from swagger.

#### 14. `GET /Merchants/{id}/Partner`
- **Description**: Fetches a single partner belonging to a specific merchant. The merchant id is passed as a route parameter.
- **Response**:
  ```json
  {
    "id": "0c35c3fc-b59b-4c11-977a-e38a7ce807ea",
    "name": "Test Partner"
  }

#### 15. `GET /Merchants/{id}/Transactions`
- **Description**: Feteches transactions belonging to a merchant as a paginated result. The merchants are filtered by merchant id passed as a route parameter.
- **Response**: Returns a paginated reuslt of transactions:
  ```json
  {
    "items": [
        {
          "id": "5bed38ad-3cd3-4c85-b6f6-cd412781696d",
          "createDate": "2023-02-22T07:08:59.679",
          "direction": "D",
          "amount": 111.11,
          "currency": "EUR",
          "deptorIBAN": "NL68INGB5831335380",
          "beneficiaryIBAN": "BG83IORT80949736921315",
          "status": 1,
          "externalId": "123213123123",
          "merchantId": null
        },
        {
          "id": "2ac300e3-00ba-430e-8497-e58ad52b4e7e",
          "createDate": "2023-02-22T07:07:44.123",
          "direction": "C",
          "amount": 1231.13,
          "currency": "EUR",
          "deptorIBAN": "NL68INGB5831335380",
          "beneficiaryIBAN": "BG90RZBB91552112199351",
          "status": 0,
          "externalId": "123213234123",
          "merchantId": null
        }
    ],
    "totalCount": 2,
    "totalPages": 1
  }

### Transactions

#### 16. `GET /Transactions`
- **Description**: Fetches transactions as a paginated result, using `pageNumber` and `pageSize` passed as query parameters. 
The transactions also allow for filtering by different criteria - `CreateDateFrom`, `CreateDateTo`, `Direction`, `AmountMin`, `AmountMax`, `Currency`, `DeptorIBAN`, `BeneficiaryIBAN` and `Status`.
- **Response**: Returns a paginated reuslt of transactions:
  ```json
  {
    "items": [
        {
          "id": "5bed38ad-3cd3-4c85-b6f6-cd412781696d",
          "createDate": "2023-02-22T07:08:59.679",
          "direction": "D",
          "amount": 111.11,
          "currency": "EUR",
          "deptorIBAN": "NL68INGB5831335380",
          "beneficiaryIBAN": "BG83IORT80949736921315",
          "status": 1,
          "externalId": "123213123123",
          "merchantId": null
        },
        {
          "id": "2ac300e3-00ba-430e-8497-e58ad52b4e7e",
          "createDate": "2023-02-22T07:07:44.123",
          "direction": "C",
          "amount": 1231.13,
          "currency": "EUR",
          "deptorIBAN": "NL68INGB5831335380",
          "beneficiaryIBAN": "BG90RZBB91552112199351",
          "status": 0,
          "externalId": "123213234123",
          "merchantId": null
        }
    ],
    "totalCount": 2,
    "totalPages": 1
  }

#### 17. `GET /Transactions/{id}`
- **Description**: Fetches a single transaction by the requested id passed as a route parameter.
- **Response**: 
  ```json
  {
    "id": "5bed38ad-3cd3-4c85-b6f6-cd412781696d",
    "createDate": "2023-02-22T07:08:59.679",
    "direction": "D",
    "amount": 111.11,
    "currency": "EUR",
    "deptorIBAN": "NL68INGB5831335380",
    "beneficiaryIBAN": "BG83IORT80949736921315",
    "status": 1,
    "externalId": "123213123123",
    "merchantId": null
  }

#### 18. `GET /Transactions/ExportCsv`
- **Description**: Feteches all transactions and puts them in a csv file.
- **Response**: Returns a `Transactions.csv` file, which can be downloaded from swagger.

#### 19. `GET /Transactions/{id}/Merchant`
- **Description**: Fetches a single merchant belonging to a specific transaction. The transaction id is passed as a route parameter.
- **Response**:
  ```json
  {
    "id": "bb2e7ad5-fe2f-4bf0-a14b-6c645b0340be",
    "name": "Test merchant",
    "boardingDate": "2024-12-12T17:14:22.009",
    "url": null,
    "country": "Bulgaria",
    "firstAddress": "Some address",
    "secondAddress": null,
    "partnerId": "0c35c3fc-b59b-4c11-977a-e38a7ce807ea"
  }

#### 20. `POST /Transactions/ImportXml`
- **Description**: Accepts an .xml file with one or more transactions in it. The transactions are read from the file and persisted in the database.
- **Response**: Returns 201 Created.