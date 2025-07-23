

-- Insert sample data into PaymentModes
INSERT INTO PaymentModes (name)
VALUES 
    ('cash');

-- Insert sample data into Packages
INSERT INTO Packages (package_type, package_cost, is_active, created_date, created_by, updated_date, updated_by)
VALUES 
    ('Daily', 100.00, true, now(), 'System', now(), 'System');

-- Insert sample data into Tarif
-- Ensure you use valid UUIDs
INSERT INTO Tarif (vehicle_type_id, tarif_type_id, is_active, description, created_date, updated_date, created_by, updated_by)
VALUES
(
    
    '3fa85f64-5717-4562-b3fc-2c963f66afa6',
    '3fa85f64-5717-4562-b3fc-2c963f66afa6',
    true,
    'Standard car tarif',
    now(),
    now(),
    'System',
    'System'
);

-- Insert sample data into FixedTarif
INSERT INTO FixedTarif (amount, tarif_id, is_active, created_date, updated_date, created_by, updated_by)
VALUES 
(
    250.00,
    'fa9b5846-c477-4d30-a836-68315b44d541',  
    true,
    now(),
    now(),
    'System',
    'System'
);

-- Insert sample data into AccessFeeTransaction
INSERT INTO AccessFeeTransaction (vehicle_id, amountcharged, vehicle_category, payment_mode, is_active, created_date, updated_date, created_by, updated_by)
VALUES 
(
    '3fa85f64-5717-4562-b3fc-2c963f66afa6',
    150.00,
    'SUV',
    'cash',
    true,
    now(),
    now(),
    'System',
    'System'
);
