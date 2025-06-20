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
To use this database structure, you need to have SQL Server installed on your computer. You can download it from the official website

To implement this database structure, run the `schema-sqlserver.sql` file in your SQL Server instance:
```sql
sqlcmd -S server_name -d database_name -i schema-sqlserver.sql
```
To insert sample data, run the `sample-data.sql` file:
```sql
sqlcmd -S server_name -d database_name -i sample-data.sql
```
Where:
- `server_name` is your SQL Server instance name
- `database_name` is the name of your database