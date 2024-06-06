use NET1814_212_4_JewelryStore
INSERT INTO Category (CategoryID, CategoryName) VALUES
(1, 'Rings'),
(2, 'Necklaces'),
(3, 'Bracelets'),
(4, 'Earrings'),
(5, 'Other Jewelry');

INSERT INTO OrderItem (OrderItemID, OrderID, ProductID, Quantity, Price, Subtotal) VALUES
(1, 1001, 101, 2, 500, 1000),
(2, 1001, 102, 1, 300, 300),
(3, 1002, 103, 3, 200, 600),
(4, 1003, 104, 1, 1000, 1000),
(5, 1004, 105, 2, 750, 1500);

INSERT INTO Promotion (PromotionID, Name, Description, StartDate, EndDate, DiscountPercentage, CompanyID) VALUES
(1, 'Summer Sale', 'Seasonal Discount', '2024-06-01', '2024-08-31', '10%', 1),
(2, 'New Year Offer', 'Special New Year Offer', '2024-12-25', '2025-01-07', '15%', 2),
(3, 'Anniversary Promo', 'Company Anniversary Promo', '2024-09-01', '2024-09-30', '20%', 3),
(4, 'Loyalty Discount', 'For Loyal Customers', '2024-01-01', '2024-12-31', '5%', 1),
(5, 'Clearance Sale', 'End of Season Sale', '2024-11-01', '2024-11-30', '25%', 2);

INSERT INTO SI_Company (CompanyID, CompanyName, CompanyAddress) VALUES
(1, 'Jewel Masterpieces Inc.', '123 Main St, City A'),
(2, 'Glittering Gems Co.', '456 Oak Ave, City B'),
(3, 'Radiant Diamonds Ltd.', '789 Elm Rd, City C'),
(4, 'Golden Treasures Corp.', '321 Pine Blvd, City D'),
(5, 'Sparkling Jewels LLC', '654 Maple St, City E');

INSERT INTO SI_Customer (CustomerID, Name, Phone, Address) VALUES
(1, 'John Doe', '123-456-7890', '100 Oak St, City A'),
(2, 'Jane Smith', '987-654-3210', '200 Elm Rd, City B'),
(3, 'Michael Johnson', '456-789-0123', '300 Pine Ave, City C'),
(4, 'Emily Davis', '789-012-3456', '400 Maple Blvd, City D'),
(5, 'David Wilson', '012-345-6789', '500 Oak Dr, City E');

INSERT INTO SI_Order (OrderID, CustomerID, PromotionID, OrderDate, TotalAmount, Discount, PaymentMethod, PaymentStatus, ShipmentStatus) VALUES
(1001, 1, 1, '2024-07-15', 1300, 130, 'Credit Card', 'Paid', 'Shipped'),
(1002, 2, NULL, '2024-05-20', 600, 0, 'Cash', 'Paid', 'Delivered'),
(1003, 3, 2, '2025-01-03', 1000, 150, 'Online Payment', 'Paid', 'Pending'),
(1004, 4, 3, '2024-09-15', 1500, 300, 'Credit Card', 'Paid', 'Shipped'),
(1005, 5, NULL, '2024-11-25', 2000, 0, 'Cash', 'Unpaid', 'Pending');

INSERT INTO SI_Product (ProductID, Name, CategoryID, Description, Barcode, Weight, CostPrice, GoldPrice, LaborCost, StoneCost, SellPriceRatio) VALUES
(101, 'Diamond Ring', 1, 'Elegant Diamond Ring', '123456789012', 5.2, 500, 300, 100, 200, 1.5),
(102, 'Pearl Necklace', 2, 'Luxurious Pearl Necklace', '987654321098', 10.0, 200, 100, 50, 100, 1.3),
(103, 'Ruby Bracelet', 3, 'Stunning Ruby Bracelet', '456789012345', 20.5, 300, 150, 100, 200, 1.4),
(104, 'Emerald Earrings', 4, 'Exquisite Emerald Earrings', '789012345678', 3.0, 800, 500, 200, 300, 1.6),
(105, 'Sapphire Pendant', 5, 'Beautiful Sapphire Pendant', '012345678901', 8.0, 600, 400, 150, 250, 1.7);