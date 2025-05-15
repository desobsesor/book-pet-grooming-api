-- SQL Server Database Schema for Pet Grooming Booking System
USE master;
GO

-- Create database if it doesn't exist
IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'BookPetGrooming')
BEGIN
    CREATE DATABASE BookPetGrooming;
END;
GO

USE BookPetGrooming;
GO

-- Drop tables if they exist (for clean setup)
IF OBJECT_ID('notifications', 'U') IS NOT NULL DROP TABLE notifications;
IF OBJECT_ID('appointments', 'U') IS NOT NULL DROP TABLE appointments;
IF OBJECT_ID('pets', 'U') IS NOT NULL DROP TABLE pets;
IF OBJECT_ID('pet_categories', 'U') IS NOT NULL DROP TABLE pet_categories;
IF OBJECT_ID('breeds', 'U') IS NOT NULL DROP TABLE breeds;
IF OBJECT_ID('customers', 'U') IS NOT NULL DROP TABLE customers;
IF OBJECT_ID('groomers', 'U') IS NOT NULL DROP TABLE groomers;
IF OBJECT_ID('sessions', 'U') IS NOT NULL DROP TABLE sessions;
IF OBJECT_ID('users', 'U') IS NOT NULL DROP TABLE users;
GO

-- Create tables

-- Users table
CREATE TABLE users (
    user_id INT IDENTITY(1,1) PRIMARY KEY,
    username NVARCHAR(50) NOT NULL UNIQUE,
    email NVARCHAR(100) NOT NULL UNIQUE,
    password_hash NVARCHAR(255) NOT NULL,
    role NVARCHAR(20) NOT NULL,
    last_login DATETIME2,
    is_active BIT DEFAULT 1,
    created_at DATETIME2 DEFAULT GETDATE(),
    updated_at DATETIME2 DEFAULT GETDATE(),
    CONSTRAINT chk_role CHECK (role IN ('admin', 'groomer', 'customer'))
);

-- Sessions table
CREATE TABLE sessions (
    session_id INT IDENTITY(1,1) PRIMARY KEY,
    user_id INT NOT NULL REFERENCES users(user_id),
    token NVARCHAR(255) NOT NULL UNIQUE,
    ip_address NVARCHAR(50),
    user_agent NVARCHAR(255),
    expires_at DATETIME2 NOT NULL,
    created_at DATETIME2 DEFAULT GETDATE(),
    updated_at DATETIME2 DEFAULT GETDATE()
);

-- Groomers table
CREATE TABLE groomers (
    groomer_id INT IDENTITY(1,1) PRIMARY KEY,
    user_id INT REFERENCES users(user_id),
    first_name NVARCHAR(50) NOT NULL,
    last_name NVARCHAR(50) NOT NULL,
    email NVARCHAR(100) NOT NULL UNIQUE,
    phone NVARCHAR(20),
    specialization NVARCHAR(100),
    years_of_experience INT,
    is_active BIT DEFAULT 1,
    created_at DATETIME2 DEFAULT GETDATE(),
    updated_at DATETIME2 DEFAULT GETDATE()
);

-- Customers table
CREATE TABLE customers (
    customer_id INT IDENTITY(1,1) PRIMARY KEY,
    user_id INT REFERENCES users(user_id),
    first_name NVARCHAR(50) NOT NULL,
    last_name NVARCHAR(50) NOT NULL,
    email NVARCHAR(100) NOT NULL UNIQUE,
    phone NVARCHAR(20) NOT NULL,
    address NVARCHAR(255),
    preferred_groomer_id INT REFERENCES groomers(groomer_id),
    created_at DATETIME2 DEFAULT GETDATE(),
    updated_at DATETIME2 DEFAULT GETDATE()
);

-- Breeds table
CREATE TABLE breeds (
    breed_id INT IDENTITY(1,1) PRIMARY KEY,
    name NVARCHAR(100) NOT NULL UNIQUE,
    species NVARCHAR(50) NOT NULL, -- Dog, Cat, etc.
    coat_type NVARCHAR(50), -- Long, Short, Curly, etc.
    grooming_difficulty INT, -- Scale 1-5
    created_at DATETIME2 DEFAULT GETDATE(),
    updated_at DATETIME2 DEFAULT GETDATE()
);

-- Pet Categories table
CREATE TABLE pet_categories (
    category_id INT IDENTITY(1,1) PRIMARY KEY,
    name NVARCHAR(50) NOT NULL UNIQUE, -- Puppy, Adult
    description NVARCHAR(MAX),
    created_at DATETIME2 DEFAULT GETDATE(),
    updated_at DATETIME2 DEFAULT GETDATE()
);

-- Pets table
CREATE TABLE pets (
    pet_id INT IDENTITY(1,1) PRIMARY KEY,
    customer_id INT NOT NULL REFERENCES customers(customer_id),
    name NVARCHAR(100) NOT NULL,
    breed_id INT NOT NULL REFERENCES breeds(breed_id),
    category_id INT NOT NULL REFERENCES pet_categories(category_id),
    weight DECIMAL(5,2) NOT NULL, -- in kg
    date_of_birth DATE,
    gender NVARCHAR(15),
    allergies NVARCHAR(MAX),
    notes NVARCHAR(MAX),
    created_at DATETIME2 DEFAULT GETDATE(),
    updated_at DATETIME2 DEFAULT GETDATE()
);

-- Appointments table
CREATE TABLE appointments (
    appointment_id INT IDENTITY(1,1) PRIMARY KEY,
    pet_id INT NOT NULL REFERENCES pets(pet_id),
    groomer_id INT REFERENCES groomers(groomer_id),
    created_by_user_id INT NOT NULL REFERENCES users(user_id),
    appointment_date DATE NOT NULL,
    start_time TIME NOT NULL,
    estimated_duration INT NOT NULL, -- in minutes
    status NVARCHAR(20) NOT NULL DEFAULT 'pending',
    price DECIMAL(10,2),
    notes NVARCHAR(MAX),
    created_at DATETIME2 DEFAULT GETDATE(),
    updated_at DATETIME2 DEFAULT GETDATE(),
    CONSTRAINT chk_status CHECK (status IN ('pending', 'approved', 'rejected', 'completed', 'cancelled'))
);

-- Notifications table
CREATE TABLE notifications (
    notification_id INT IDENTITY(1,1) PRIMARY KEY,
    appointment_id INT NOT NULL REFERENCES appointments(appointment_id),
    recipient_type NVARCHAR(20) NOT NULL,
    message NVARCHAR(MAX) NOT NULL,
    is_read BIT DEFAULT 0,
    created_at DATETIME2 DEFAULT GETDATE(),
    updated_at DATETIME2 DEFAULT GETDATE(),
    CONSTRAINT chk_recipient_type CHECK (recipient_type IN ('customer', 'groomer'))
);
GO

-- Create indexes for performance
CREATE INDEX idx_pets_customer_id ON pets(customer_id);
CREATE INDEX idx_appointments_pet_id ON appointments(pet_id);
CREATE INDEX idx_appointments_groomer_id ON appointments(groomer_id);
CREATE INDEX idx_appointments_status ON appointments(status);
CREATE INDEX idx_appointments_created_by_user_id ON appointments(created_by_user_id);
CREATE INDEX idx_notifications_appointment_id ON notifications(appointment_id);
CREATE INDEX idx_groomers_user_id ON groomers(user_id);
CREATE INDEX idx_customers_user_id ON customers(user_id);
CREATE INDEX idx_sessions_user_id ON sessions(user_id);
CREATE INDEX idx_sessions_token ON sessions(token);
GO

-- Insert initial data for pet categories
INSERT INTO pet_categories (name, description) VALUES
('Puppy', 'Young pet under 1 year old'),
('Adult', 'Pet over 1 year old');
GO

-- Create a view for appointment details
CREATE OR ALTER VIEW appointment_details AS
SELECT 
    a.appointment_id,
    a.appointment_date,
    a.start_time,
    a.estimated_duration,
    a.status,
    a.price,
    p.pet_id,
    p.name AS pet_name,
    b.name AS breed_name,
    pc.name AS pet_category,
    p.weight,
    c.customer_id,
    c.first_name AS customer_first_name,
    c.last_name AS customer_last_name,
    c.email AS customer_email,
    c.phone AS customer_phone,
    g.groomer_id,
    g.first_name AS groomer_first_name,
    g.last_name AS groomer_last_name,
    a.created_by_user_id,
    u.username AS created_by_username,
    u.role AS created_by_role
FROM 
    appointments a
JOIN 
    pets p ON a.pet_id = p.pet_id
JOIN 
    breeds b ON p.breed_id = b.breed_id
JOIN 
    pet_categories pc ON p.category_id = pc.category_id
JOIN 
    customers c ON p.customer_id = c.customer_id
LEFT JOIN 
    groomers g ON a.groomer_id = g.groomer_id
JOIN 
    users u ON a.created_by_user_id = u.user_id;
GO

-- Create function to calculate grooming price
CREATE OR ALTER FUNCTION calculate_grooming_price
(
    @breed_id INT,
    @category_id INT,
    @weight DECIMAL(5,2)
)
RETURNS DECIMAL(10,2)
AS
BEGIN
    DECLARE @base_price DECIMAL(10,2);
    DECLARE @difficulty INT;
    DECLARE @category_factor DECIMAL(10,2);

    -- Get the grooming difficulty from the breed
    SELECT @difficulty = grooming_difficulty FROM breeds WHERE breed_id = @breed_id;
    
    -- Set category factor (puppies might be cheaper than adults)
    SELECT @category_factor = CASE 
        WHEN name = 'Puppy' THEN 0.8
        ELSE 1.0
    END
    FROM pet_categories 
    WHERE category_id = @category_id;
    
    -- Calculate base price based on difficulty (1-5 scale)
    SET @base_price = 20.0 + (@difficulty * 5.0);
    
    -- Return final price
    RETURN (@base_price + (@weight * 0.5)) * @category_factor;
END;
GO

-- Create trigger to set appointment price
CREATE OR ALTER TRIGGER trg_set_appointment_price
ON appointments
AFTER INSERT, UPDATE
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE a
    SET price = dbo.calculate_grooming_price(p.breed_id, p.category_id, p.weight)
    FROM appointments a
    INNER JOIN inserted i ON a.appointment_id = i.appointment_id
    INNER JOIN pets p ON i.pet_id = p.pet_id;
END;
GO

-- Create trigger for appointment notifications
CREATE OR ALTER TRIGGER trg_create_appointment_notification
ON appointments
AFTER INSERT, UPDATE
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @notification_message NVARCHAR(MAX);
    
    -- Handle status changes
    INSERT INTO notifications (appointment_id, recipient_type, message)
    SELECT 
        i.appointment_id,
        'customer',
        CASE i.status
            WHEN 'approved' THEN 'Your appointment has been approved.'
            WHEN 'rejected' THEN 'Your appointment has been rejected.'
            WHEN 'completed' THEN 'Your appointment has been marked as completed.'
            WHEN 'cancelled' THEN 'Your appointment has been cancelled.'
            ELSE 'Your appointment status has been updated to ' + i.status + '.'
        END
    FROM inserted i
    LEFT JOIN deleted d ON i.appointment_id = d.appointment_id
    WHERE d.status IS NULL OR i.status <> d.status;

    -- Notify groomer for new appointments
    INSERT INTO notifications (appointment_id, recipient_type, message)
    SELECT 
        i.appointment_id,
        'groomer',
        'New appointment request is waiting for your approval.'
    FROM inserted i
    LEFT JOIN deleted d ON i.appointment_id = d.appointment_id
    WHERE i.status = 'pending' AND d.status IS NULL;
END;
GO