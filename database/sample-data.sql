-- Script to insert test data into the BookPetGrooming database
USE BookPetGrooming;
GO

-- Insert users (if they don't already exist in initial-users.sql)
IF NOT EXISTS (SELECT 1 FROM users WHERE username = 'admin')
BEGIN
    INSERT INTO users (username, email, password_hash, role, last_login, is_active)
    VALUES
        ('admin', 'admin@petgrooming.com', 'hashed_password_123', 'admin', GETDATE(), 1),
        ('groomer1', 'groomer1@petgrooming.com', 'hashed_password_456', 'groomer', GETDATE(), 1),
        ('groomer2', 'groomer2@petgrooming.com', 'hashed_password_789', 'groomer', GETDATE(), 1),
        ('groomer3', 'groomer3@petgrooming.com', 'hashed_password_101', 'groomer', GETDATE(), 1),
        ('customer1', 'customer1@example.com', 'hashed_password_102', 'customer', GETDATE(), 1),
        ('customer2', 'customer2@example.com', 'hashed_password_103', 'customer', GETDATE(), 1),
        ('customer3', 'customer3@example.com', 'hashed_password_104', 'customer', GETDATE(), 1),
        ('customer4', 'customer4@example.com', 'hashed_password_105', 'customer', GETDATE(), 1),
        ('customer5', 'customer5@example.com', 'hashed_password_106', 'customer', GETDATE(), 1);
END
GO

-- Insert groomers
IF NOT EXISTS (SELECT 1 FROM groomers WHERE email = 'groomer1@petgrooming.com')
BEGIN
    INSERT INTO groomers (user_id, first_name, last_name, email, phone, specialization, years_of_experience, is_active)
    VALUES
        ((SELECT user_id FROM users WHERE username = 'groomer1'), 'Carlos', 'Rodriguez', 'groomer1@petgrooming.com', '555-123-4567', 'Small Dogs', 5, 1),
        ((SELECT user_id FROM users WHERE username = 'groomer2'), 'Maria', 'Gonzalez', 'groomer2@petgrooming.com', '555-234-5678', 'Cats', 3, 1),
        ((SELECT user_id FROM users WHERE username = 'groomer3'), 'Juan', 'Martinez', 'groomer3@petgrooming.com', '555-345-6789', 'Large Dogs', 7, 1);
END
GO

-- Insert customers
IF NOT EXISTS (SELECT 1 FROM customers WHERE email = 'customer1@example.com')
BEGIN
    INSERT INTO customers (user_id, first_name, last_name, email, phone, address, preferred_groomer_id)
    VALUES
        ((SELECT user_id FROM users WHERE username = 'customer1'), 'Ana', 'Lopez', 'customer1@example.com', '555-456-7890', 'Main Street 123', (SELECT groomer_id FROM groomers WHERE email = 'groomer1@petgrooming.com')),
        ((SELECT user_id FROM users WHERE username = 'customer2'), 'Pedro', 'Sanchez', 'customer2@example.com', '555-567-8901', 'Central Avenue 456', (SELECT groomer_id FROM groomers WHERE email = 'groomer2@petgrooming.com')),
        ((SELECT user_id FROM users WHERE username = 'customer3'), 'Laura', 'Fernandez', 'customer3@example.com', '555-678-9012', 'Main Square 789', NULL),
        ((SELECT user_id FROM users WHERE username = 'customer4'), 'Miguel', 'Torres', 'customer4@example.com', '555-789-0123', 'Secondary Street 101', (SELECT groomer_id FROM groomers WHERE email = 'groomer3@petgrooming.com')),
        ((SELECT user_id FROM users WHERE username = 'customer5'), 'Sofia', 'Ramirez', 'customer5@example.com', '555-890-1234', 'North Avenue 202', (SELECT groomer_id FROM groomers WHERE email = 'groomer1@petgrooming.com'));
END
GO

-- Insert breeds
IF NOT EXISTS (SELECT 1 FROM breeds WHERE name = 'Labrador Retriever')
BEGIN
    INSERT INTO breeds (name, species, coat_type, grooming_difficulty)
    VALUES
        ('Labrador Retriever', 'Dog', 'Short', 2),
        ('Golden Retriever', 'Dog', 'Long', 3),
        ('French Bulldog', 'Dog', 'Short', 1),
        ('Poodle', 'Dog', 'Curly', 4),
        ('German Shepherd', 'Dog', 'Medium', 3),
        ('Siamese', 'Cat', 'Short', 2),
        ('Persian', 'Cat', 'Long', 5),
        ('Maine Coon', 'Cat', 'Long', 4),
        ('Sphynx', 'Cat', 'Hairless', 1),
        ('Chihuahua', 'Dog', 'Short', 2);
END
GO

-- Pet categories are already inserted before the pets section
GO

-- Insert pets
-- First we verify that the necessary categories exist
IF NOT EXISTS (SELECT 1 FROM pet_categories WHERE name = 'Adult')
BEGIN
    INSERT INTO pet_categories (name, description)
    VALUES
        ('Adult', 'Pet older than 1 year');
END

IF NOT EXISTS (SELECT 1 FROM pet_categories WHERE name = 'Puppy')
BEGIN
    INSERT INTO pet_categories (name, description)
    VALUES
        ('Puppy', 'Young pet under 1 year');
END

IF NOT EXISTS (SELECT 1 FROM pet_categories WHERE name = 'Senior')
BEGIN
    INSERT INTO pet_categories (name, description)
    VALUES
        ('Senior', 'Pet older than 7 years');
END
GO

-- Now we insert the pets making sure the categories exist
IF NOT EXISTS (SELECT 1 FROM pets WHERE name = 'Max')
BEGIN
    -- We verify that the necessary clients and breeds exist
    IF EXISTS (SELECT 1 FROM customers WHERE email = 'customer1@example.com') AND 
       EXISTS (SELECT 1 FROM breeds WHERE name = 'Labrador Retriever') AND
       EXISTS (SELECT 1 FROM pet_categories WHERE name = 'Adult')
    BEGIN
        INSERT INTO pets (customer_id, name, breed_id, category_id, weight, date_of_birth, gender, allergies, notes)
        VALUES
            ((SELECT customer_id FROM customers WHERE email = 'customer1@example.com'), 'Max', 
             (SELECT breed_id FROM breeds WHERE name = 'Labrador Retriever'), 
             (SELECT category_id FROM pet_categories WHERE name = 'Adult'), 
             25.5, '2020-05-15', 'Male', NULL, 'Very friendly');
    END

    IF EXISTS (SELECT 1 FROM customers WHERE email = 'customer1@example.com') AND 
       EXISTS (SELECT 1 FROM breeds WHERE name = 'Siamés') AND
       EXISTS (SELECT 1 FROM pet_categories WHERE name = 'Adult')
    BEGIN
        INSERT INTO pets (customer_id, name, breed_id, category_id, weight, date_of_birth, gender, allergies, notes)
        VALUES
            ((SELECT customer_id FROM customers WHERE email = 'customer1@example.com'), 'Luna', 
             (SELECT breed_id FROM breeds WHERE name = 'Siamese'), 
             (SELECT category_id FROM pet_categories WHERE name = 'Adult'), 
             4.2, '2019-08-20', 'Female', 'Sensitivity to certain shampoos', 'Prefers gentle brushing');
    END

    IF EXISTS (SELECT 1 FROM customers WHERE email = 'customer2@example.com') AND 
       EXISTS (SELECT 1 FROM breeds WHERE name = 'Bulldog Francés') AND
       EXISTS (SELECT 1 FROM pet_categories WHERE name = 'Puppy')
    BEGIN
        INSERT INTO pets (customer_id, name, breed_id, category_id, weight, date_of_birth, gender, allergies, notes)
        VALUES
            ((SELECT customer_id FROM customers WHERE email = 'customer2@example.com'), 'Rocky', 
             (SELECT breed_id FROM breeds WHERE name = 'French Bulldog'), 
             (SELECT category_id FROM pet_categories WHERE name = 'Puppy'), 
             8.3, '2022-11-10', 'Male', NULL, 'Energetic');
    END

    IF EXISTS (SELECT 1 FROM customers WHERE email = 'customer3@example.com') AND 
       EXISTS (SELECT 1 FROM breeds WHERE name = 'Poodle') AND
       EXISTS (SELECT 1 FROM pet_categories WHERE name = 'Adult')
    BEGIN
        INSERT INTO pets (customer_id, name, breed_id, category_id, weight, date_of_birth, gender, allergies, notes)
        VALUES
            ((SELECT customer_id FROM customers WHERE email = 'customer3@example.com'), 'Coco', 
             (SELECT breed_id FROM breeds WHERE name = 'Poodle'), 
             (SELECT category_id FROM pet_categories WHERE name = 'Adult'), 
             12.7, '2018-03-25', 'Female', NULL, 'Needs special haircut');
    END

    IF EXISTS (SELECT 1 FROM customers WHERE email = 'customer3@example.com') AND 
       EXISTS (SELECT 1 FROM breeds WHERE name = 'Maine Coon') AND
       EXISTS (SELECT 1 FROM pet_categories WHERE name = 'Senior')
    BEGIN
        INSERT INTO pets (customer_id, name, breed_id, category_id, weight, date_of_birth, gender, allergies, notes)
        VALUES
            ((SELECT customer_id FROM customers WHERE email = 'customer3@example.com'), 'Simba', 
             (SELECT breed_id FROM breeds WHERE name = 'Maine Coon'), 
             (SELECT category_id FROM pet_categories WHERE name = 'Senior'), 
             8.9, '2015-06-12', 'Male', 'Flea allergy', 'Very tangled fur');
    END

    IF EXISTS (SELECT 1 FROM customers WHERE email = 'customer4@example.com') AND 
       EXISTS (SELECT 1 FROM breeds WHERE name = 'Golden Retriever') AND
       EXISTS (SELECT 1 FROM pet_categories WHERE name = 'Adult')
    BEGIN
        INSERT INTO pets (customer_id, name, breed_id, category_id, weight, date_of_birth, gender, allergies, notes)
        VALUES
            ((SELECT customer_id FROM customers WHERE email = 'customer4@example.com'), 'Bella', 
             (SELECT breed_id FROM breeds WHERE name = 'Golden Retriever'), 
             (SELECT category_id FROM pet_categories WHERE name = 'Adult'), 
             27.8, '2019-12-05', 'Female', NULL, 'Very docile');
    END

    IF EXISTS (SELECT 1 FROM customers WHERE email = 'customer5@example.com') AND 
       EXISTS (SELECT 1 FROM breeds WHERE name = 'Pastor Alemán') AND
       EXISTS (SELECT 1 FROM pet_categories WHERE name = 'Adult')
    BEGIN
        INSERT INTO pets (customer_id, name, breed_id, category_id, weight, date_of_birth, gender, allergies, notes)
        VALUES
            ((SELECT customer_id FROM customers WHERE email = 'customer5@example.com'), 'Thor', 
             (SELECT breed_id FROM breeds WHERE name = 'German Shepherd'), 
             (SELECT category_id FROM pet_categories WHERE name = 'Adult'), 
             32.1, '2020-02-18', 'Male', NULL, 'Needs frequent brushing');
    END

    IF EXISTS (SELECT 1 FROM customers WHERE email = 'customer5@example.com') AND 
       EXISTS (SELECT 1 FROM breeds WHERE name = 'Chihuahua') AND
       EXISTS (SELECT 1 FROM pet_categories WHERE name = 'Puppy')
    BEGIN
        INSERT INTO pets (customer_id, name, breed_id, category_id, weight, date_of_birth, gender, allergies, notes)
        VALUES
            ((SELECT customer_id FROM customers WHERE email = 'customer5@example.com'), 'Mia', 
             (SELECT breed_id FROM breeds WHERE name = 'Chihuahua'), 
             (SELECT category_id FROM pet_categories WHERE name = 'Puppy'), 
             2.1, '2022-09-30', 'Female', 'Skin sensitivity', 'Nervous with strangers');
    END
END
GO

-- Insert appointments
IF NOT EXISTS (SELECT 1 FROM appointments WHERE appointment_date = '2023-06-15')
BEGIN
    DECLARE @yesterday DATE = DATEADD(DAY, -1, GETDATE());
    DECLARE @today DATE = GETDATE();
    DECLARE @tomorrow DATE = DATEADD(DAY, 1, GETDATE());
    DECLARE @nextWeek DATE = DATEADD(DAY, 7, GETDATE());
    
    -- Past appointments - Max
    IF EXISTS (SELECT 1 FROM pets WHERE name = 'Max') AND 
       EXISTS (SELECT 1 FROM groomers WHERE email = 'groomer1@petgrooming.com') AND
       EXISTS (SELECT 1 FROM users WHERE username = 'customer1')
    BEGIN
        INSERT INTO appointments (pet_id, groomer_id, created_by_user_id, appointment_date, start_time, estimated_duration, status, notes)
        VALUES
            ((SELECT pet_id FROM pets WHERE name = 'Max'), 
             (SELECT groomer_id FROM groomers WHERE email = 'groomer1@petgrooming.com'), 
             (SELECT user_id FROM users WHERE username = 'customer1'), 
             @yesterday, '10:00', 60, 'completed', 'Standard haircut and bath');
    END

    -- Past appointments - Rocky
    IF EXISTS (SELECT 1 FROM pets WHERE name = 'Rocky') AND 
       EXISTS (SELECT 1 FROM groomers WHERE email = 'groomer3@petgrooming.com') AND
       EXISTS (SELECT 1 FROM users WHERE username = 'customer2')
    BEGIN
        INSERT INTO appointments (pet_id, groomer_id, created_by_user_id, appointment_date, start_time, estimated_duration, status, notes)
        VALUES
            ((SELECT pet_id FROM pets WHERE name = 'Rocky'), 
             (SELECT groomer_id FROM groomers WHERE email = 'groomer3@petgrooming.com'), 
             (SELECT user_id FROM users WHERE username = 'customer2'), 
             @yesterday, '14:30', 45, 'completed', 'Bath only');
    END
    
    -- Today's appointments - Coco
    IF EXISTS (SELECT 1 FROM pets WHERE name = 'Coco') AND 
       EXISTS (SELECT 1 FROM groomers WHERE email = 'groomer2@petgrooming.com') AND
       EXISTS (SELECT 1 FROM users WHERE username = 'customer3')
    BEGIN
        INSERT INTO appointments (pet_id, groomer_id, created_by_user_id, appointment_date, start_time, estimated_duration, status, notes)
        VALUES
            ((SELECT pet_id FROM pets WHERE name = 'Coco'), 
             (SELECT groomer_id FROM groomers WHERE email = 'groomer2@petgrooming.com'), 
             (SELECT user_id FROM users WHERE username = 'customer3'), 
             @today, '09:00', 90, 'approved', 'Special haircut for exhibition');
    END

    -- Today's appointments - Bella
    IF EXISTS (SELECT 1 FROM pets WHERE name = 'Bella') AND 
       EXISTS (SELECT 1 FROM groomers WHERE email = 'groomer1@petgrooming.com') AND
       EXISTS (SELECT 1 FROM users WHERE username = 'customer4')
    BEGIN
        INSERT INTO appointments (pet_id, groomer_id, created_by_user_id, appointment_date, start_time, estimated_duration, status, notes)
        VALUES
            ((SELECT pet_id FROM pets WHERE name = 'Bella'), 
             (SELECT groomer_id FROM groomers WHERE email = 'groomer1@petgrooming.com'), 
             (SELECT user_id FROM users WHERE username = 'customer4'), 
             @today, '11:30', 60, 'approved', 'Bath and deep brushing');
    END

    -- Today's appointments - Simba
    IF EXISTS (SELECT 1 FROM pets WHERE name = 'Simba') AND 
       EXISTS (SELECT 1 FROM groomers WHERE email = 'groomer2@petgrooming.com') AND
       EXISTS (SELECT 1 FROM users WHERE username = 'customer3')
    BEGIN
        INSERT INTO appointments (pet_id, groomer_id, created_by_user_id, appointment_date, start_time, estimated_duration, status, notes)
        VALUES
            ((SELECT pet_id FROM pets WHERE name = 'Simba'), 
             (SELECT groomer_id FROM groomers WHERE email = 'groomer2@petgrooming.com'), 
             (SELECT user_id FROM users WHERE username = 'customer3'), 
             @today, '15:00', 75, 'cancelled', 'Client canceled due to illness');
    END
    
    -- Future appointments - Luna
    IF EXISTS (SELECT 1 FROM pets WHERE name = 'Luna') AND 
       EXISTS (SELECT 1 FROM groomers WHERE email = 'groomer2@petgrooming.com') AND
       EXISTS (SELECT 1 FROM users WHERE username = 'customer1')
    BEGIN
        INSERT INTO appointments (pet_id, groomer_id, created_by_user_id, appointment_date, start_time, estimated_duration, status, notes)
        VALUES
            ((SELECT pet_id FROM pets WHERE name = 'Luna'), 
             (SELECT groomer_id FROM groomers WHERE email = 'groomer2@petgrooming.com'), 
             (SELECT user_id FROM users WHERE username = 'customer1'), 
             @tomorrow, '10:00', 45, 'pending', 'Brushing and ear cleaning');
    END

    -- Future appointments - Thor
    IF EXISTS (SELECT 1 FROM pets WHERE name = 'Thor') AND 
       EXISTS (SELECT 1 FROM groomers WHERE email = 'groomer3@petgrooming.com') AND
       EXISTS (SELECT 1 FROM users WHERE username = 'customer5')
    BEGIN
        INSERT INTO appointments (pet_id, groomer_id, created_by_user_id, appointment_date, start_time, estimated_duration, status, notes)
        VALUES
            ((SELECT pet_id FROM pets WHERE name = 'Thor'), 
             (SELECT groomer_id FROM groomers WHERE email = 'groomer3@petgrooming.com'), 
             (SELECT user_id FROM users WHERE username = 'customer5'), 
             @tomorrow, '13:00', 75, 'approved', 'Complete bath and nail trimming');
    END

    -- Future appointments - Mia
    IF EXISTS (SELECT 1 FROM pets WHERE name = 'Mia') AND 
       EXISTS (SELECT 1 FROM groomers WHERE email = 'groomer1@petgrooming.com') AND
       EXISTS (SELECT 1 FROM users WHERE username = 'customer5')
    BEGIN
        INSERT INTO appointments (pet_id, groomer_id, created_by_user_id, appointment_date, start_time, estimated_duration, status, notes)
        VALUES
            ((SELECT pet_id FROM pets WHERE name = 'Mia'), 
             (SELECT groomer_id FROM groomers WHERE email = 'groomer1@petgrooming.com'), 
             (SELECT user_id FROM users WHERE username = 'customer5'), 
             @nextWeek, '09:30', 30, 'pending', 'Nail trimming and dental cleaning');
    END
END
GO

-- Notifications are automatically generated by the trg_create_appointment_notification trigger
-- It is not necessary to manually insert records

-- Insert additional sessions
IF NOT EXISTS (SELECT 1 FROM sessions WHERE user_agent LIKE '%Chrome%')
BEGIN
    INSERT INTO sessions (user_id, token, ip_address, user_agent, expires_at)
    VALUES
        ((SELECT user_id FROM users WHERE username = 'groomer1'), 'token_groomer1_123456', '192.168.1.10', 'Mozilla/5.0 (Windows NT 10.0; Win64; x64) Chrome/91.0.4472.124', DATEADD(DAY, 1, GETDATE())),
        ((SELECT user_id FROM users WHERE username = 'customer1'), 'token_customer1_654321', '192.168.1.20', 'Mozilla/5.0 (iPhone; CPU iPhone OS 14_6) AppleWebKit/605.1.15 Safari/604.1', DATEADD(DAY, 1, GETDATE())),
        ((SELECT user_id FROM users WHERE username = 'customer3'), 'token_customer3_789012', '192.168.1.30', 'Mozilla/5.0 (Macintosh; Intel Mac OS X 10_15_7) Safari/605.1.15', DATEADD(DAY, 1, GETDATE()));
END
GO

SELECT 'Test data inserted successfully' AS [Message];
GO