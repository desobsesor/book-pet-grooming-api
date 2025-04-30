-- PostgreSQL Database Schema for Pet Grooming Booking System
CREATE SCHEMA IF NOT EXISTS book_pet_grooming_system;
SET search_path TO book_pet_grooming_system;

-- Drop tables if they exist (for clean setup)
DROP TABLE IF EXISTS notifications CASCADE;
DROP TABLE IF EXISTS appointments CASCADE;
DROP TABLE IF EXISTS pets CASCADE;
DROP TABLE IF EXISTS pet_categories CASCADE;
DROP TABLE IF EXISTS breeds CASCADE;
DROP TABLE IF EXISTS customers CASCADE;
DROP TABLE IF EXISTS groomers CASCADE;

-- Create tables

-- Groomers table
CREATE TABLE groomers (
    groomer_id SERIAL PRIMARY KEY,
    first_name VARCHAR(50) NOT NULL,
    last_name VARCHAR(50) NOT NULL,
    email VARCHAR(100) UNIQUE NOT NULL,
    phone VARCHAR(20),
    specialization VARCHAR(100),
    years_of_experience INTEGER,
    is_active BOOLEAN DEFAULT TRUE,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

-- Customers table
CREATE TABLE customers (
    customer_id SERIAL PRIMARY KEY,
    first_name VARCHAR(50) NOT NULL,
    last_name VARCHAR(50) NOT NULL,
    email VARCHAR(100) UNIQUE NOT NULL,
    phone VARCHAR(20) NOT NULL,
    address VARCHAR(255),
    preferred_groomer_id INTEGER REFERENCES groomers(groomer_id),
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

-- Breeds table
CREATE TABLE breeds (
    breed_id SERIAL PRIMARY KEY,
    name VARCHAR(100) UNIQUE NOT NULL,
    species VARCHAR(50) NOT NULL, -- Dog, Cat, etc.
    coat_type VARCHAR(50), -- Long, Short, Curly, etc.
    grooming_difficulty INTEGER, -- Scale 1-5
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

-- Pet Categories table (puppy or adult)
CREATE TABLE pet_categories (
    category_id SERIAL PRIMARY KEY,
    name VARCHAR(50) UNIQUE NOT NULL, -- Puppy, Adult
    description TEXT,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

-- Pets table
CREATE TABLE pets (
    pet_id SERIAL PRIMARY KEY,
    customer_id INTEGER NOT NULL REFERENCES customers(customer_id),
    name VARCHAR(100) NOT NULL,
    breed_id INTEGER NOT NULL REFERENCES breeds(breed_id),
    category_id INTEGER NOT NULL REFERENCES pet_categories(category_id),
    weight DECIMAL(5,2) NOT NULL, -- in kg
    date_of_birth DATE,
    notes TEXT,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

-- Appointments table
CREATE TABLE appointments (
    appointment_id SERIAL PRIMARY KEY,
    pet_id INTEGER NOT NULL REFERENCES pets(pet_id),
    groomer_id INTEGER REFERENCES groomers(groomer_id),
    appointment_date DATE NOT NULL,
    start_time TIME NOT NULL,
    estimated_duration INTEGER NOT NULL, -- in minutes
    status VARCHAR(20) NOT NULL DEFAULT 'pending', -- pending, approved, rejected, completed, cancelled
    price DECIMAL(10,2),
    notes TEXT,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    CONSTRAINT valid_status CHECK (status IN ('pending', 'approved', 'rejected', 'completed', 'cancelled'))
);

-- Notifications table
CREATE TABLE notifications (
    notification_id SERIAL PRIMARY KEY,
    appointment_id INTEGER NOT NULL REFERENCES appointments(appointment_id),
    recipient_type VARCHAR(20) NOT NULL, -- customer, groomer
    message TEXT NOT NULL,
    is_read BOOLEAN DEFAULT FALSE,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    CONSTRAINT valid_recipient_type CHECK (recipient_type IN ('customer', 'groomer'))
);

-- Create indexes for performance
CREATE INDEX idx_pets_customer_id ON pets(customer_id);
CREATE INDEX idx_appointments_pet_id ON appointments(pet_id);
CREATE INDEX idx_appointments_groomer_id ON appointments(groomer_id);
CREATE INDEX idx_appointments_status ON appointments(status);
CREATE INDEX idx_notifications_appointment_id ON notifications(appointment_id);

-- Insert initial data for pet categories
INSERT INTO pet_categories (name, description) VALUES
('Puppy', 'Young pet under 1 year old'),
('Adult', 'Pet over 1 year old');

-- Create a view for appointment details
CREATE OR REPLACE VIEW appointment_details AS
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
    g.last_name AS groomer_last_name
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
    groomers g ON a.groomer_id = g.groomer_id;

-- Create a function to calculate price based on breed and category
CREATE OR REPLACE FUNCTION calculate_grooming_price(
    p_breed_id INTEGER,
    p_category_id INTEGER,
    p_weight DECIMAL
) RETURNS DECIMAL AS $$
DECLARE
    base_price DECIMAL;
    difficulty INTEGER;
    category_factor DECIMAL;
BEGIN
    -- Get the grooming difficulty from the breed
    SELECT grooming_difficulty INTO difficulty FROM breeds WHERE breed_id = p_breed_id;
    
    -- Set category factor (puppies might be cheaper than adults)
    IF (SELECT name FROM pet_categories WHERE category_id = p_category_id) = 'Puppy' THEN
        category_factor := 0.8;
    ELSE
        category_factor := 1.0;
    END IF;
    
    -- Calculate base price based on difficulty (1-5 scale)
    base_price := 20.0 + (difficulty * 5.0);
    
    -- Adjust price based on weight
    RETURN (base_price + (p_weight * 0.5)) * category_factor;
END;
$$ LANGUAGE plpgsql;

-- Create a trigger to automatically set the price when an appointment is created or updated
CREATE OR REPLACE FUNCTION set_appointment_price()
RETURNS TRIGGER AS $$
DECLARE
    pet_breed_id INTEGER;
    pet_category_id INTEGER;
    pet_weight DECIMAL;
BEGIN
    -- Get pet details
    SELECT breed_id, category_id, weight 
    INTO pet_breed_id, pet_category_id, pet_weight
    FROM pets 
    WHERE pet_id = NEW.pet_id;
    
    -- Calculate and set the price
    NEW.price := calculate_grooming_price(pet_breed_id, pet_category_id, pet_weight);
    
    RETURN NEW;
END;
$$ LANGUAGE plpgsql;

CREATE TRIGGER before_appointment_insert_update
BEFORE INSERT OR UPDATE ON appointments
FOR EACH ROW
EXECUTE FUNCTION set_appointment_price();

-- Create a function to automatically create a notification when appointment status changes
CREATE OR REPLACE FUNCTION create_appointment_notification()
RETURNS TRIGGER AS $$
DECLARE
    notification_message TEXT;
BEGIN
    -- Only create notification if status has changed
    IF OLD.status IS NULL OR NEW.status <> OLD.status THEN
        -- Create message based on new status
        CASE NEW.status
            WHEN 'approved' THEN
                notification_message := 'Your appointment has been approved.';
            WHEN 'rejected' THEN
                notification_message := 'Your appointment has been rejected.';
            WHEN 'completed' THEN
                notification_message := 'Your appointment has been marked as completed.';
            WHEN 'cancelled' THEN
                notification_message := 'Your appointment has been cancelled.';
            ELSE
                notification_message := 'Your appointment status has been updated to ' || NEW.status || '.';
        END CASE;
        
        -- Insert notification for customer
        INSERT INTO notifications (appointment_id, recipient_type, message)
        VALUES (NEW.appointment_id, 'customer', notification_message);
        
        -- If appointment is new (pending), also notify groomer
        IF NEW.status = 'pending' AND OLD.status IS NULL THEN
            INSERT INTO notifications (appointment_id, recipient_type, message)
            VALUES (NEW.appointment_id, 'groomer', 'New appointment request is waiting for your approval.');
        END IF;
    END IF;
    
    RETURN NEW;
END;
$$ LANGUAGE plpgsql;

CREATE TRIGGER after_appointment_status_change
AFTER INSERT OR UPDATE ON appointments
FOR EACH ROW
EXECUTE FUNCTION create_appointment_notification();