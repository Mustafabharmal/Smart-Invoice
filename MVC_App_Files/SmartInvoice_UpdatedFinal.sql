SELECT * FROM Tcustomer

SELECT * FROM Tcategory

SELECT * FROM Tproduct

SELECT * FROM Tquotation

SELECT * FROM Tsale

SELECT * FROM Tstore

SELECT * FROM Ttransfer

SELECT * FROM Tuser




-- Create the 'add_user' table
CREATE TABLE Tuser (
    user_id INT IDENTITY(1,1) PRIMARY KEY,
    first_name NVARCHAR(255),
    last_name NVARCHAR(255),
    username NVARCHAR(255),
    pwd NVARCHAR(255),
    phone NVARCHAR(255),
    email NVARCHAR(255),
    role INT,
    avatar VARBINARY(MAX),
	status INT,
	created_at DATETIME DEFAULT GETDATE(),
    updated_at DATETIME DEFAULT GETDATE(),

);

alter table Tuser
add store_id INT,
FOREIGN KEY (store_id) REFERENCES Tstore(store_id)
	

-- Create the 'add_store' table
CREATE TABLE Tstore (
    store_id INT IDENTITY(1,1) PRIMARY KEY,
    store_name NVARCHAR(255),
    username NVARCHAR(50),
    pwd NVARCHAR(50),
    phone NVARCHAR(20),
    email NVARCHAR(255),
    image VARBINARY(MAX),
	user_id INT,
	FOREIGN KEY (user_id) REFERENCES Tuser(user_id),
	created_at DATETIME DEFAULT GETDATE(),
    updated_at DATETIME DEFAULT GETDATE()
);


-- Create the 'add_category' table
CREATE TABLE Tcategory (
	category_id INT IDENTITY(1,1) PRIMARY KEY,
    product_name NVARCHAR(255),
    code NVARCHAR(50),
    description NVARCHAR(MAX),
    image VARBINARY(MAX),
	--store id
	store_id INT,
    FOREIGN KEY (store_id) REFERENCES Tstore(store_id),
	created_at DATETIME DEFAULT GETDATE(),
	updated_at DATETIME DEFAULT GETDATE()
	
);

-- Create the 'add_sale' table
CREATE TABLE Tsale (
    sale_id INT IDENTITY(1,1) PRIMARY KEY,
    sale_date DATETIME,
    customer_name NVARCHAR(255),
    product_name NVARCHAR(255),
    quantity NVARCHAR(255),
    price NVARCHAR(255),
    discount NVARCHAR(255),
    tax NVARCHAR(255),
    subtotal NVARCHAR(255),
	paid_with INT,
	store_id INT,
    FOREIGN KEY (store_id) REFERENCES Tstore(store_id),
	user_id INT,
	FOREIGN KEY (user_id) REFERENCES Tuser(user_id),
	created_at DATETIME DEFAULT GETDATE(),
    updated_at DATETIME DEFAULT GETDATE()
   
);


-- Create the 'add_customer' table
CREATE TABLE Tcustomer (
    customer_id INT IDENTITY(1,1) PRIMARY KEY,
    name NVARCHAR(255),
    email NVARCHAR(255),
    phone NVARCHAR(20),
    description NVARCHAR(MAX),
    --avatar VARBINARY(MAX),
	store_id INT,
    FOREIGN KEY (store_id) REFERENCES Tstore(store_id),
	user_id INT,
	FOREIGN KEY (user_id) REFERENCES Tuser(user_id),
	created_at DATETIME DEFAULT GETDATE(),
    updated_at DATETIME DEFAULT GETDATE()
);

-- Create the 'add_product' table
CREATE TABLE Tproduct (
    product_id INT IDENTITY(1,1) PRIMARY KEY,
    product_name NVARCHAR(255),
    category NVARCHAR(255),
    min_quantity INT,
    quantity INT,
    description NVARCHAR(MAX),
    price NVARCHAR(255),
    status int,
    image VARBINARY(MAX),
	store_id INT,
    FOREIGN KEY (store_id) REFERENCES Tstore(store_id),
	user_id INT,
	FOREIGN KEY (user_id) REFERENCES Tuser(user_id),
	created_at DATETIME DEFAULT GETDATE(),
    updated_at DATETIME DEFAULT GETDATE()
	
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
	store_id INT,
    FOREIGN KEY (store_id) REFERENCES Tstore(store_id),
	user_id INT,
	FOREIGN KEY (user_id) REFERENCES Tuser(user_id),
	created_at DATETIME DEFAULT GETDATE(),
    updated_at DATETIME DEFAULT GETDATE()
);


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
    status INT,
	store_id INT,
    FOREIGN KEY (store_id) REFERENCES Tstore(store_id),
	user_id INT,
	FOREIGN KEY (user_id) REFERENCES Tuser(user_id),
	created_at DATETIME DEFAULT GETDATE(),
    updated_at DATETIME DEFAULT GETDATE()
);
