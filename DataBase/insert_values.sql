INSERT INTO Customer (name, email, phone_number, password) VALUES
('Alice Johansson', 'alice@mail.com', '0701234567', 'hashedpassword1'),
('Bob Karlsson', 'bob@mail.com', '0707654321', 'hashedpassword2'),
('Charlie Svensson', 'charlie@mail.com', '0709988776', 'hashedpassword3');



INSERT INTO Staff (name, role) VALUES
('Emma Andersson', 'Hairdresser'),
('Johan Eriksson', 'Barber'),
('Sara Nilsson', 'Stylist');



INSERT INTO Service (name, price, description, duration) VALUES
('Haircut', 299.00, 'Basic haircut and styling', 30),
('Beard Trim', 199.00, 'Trimming and shaping of beard', 20),
('Hair Coloring', 899.00, 'Full hair coloring service', 90);



INSERT INTO Booking (customerID, staffID, serviceID, date_time, status) VALUES
(1, 1, 1, '2025-03-05 10:00:00', 'Booked'),
(2, 2, 2, '2025-03-06 15:00:00', 'Booked'),
(3, 3, 3, '2025-03-07 12:30:00', 'Canceled');



INSERT INTO Payment (bookingID, amount, payment_method, status) VALUES
(1, 299.00, 'Card', 'Paid'),
(2, 199.00, 'Swish', 'Paid'),
(3, 899.00, 'Cash', 'Unpaid');



INSERT INTO Notification (bookingID, type, sent_time) VALUES
(1, 'Confirmation', '2025-03-04 18:00:00'),
(2, 'Reminder', '2025-03-05 19:00:00'),
(3, 'Confirmation', '2025-03-06 10:00:00');


