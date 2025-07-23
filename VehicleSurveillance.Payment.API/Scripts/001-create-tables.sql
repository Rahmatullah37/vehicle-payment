-- Enable UUID extension
CREATE EXTENSION IF NOT EXISTS "uuid-ossp";

-- Packages table
CREATE TABLE IF NOT EXISTS Packages (
    id UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    package_type VARCHAR(12) NOT NULL,
    package_cost DECIMAL(12,2) NOT NULL,
    is_active BOOLEAN NOT NULL DEFAULT true,
    created_date TIMESTAMPTZ NOT NULL DEFAULT now(),
    created_by VARCHAR(50) NOT NULL,
    updated_date TIMESTAMPTZ NOT NULL DEFAULT now(),
    updated_by VARCHAR(50) NOT NULL
);

-- AccessFeeTransaction table
CREATE TABLE IF NOT EXISTS AccessFeeTransaction (
    id UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    vehicle_id UUID,
    amountcharged DECIMAL(12,2) NOT NULL,
    vehicle_category VARCHAR(50) NOT NULL,
    payment_mode VARCHAR(20) NOT NULL
    is_active BOOLEAN NOT NULL DEFAULT true,
    created_date TIMESTAMPTZ NOT NULL DEFAULT now(),
    updated_date TIMESTAMPTZ NOT NULL DEFAULT now(),
    created_by VARCHAR(50) NOT NULL,
    updated_by VARCHAR(50) NOT NULL,
    category_id UUID
);

-- PaymentModes table
CREATE TABLE IF NOT EXISTS PaymentModes (
    id UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    name VARCHAR(50) NOT NULL
);

-- Tarif table
CREATE TABLE IF NOT EXISTS Tarif (
    id UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    vehicle_type_id UUID NOT NULL,
    tarif_type_id UUID NOT NULL,
    is_active BOOLEAN NOT NULL DEFAULT true,
    description TEXT,
    created_date TIMESTAMPTZ NOT NULL DEFAULT now(),
    updated_date TIMESTAMPTZ NOT NULL DEFAULT now(),
    created_by VARCHAR(50) NOT NULL,
    updated_by VARCHAR(50) NOT NULL
);

-- FixedTarif table
CREATE TABLE IF NOT EXISTS FixedTarif (
    id UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    amount DECIMAL(12,2) NOT NULL,
    tarif_id UUID NOT NULL,
    is_active BOOLEAN NOT NULL DEFAULT true,
    created_date TIMESTAMPTZ NOT NULL DEFAULT now(),
    updated_date TIMESTAMPTZ NOT NULL DEFAULT now(),
    created_by VARCHAR(50) NOT NULL,
    updated_by VARCHAR(50) NOT NULL,
    FOREIGN KEY (tarif_id) REFERENCES Tarif(id)
);
-- Create the TarifType table
CREATE TABLE IF NOT EXISTS TarifType (
    id UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    name VARCHAR(50) NOT NULL,
    is_active BOOLEAN NOT NULL DEFAULT TRUE,
    description TEXT,
    created_date TIMESTAMPTZ NOT NULL DEFAULT now(),
    updated_date TIMESTAMPTZ NOT NULL DEFAULT now(),
    created_by VARCHAR(50) NOT NULL,
    updated_by VARCHAR(50) NOT NULL
);

-- Create the VehicleType table
CREATE TABLE IF NOT EXISTS VehicleType (
    id UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    name VARCHAR(50) NOT NULL,
    is_active BOOLEAN NOT NULL DEFAULT TRUE,
    description TEXT,
    created_date TIMESTAMPTZ NOT NULL DEFAULT now(),
    updated_date TIMESTAMPTZ NOT NULL DEFAULT now(),
    created_by VARCHAR(50) NOT NULL,
    updated_by VARCHAR(50) NOT NULL
);
