-- Create the 'add_category' table
CREATE TABLE Tcategory (
    product_name NVARCHAR(255),
    code NVARCHAR(50),
    description NVARCHAR(MAX),
    image VARBINARY(MAX),
	created_at DATETIME DEFAULT GETDATE()
);

select * from Tcategory


-- Create the 'add_sale' table
CREATE TABLE Tsale (
    sale_id INT IDENTITY(1,1) PRIMARY KEY,
    sale_date DATETIME,
    customer_name NVARCHAR(255),
    supplier_name NVARCHAR(255),
    product_name NVARCHAR(255),
    quantity INT,
    price DECIMAL(18, 2),
    discount DECIMAL(18, 2),
    tax DECIMAL(18, 2),
    subtotal DECIMAL(18, 2),
	created_at DATETIME DEFAULT GETDATE()
);

-- Create the 'add_brand' table
CREATE TABLE Tbrand (
    brand_name NVARCHAR(255),
    description NVARCHAR(MAX),
    photo VARBINARY(MAX),
	created_at DATETIME DEFAULT GETDATE()
);

-- Create the 'add_customer' table
CREATE TABLE Tcustomer (
    customer_id INT IDENTITY(1,1) PRIMARY KEY,
    name NVARCHAR(255),
    email NVARCHAR(255),
    phone NVARCHAR(20),
    country NVARCHAR(255),
    city NVARCHAR(255),
    address NVARCHAR(MAX),
    description NVARCHAR(MAX),
    avatar VARBINARY(MAX),
	created_at DATETIME DEFAULT GETDATE()
);

-- Create the 'add_product' table
CREATE TABLE Tproduct (
    product_id INT IDENTITY(1,1) PRIMARY KEY,
    product_name NVARCHAR(255),
    category NVARCHAR(255),
    sub_category NVARCHAR(255),
    brand NVARCHAR(255),
    unit NVARCHAR(50),
    sku NVARCHAR(50), --stock keeping units
    min_quantity INT,
    quantity INT,
    description NVARCHAR(MAX),
    tax DECIMAL(18, 2),
    discount_type NVARCHAR(50),
    price DECIMAL(18, 2),
    status NVARCHAR(50),
    image VARBINARY(MAX),
	created_at DATETIME DEFAULT GETDATE()
);

-- Create the 'add_purchase' table
CREATE TABLE Tpurchase (
    purchase_id INT IDENTITY(1,1) PRIMARY KEY,
    supplier_name NVARCHAR(255),
    purchase_date DATETIME,
    product_name NVARCHAR(255),
    ref_no NVARCHAR(50),
	description NVARCHAR(MAX),
    tax DECIMAL(18, 2),
    discount_type NVARCHAR(50),
    price DECIMAL(18, 2),
    status NVARCHAR(50),
	created_at DATETIME DEFAULT GETDATE()
);

-- Create the 'add_quotation' table
CREATE TABLE Tquotation (
    quotation_id INT IDENTITY(1,1) PRIMARY KEY,
    customer_name NVARCHAR(255),
    quotation_date DATETIME,
    ref_no NVARCHAR(50),
    product_name NVARCHAR(255),
	description NVARCHAR(MAX),
    tax DECIMAL(18, 2),
    discount_type NVARCHAR(50),
    price DECIMAL(18, 2),
    status NVARCHAR(50),
	created_at DATETIME DEFAULT GETDATE()
);

-- Create the 'add_store' table
CREATE TABLE Tstore (
    store_id INT IDENTITY(1,1) PRIMARY KEY,
    store_name NVARCHAR(255),
    username NVARCHAR(50),
    pwd NVARCHAR(50),
    phone NVARCHAR(20),
    email NVARCHAR(255),
    image VARBINARY(MAX),
	created_at DATETIME DEFAULT GETDATE()
);

-- Create the 'add_supplier' table
CREATE TABLE Tsupplier (
    supplier_id INT IDENTITY(1,1) PRIMARY KEY,
    supplier_name NVARCHAR(255),
    email NVARCHAR(255),
    phone NVARCHAR(20),
    country NVARCHAR(255),
    city NVARCHAR(255),
    address NVARCHAR(MAX),
    description NVARCHAR(MAX),
    avatar VARBINARY(MAX),
	created_at DATETIME DEFAULT GETDATE()
);
INSERT INTO Tsupplier (supplier_name, email, phone, country, city, address, description, avatar)
VALUES
    ('Supplier1', 'supplier1@email.com', '123456789', 'Country1', 'City1', 'Address1', 'Description1', 0x);

select * from Tsupplier
-- Create the 'add_transfer' table
CREATE TABLE Ttransfer (
    transfer_id INT IDENTITY(1,1) PRIMARY KEY,
    transfer_date DATETIME,
    product_name NVARCHAR(255),
    from_location NVARCHAR(255),
    to_location NVARCHAR(255),
	description NVARCHAR(MAX),
    tax DECIMAL(18, 2),
    discount_type NVARCHAR(50),
    price DECIMAL(18, 2),
    status NVARCHAR(50),
	created_at DATETIME DEFAULT GETDATE()
);

-- Create the 'add_user' table
CREATE TABLE Tuser (
    user_id INT IDENTITY(1,1) PRIMARY KEY,
    first_name NVARCHAR(50),
    last_name NVARCHAR(50),
    username NVARCHAR(50),
    pwd NVARCHAR(50),
    phone NVARCHAR(20),
    email NVARCHAR(255),
    role NVARCHAR(50),
    avatar VARBINARY(MAX),
	created_at DATETIME DEFAULT GETDATE()
);
