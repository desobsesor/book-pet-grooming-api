# Database Structure for Pet Grooming System

## Business Model Description

This system allows managing reservations for pet grooming services. The workflow is as follows:

1. A customer requests an appointment for their pet's grooming service
2. The groomer reviews the request and approves or rejects it
3. The customer receives a notification about the status of their reservation

## Main Entities

### Customers
- Can have multiple pets
- Can have a preferred groomer
- Receive notifications about the status of their reservations

### Pets
- Belong to a customer
- Have a specific breed
- Are classified as puppy or adult
- Have a registered weight
- Service time and price depend on their breed, category, and weight

### Groomers
- Review and manage reservation requests
- May have specialties or experience with certain breeds

### Appointments
- Contain information about date, time, and estimated duration
- Have a status (pending, approved, rejected, completed, canceled)
- Price is automatically calculated based on the pet's breed, category, and weight

## Relationship Diagram

```
Customers 1 --- N Pets
Customers N --- 1 Groomers (preferred)
Pets N --- 1 Breeds
Pets N --- 1 PetCategories
Pets 1 --- N Appointments
Appointments N --- 1 Groomers
Appointments 1 --- N Notifications
```

## Technical Features

- Automatic price calculation based on the pet's breed, category, and weight
- Automatic notification system when the status of a reservation changes
- Consolidated view to see all the details of a reservation
- Indexes to optimize the performance of frequent queries

## Usage

To implement this database structure, run the `schema.sql` file in your PostgreSQL instance:

```bash
psql -U username -d database_name -f schema.sql
```

Where:
- `username` is your PostgreSQL username
- `database_name` is the name of the database where you want to implement the schema