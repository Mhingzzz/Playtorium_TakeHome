-- Drop tables in order due to FK constraints
DROP TABLE IF EXISTS applied_discounts;
DROP TABLE IF EXISTS cart_items;
DROP TABLE IF EXISTS cart;
DROP TABLE IF EXISTS discount_campaigns;
DROP TABLE IF EXISTS items;
DROP TABLE IF EXISTS customers;

-- Customers
CREATE TABLE customers (
    id SERIAL PRIMARY KEY,
    name VARCHAR(255),
    email VARCHAR(255) UNIQUE,
    points INT DEFAULT 0
);

INSERT INTO customers (name, email, points) VALUES
('Alice', 'alice@example.com', 100),
('Bob', 'bob@example.com', 50);

-- Items
CREATE TABLE items (
    id SERIAL PRIMARY KEY,
    name VARCHAR(255),
    category VARCHAR(255),
    price DECIMAL(10, 2) DEFAULT 0 
);

INSERT INTO items (name, category, price) VALUES
('T-Shirt', 'Clothing', 350.00),
('Hat', 'Accessories', 250.00),
('Hoodie', 'Clothing', 700.00),
('Watch', 'Electronics', 850.00),
('Bag', 'Accessories', 640.00),
('Belt', 'Accessories', 230.00);

-- Discount Campaigns
CREATE TABLE discount_campaigns (
    id SERIAL PRIMARY KEY,
    campaign_type VARCHAR(255),
    discount_value DECIMAL(10, 2),
    category VARCHAR(255),
	item_category VARCHAR(255),
    points_cap DECIMAL(10, 2),
    every_x_thb DECIMAL(10, 2),
    discount_y_thb DECIMAL(10, 2),
    is_active BOOLEAN DEFAULT TRUE
);

-- Insert mock campaigns
INSERT INTO discount_campaigns (campaign_type, discount_value, item_category, points_cap, every_x_thb, discount_y_thb, is_active , category) VALUES
('FixedAmount', 50.00, NULL, NULL, NULL, NULL, TRUE , 'Coupon'),
('Percentage', 10.00, NULL, NULL, NULL, NULL, TRUE, 'Coupon'),
('PercentageByItemCategory', 15.00, 'Clothing', NULL, NULL, NULL, TRUE, 'OnTop'),
('DiscountByPoints', 0.00, NULL, 0.20, NULL, NULL, TRUE, 'OnTop'),
(' SpecialCampaign', 0.00, NULL, NULL, 300.00, 40.00, TRUE , 'Seasonal');

-- Cart
CREATE TABLE cart (
    id SERIAL PRIMARY KEY,
    customer_id INT NOT NULL,
    total_price DECIMAL(10, 2) DEFAULT 0,
    status VARCHAR(255),
    FOREIGN KEY (customer_id) REFERENCES customers(id)
);


-- Cart Items
CREATE TABLE cart_items (
    id SERIAL PRIMARY KEY,
    cart_id INT NOT NULL,
    item_id INT NOT NULL,
    quantity INT DEFAULT 1,
    FOREIGN KEY (cart_id) REFERENCES cart(id),
    FOREIGN KEY (item_id) REFERENCES items(id)
);



-- Applied Discounts
CREATE TABLE applied_discounts (
    id SERIAL PRIMARY KEY,
    cart_id INT NOT NULL,
    campaign_id INT NOT NULL,
    discount_amount DECIMAL(10, 2),
    discount_type VARCHAR(255),
    applied_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (cart_id) REFERENCES cart(id),
    FOREIGN KEY (campaign_id) REFERENCES discount_campaigns(id)
);

