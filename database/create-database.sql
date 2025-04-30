-- Script to create a database in SQL Server
USE master;
GO

-- Check if the database already exists and delete it if necessary
IF EXISTS (SELECT name FROM sys.databases WHERE name = 'BookPetGrooming')
BEGIN
    ALTER DATABASE BookPetGrooming SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE BookPetGrooming;
END
GO

-- Create the database
CREATE DATABASE BookPetGrooming
ON PRIMARY (
    NAME = 'BookPetGrooming_Data',
    FILENAME = 'C:\SQLData\BookPetGrooming_Data.mdf',
    SIZE = 10MB,
    MAXSIZE = UNLIMITED,
    FILEGROWTH = 10%
)
LOG ON (
    NAME = 'BookPetGrooming_Log',
    FILENAME = 'C:\SQLData\BookPetGrooming_Log.ldf',
    SIZE = 5MB,
    MAXSIZE = UNLIMITED,
    FILEGROWTH = 10%
);
GO

-- Set basic configuration options
ALTER DATABASE BookPetGrooming SET RECOVERY SIMPLE;
ALTER DATABASE BookPetGrooming SET AUTO_SHRINK OFF;
ALTER DATABASE BookPetGrooming SET AUTO_CREATE_STATISTICS ON;
ALTER DATABASE BookPetGrooming SET AUTO_UPDATE_STATISTICS ON;
GO

-- Confirm that the database was created
SELECT name, database_id, create_date FROM sys.databases WHERE name = 'BookPetGrooming';
GO