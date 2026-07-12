# Wallet API Transfer - Payphone

API para administrar billeteras electrónicas, realizar transferencias y consultar movimientos y detalle de billetereas.

## Información general

| Propiedad | Valor |
|-----------|--------|
| Versión | v1 |
| Base URL | https://localhost:7289/api/v1 |
| Formato | JSON |
| Autenticación | Bearer Token |
---
## Incializar proyecto:

La base de datos es SQL-SERVER y se debe correr el script llamado baseDatos.sql antes de iniciar el proyecto.

Modificar la cadena de conexión de appsettings.json del proyecto api para que apunte al server donde se cree (Si se cambia de nombre la DB también se debe modificar).

  "ConnectionStrings": {
    "PayphoneConnection": "Server=(localdb)\\MSSQLLocalDB;Database=PAYPHONEWALLETS;Trusted_Connection=True;TrustServerCertificate=True;"
  },

  Esta cadena de conexión para efectos de prueba se dejó en este archivo y en claro, lo correcto es guardar valores sensibles en un vault / secret o cualquier otro baúl de datos para no exponer datos, así como la configuración del JWT, para efectos de prueba esta se dejó de esa manera.

---
Proceso:
1. Correr script
2. Levantar proyecto
3. Pedir token auth
4. Crear wallets (2 mínimo para transferencias)
5. Consultar wallet
6. Realizar transferencia
7. Consultar movimientos

# Se adjunta colección de postman para realizar pruebas.

---
# Autenticación

Los endpoints /wallet, /wallet/{idWallet}, /transfer requieren un token JWT generado con el endpoint /auth/login.

Ejemplo:

Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoicGF5cGhvbmUiLCJqdGkiOiI2MTZmM2MwYS1lNWNkLTQyMjAtYTEwMy03OGI4NTUxNzc5ZmMiLCJleHAiOjE3ODM4Mzg0OTQsImlzcyI6IlRyYW5zZmVyU2VydmljZS5BcGkiLCJhdWQiOiJUcmFuc2ZlclNlcnZpY2UuQ2xpZW50cyJ9.DU5LCO5TOwjZRonZyesvaauAhMoWzaX7uuXDUIszLIE

---

# Login

Genera token para login en endpoints: /wallet, /wallet/{idWallet}, /transfer

## Endpoint

POST /auth/login

## Parámetros

| Nombre | Tipo | Requerido | Descripción |
|---------|------|-----------|-------------|
| username | string | Sí | usuario |
| password | string | Sí | password |


## Request

```json
{
  "username": "payphone",
  "password": "payphonepassword"
}

```

## Response 200

```json
{
    "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoicGF5cGhvbmUiLCJqdGkiOiIzNWRhMTRmNi05ZmIzLTQ2NmYtODViYy0wZjZlYTk2NjM4NjAiLCJleHAiOjE3ODM4Mzg3MDMsImlzcyI6IlRyYW5zZmVyU2VydmljZS5BcGkiLCJhdWQiOiJUcmFuc2ZlclNlcnZpY2UuQ2xpZW50cyJ9.QwKyeSWkEkGmiGawQjjy0LHcFmhECtd_S02gC2JPUmk"
}
```

## Response 401

```json
{
    "error": "Invalid credentials"
}
```

## Posibles códigos

| Código | Descripción |
|---------|-------------|
| 200 | Operación exitosa  |
| 401 | No autorizado |


# Crear Wallet

Crear una billetera.

## Endpoint

POST /wallet

## Parámetros

| Nombre | Tipo | Requerido | Descripción |
|---------|------|-----------|-------------|
| documentId | string | Sí | Id del cliente (13 caracteres del 0 al 9 formato HN para pruebas) |
| name | string | Sí | Nombre del dueño de wallet |
| initialBalance | money / decimal | Sí | Saldo inicial (puede ser 0 o mayor a 0) |

## Request

```json
Este debe llevar el bearer token generado en por en endpoint login

{
  "documentId": "0101199514445",
  "name": "Junior Moreno",
  "initialBalance": 800
}
```

## Response 201

```json
{
    "id": 1010,
    "documentId": "0101199514445",
    "name": "Junior Moreno",
    "balance": 800,
    "createdAt": "2026-07-12T05:45:12.5296407Z",
    "updatedAt": "2026-07-12T05:45:12.5296407Z"
}
```

## Response 400

```json
{
    "errors": [
        "An identity document is mandatory.."
    ]
}
```

## Posibles códigos

| Código | Descripción |
|---------|-------------|
| 201 | Operación exitosa / Created |
| 400 | Solicitud inválida |
| 401 | No autorizado |


# Consultar Wallet

Consultar una billetera.

## Endpoint

GET /wallet/{idWallet}

## Parámetros

| Nombre | Tipo | Requerido | Descripción |
|---------|------|-----------|-------------|


## Request

```json
Este debe llevar el bearer token generado en por en endpoint login
```

## Response 200

```json
{
    "id": 1010,
    "documentId": "0101199514445",
    "name": "Junior Moreno",
    "balance": 800.0000,
    "createdAt": "2026-07-12T05:45:12.5296407",
    "updatedAt": "2026-07-12T05:45:12.5296407"
}
```

## Response 404

```json
{
    "error": "Wallet Not Found: Id 5000."
}

```

## Posibles códigos

| Código | Descripción |
|---------|-------------|
| 200 | Operación exitosa  |
| 404 | Not Found / Billetera no existe |
| 401 | No autorizado |

# Realizar transferencia

Realizar una transferencia entre cuentas

## Endpoint

POST /transfer

## Parámetros

| Nombre | Tipo | Requerido | Descripción |
|---------|------|-----------|-------------|
| fromWalletId | int | Sí | Billetera origen |
| toWalletId | string | Sí | Billetera destino |
| amount | money / decimal | Sí | Saldo de transferencia (debe ser mayor a 0) |
| transferId | Guid | Sí | Id de transacción (Debería enviarlo el cliente para idempotencia) |


## Request

```json
Este debe llevar el bearer token generado en por en endpoint login

{
  "fromWalletId": 1,
  "toWalletId": 2,
  "amount": 82.01,
  "transferId": "afc39955-34e8-4f93-8759-7aad22dc02d9"
}
```

## Response 201

```json
{
    "transferId": "afc39955-34e8-4f93-8759-7aad22dc02d9",
    "originWalletId": 1,
    "destinationWalletId": 2,
    "amount": 82.01,
    "createdAt": "2026-07-12T05:52:31.4097974Z"
}
```

## Response 404

```json
{
    "error": "Wallet Not Found: Id 8000."
}
```

## Response 400
```json

{
    "error": "The transfer amount must be greater than zero."
}
```

## Posibles códigos

| Código | Descripción |
|---------|-------------|
| 200 | Operación exitosa  |
| 404 | Not Found / Billetera no existe |
| 400 | Solicitud inválida |
| 401 | No autorizado |

# Consultar Movimientos

Consultar movimientos de wallets.

## Endpoint

GET /wallet/{idWallet}/movements

## Parámetros

| Nombre | Tipo | Requerido | Descripción |
|---------|------|-----------|-------------|



## Request

```json
No aplica
```

## Response 200

```json
{
    "movements": [
        {
            "walletId": 1,
            "amount": 82.0100,
            "type": "Debit",
            "createdAt": "2026-07-12T05:52:31.4097974",
            "transferId": "afc39955-34e8-4f93-8759-7aad22dc02d9"
        },
        {
            "walletId": 1,
            "amount": 82.0100,
            "type": "Debit",
            "createdAt": "2026-07-12T05:20:02.5284799",
            "transferId": "afc39955-33e8-4f93-8759-7aad22dc02d9"
        },
        {
            "walletId": 1,
            "amount": 82.0100,
            "type": "Debit",
            "createdAt": "2026-07-12T05:01:45.9300099",
            "transferId": "dfc39955-33e8-4f93-8759-7aad22dc02d9"
        },
        {
            "walletId": 1,
            "amount": 82.0100,
            "type": "Debit",
            "createdAt": "2026-07-12T01:47:34.5590748",
            "transferId": "dfc39955-33e8-4f93-9759-7aad22dc02d9"
        },
        {
            "walletId": 1,
            "amount": 68.7400,
            "type": "Debit",
            "createdAt": "2026-07-12T00:30:33.4082365",
            "transferId": "dfc39955-33e8-4f93-9759-7aad22dc02d6"
        },
        {
            "walletId": 1,
            "amount": 250.7500,
            "type": "Credit",
            "createdAt": "2026-07-12T00:07:12.0693055",
            "transferId": "3fa85f64-5717-4562-b3fc-2c963f66afa6"
        }
    ]
}
```

## Response 404

```json
{
    "error": "Wallet Not Found: Id 1922."
}
```

## Posibles códigos

| Código | Descripción |
|---------|-------------|
| 200 | Operación exitosa  |
| 404 | Not Found / Billetera no existe |
| 401 | No autorizado |

# Test

dotnet test desde la raíz del proyecto (Estos test afectarán la bd PAYPHONEWALLETS_TEST porque son test de integración, los unitarios no afectan nada)

