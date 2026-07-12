CREATE DATABASE PAYPHONEWALLETS;
GO

USE PAYPHONEWALLETS;
GO

CREATE TABLE Wallets (
    Id          INT IDENTITY(1,1) PRIMARY KEY,
    DocumentId  VARCHAR(20)    NOT NULL,
    Name        NVARCHAR(120)  NOT NULL,
    Balance     DECIMAL(19,4)  NOT NULL,
    CreatedAt   DATETIME2      NOT NULL,
    UpdatedAt   DATETIME2      NOT NULL
);


CREATE TABLE Movements (
    Id        INT IDENTITY(1,1) PRIMARY KEY,
    WalletId  INT            NOT NULL REFERENCES Wallets(Id),
    Amount    DECIMAL(19,4)  NOT NULL,
    Type      VARCHAR(10)    NOT NULL,  
    CreatedAt DATETIME2      NOT NULL,
    TransferId  UNIQUEIDENTIFIER NOT NULL
);

CREATE INDEX IX_Movements_WalletId ON Movements(WalletId);

CREATE UNIQUE INDEX UX_Movements_TransferId_Type ON Movements(TransferId, Type);

--------------- BD PARA TEST ----------------------
CREATE DATABASE PAYPHONEWALLETS_TEST;
GO

USE PAYPHONEWALLETS_TEST;
GO

CREATE TABLE Wallets (
    Id          INT IDENTITY(1,1) PRIMARY KEY,
    DocumentId  VARCHAR(20)    NOT NULL,
    Name        NVARCHAR(120)  NOT NULL,
    Balance     DECIMAL(19,4)  NOT NULL,
    CreatedAt   DATETIME2      NOT NULL,
    UpdatedAt   DATETIME2      NOT NULL
);


CREATE TABLE Movements (
    Id        INT IDENTITY(1,1) PRIMARY KEY,
    WalletId  INT            NOT NULL REFERENCES Wallets(Id),
    Amount    DECIMAL(19,4)  NOT NULL,
    Type      VARCHAR(10)    NOT NULL,  
    CreatedAt DATETIME2      NOT NULL,
    TransferId  UNIQUEIDENTIFIER NOT NULL
);

CREATE INDEX IX_Movements_WalletId ON Movements(WalletId);

CREATE UNIQUE INDEX UX_Movements_TransferId_Type ON Movements(TransferId, Type);

