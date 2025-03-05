-- Skapar tabellen för kunder
CREATE TABLE Customer (
    customerID INT PRIMARY KEY IDENTITY(1,1), -- Auto-increment för SQL Server
    name VARCHAR(100),
    email VARCHAR(100) UNIQUE,
    phone_number VARCHAR(20),
    password VARCHAR(100)
);

-- Skapar tabellen för personal
CREATE TABLE Staff (
    staffID INT PRIMARY KEY IDENTITY(1,1),
    name VARCHAR(100),
    role VARCHAR(50)
);

-- Skapar tabellen för tjänster
CREATE TABLE Service (
    serviceID INT PRIMARY KEY IDENTITY(1,1),
    name VARCHAR(100),
    price DECIMAL(10,2),
    description TEXT,
    duration INT -- Tid i minuter
);

-- Skapar tabellen för bokningar
CREATE TABLE Booking (
    bookingID INT PRIMARY KEY IDENTITY(1,1),
    customerID INT,
    staffID INT,
    serviceID INT,
    date_time DATETIME,
    status VARCHAR(20) CHECK (status IN ('Booked', 'Canceled', 'Completed')),
    FOREIGN KEY (customerID) REFERENCES Customer(customerID) ON DELETE CASCADE,
    FOREIGN KEY (staffID) REFERENCES Staff(staffID) ON DELETE SET NULL,
    FOREIGN KEY (serviceID) REFERENCES Service(serviceID) ON DELETE CASCADE
);

-- Skapar tabellen för betalningar
CREATE TABLE Payment (
    paymentID INT PRIMARY KEY IDENTITY(1,1),
    bookingID INT,
    amount DECIMAL(10,2),
    payment_method VARCHAR(50) CHECK (payment_method IN ('Card', 'Swish', 'Cash')),
    status VARCHAR(50) CHECK (status IN ('Paid', 'Unpaid')),
    FOREIGN KEY (bookingID) REFERENCES Booking(bookingID) ON DELETE CASCADE
);

-- Skapar tabellen för notifikationer
CREATE TABLE Notification (
    notificationID INT PRIMARY KEY IDENTITY(1,1),
    bookingID INT,
    type VARCHAR(50) CHECK (type IN ('Confirmation', 'Reminder')),
    sent_time DATETIME,
    FOREIGN KEY (bookingID) REFERENCES Booking(bookingID) ON DELETE CASCADE
);
