-- ============================================================
-- CustomerPortal database schema + seed data
-- Reconstructed from the PHP endpoints in /CustomerPortal
-- Target: MySQL / MariaDB (MAMP), user root / pass root
-- Import with:  mysql -u root -proot < schema.sql
-- (or paste into phpMyAdmin > SQL tab)
-- ============================================================

CREATE DATABASE IF NOT EXISTS CustomerPortal
  CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;
USE CustomerPortal;

-- Drop in FK-safe order so the file is re-runnable
DROP TABLE IF EXISTS order_items;
DROP TABLE IF EXISTS orders;
DROP TABLE IF EXISTS cart_items;
DROP TABLE IF EXISTS addresses;
DROP TABLE IF EXISTS discount_codes;
DROP TABLE IF EXISTS products;
DROP TABLE IF EXISTS users;

-- ---------- users (registerUser.php / loginUser.php) ----------
CREATE TABLE users (
  user_id   INT AUTO_INCREMENT PRIMARY KEY,
  email     VARCHAR(255) NOT NULL UNIQUE,
  username  VARCHAR(100) NOT NULL UNIQUE,
  password  VARCHAR(255) NOT NULL          -- bcrypt hash from password_hash()
);

-- ---------- addresses (addAddress / getAddresses / deleteAddress) ----------
CREATE TABLE addresses (
  address_id   INT AUTO_INCREMENT PRIMARY KEY,
  user_id      INT NOT NULL,
  address_name VARCHAR(100) NOT NULL,
  street       VARCHAR(255) NOT NULL,
  city         VARCHAR(100) NOT NULL,
  state        VARCHAR(100) NOT NULL,
  zip_code     VARCHAR(20)  NOT NULL,
  FOREIGN KEY (user_id) REFERENCES users(user_id) ON DELETE CASCADE
);

-- ---------- products (getProducts.php) ----------
CREATE TABLE products (
  product_id         INT AUTO_INCREMENT PRIMARY KEY,
  item_name          VARCHAR(150) NOT NULL,
  item_description   VARCHAR(255) NOT NULL,
  price              DECIMAL(10,2) NOT NULL,
  quantity_available INT NOT NULL DEFAULT 0,
  image_url          VARCHAR(255) NOT NULL   -- relative path, e.g. images/apples.jpg
);

-- ---------- cart_items (addCartItem / getCart / updateCartItem / deleteCartItem) ----------
-- UNIQUE(user_id, product_id) is REQUIRED — addCartItem.php uses
-- INSERT ... ON DUPLICATE KEY UPDATE, which needs that unique key to work.
CREATE TABLE cart_items (
  cart_item_id INT AUTO_INCREMENT PRIMARY KEY,
  user_id      INT NOT NULL,
  product_id   INT NOT NULL,
  quantity     INT NOT NULL DEFAULT 1,
  UNIQUE KEY uq_user_product (user_id, product_id),
  FOREIGN KEY (user_id)    REFERENCES users(user_id)      ON DELETE CASCADE,
  FOREIGN KEY (product_id) REFERENCES products(product_id) ON DELETE CASCADE
);

-- ---------- discount_codes (getDiscount.php / placeOrder.php) ----------
CREATE TABLE discount_codes (
  discount_id      INT AUTO_INCREMENT PRIMARY KEY,
  code             VARCHAR(50) NOT NULL UNIQUE,
  discount_percent DECIMAL(5,2) NOT NULL,
  active           TINYINT(1) NOT NULL DEFAULT 1
);

-- ---------- orders (placeOrder.php / getOrderHistory.php) ----------
-- order_date defaults to NOW() because placeOrder.php never inserts it
-- but getOrderHistory.php sorts by it.
CREATE TABLE orders (
  order_id        INT AUTO_INCREMENT PRIMARY KEY,
  user_id         INT NOT NULL,
  address_id      INT NOT NULL,
  delivery_type   VARCHAR(20)  NOT NULL DEFAULT 'Standard',
  discount_code   VARCHAR(50)  NOT NULL DEFAULT '',
  subtotal        DECIMAL(10,2) NOT NULL,
  discount_amount DECIMAL(10,2) NOT NULL DEFAULT 0.00,
  tax_amount      DECIMAL(10,2) NOT NULL DEFAULT 0.00,
  delivery_fee    DECIMAL(10,2) NOT NULL DEFAULT 0.00,
  total_amount    DECIMAL(10,2) NOT NULL,
  order_date      TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
  FOREIGN KEY (user_id)    REFERENCES users(user_id),
  FOREIGN KEY (address_id) REFERENCES addresses(address_id)
);

-- ---------- order_items (placeOrder.php / getOrderHistory.php) ----------
CREATE TABLE order_items (
  order_item_id INT AUTO_INCREMENT PRIMARY KEY,
  order_id      INT NOT NULL,
  product_id    INT NOT NULL,
  item_name     VARCHAR(150) NOT NULL,   -- name/price snapshotted at order time
  item_price    DECIMAL(10,2) NOT NULL,
  quantity      INT NOT NULL,
  FOREIGN KEY (order_id) REFERENCES orders(order_id) ON DELETE CASCADE
);

-- ============================================================
-- Seed data
-- ============================================================

-- Discount codes (getDiscount.php upper-cases the input, so store UPPERCASE)
INSERT INTO discount_codes (code, discount_percent, active) VALUES
  ('SAVE10',    10.00, 1),
  ('WELCOME15', 15.00, 1),
  ('STUDENT20', 20.00, 1),
  ('EXPIRED',   50.00, 0);   -- inactive, for testing the "not valid" path

-- Products — image_url matches the files in /CustomerPortal/images/
INSERT INTO products (item_name, item_description, price, quantity_available, image_url) VALUES
  ('Apples',          'Fresh red apples, sold per lb',            2.49, 120, 'images/apples.jpg'),
  ('Avocados',        'Ripe Hass avocados, each',                 1.29,  80, 'images/avocados.jpg'),
  ('Bananas',         'Organic bananas, per lb',                  0.59, 200, 'images/bananas.jpg'),
  ('Bell Peppers',    'Mixed color bell peppers, each',           0.99, 150, 'images/bellpeppers.jpg'),
  ('Blueberries',     'Fresh blueberries, 1 pint',                3.99,  60, 'images/blueberries.jpg'),
  ('Bread',           'Whole wheat sandwich loaf',                2.79, 100, 'images/bread.jpg'),
  ('Broccoli',        'Fresh broccoli crowns, per lb',            1.89,  90, 'images/broccoli.jpg'),
  ('Butter',          'Salted butter, 1 lb',                      3.49,  70, 'images/butter.jpg'),
  ('Carrots',         'Whole carrots, 2 lb bag',                  1.49, 110, 'images/carrots.jpg'),
  ('Cheddar Cheese',  'Sharp cheddar block, 8 oz',                3.29,  75, 'images/cheddar.jpg'),
  ('Chicken Breast',  'Boneless skinless chicken breast, per lb', 4.99,  85, 'images/chicken.jpg'),
  ('Cooking Oil',     'Vegetable cooking oil, 48 fl oz',          4.49,  65, 'images/cookingoil.jpg'),
  ('Eggs',            'Large grade A eggs, dozen',                2.99, 130, 'images/eggs.jpg'),
  ('Flour',           'All-purpose flour, 5 lb bag',              2.69,  95, 'images/flour.jpg'),
  ('Grapes',          'Seedless green grapes, per lb',            2.99,  70, 'images/grapes.jpg'),
  ('Greek Yogurt',    'Plain Greek yogurt, 32 oz',                4.29,  60, 'images/greekyogurt.jpg'),
  ('Ground Beef',     '80/20 ground beef, per lb',                5.49,  80, 'images/groundbeef.jpg'),
  ('Heavy Cream',     'Heavy whipping cream, 16 fl oz',           3.19,  55, 'images/heavycream.jpg'),
  ('Macaroni',        'Elbow macaroni pasta, 16 oz',             1.19, 140, 'images/macaroni.jpg'),
  ('Milk',            'Whole milk, 1 gallon',                     3.59, 100, 'images/milk.jpg'),
  ('Mozzarella',      'Shredded mozzarella, 8 oz',                3.39,  75, 'images/mozzarella.jpg'),
  ('Orange Juice',    'No-pulp orange juice, 52 fl oz',           3.79,  85, 'images/orangejuice.jpg'),
  ('Pineapple',       'Whole fresh pineapple, each',              2.99,  50, 'images/pineapple.jpg'),
  ('Pork Chops',      'Bone-in pork chops, per lb',               4.19,  60, 'images/porkchops.jpg'),
  ('Rice',            'Long grain white rice, 5 lb bag',          5.99,  90, 'images/rice.jpg'),
  ('Salmon',          'Atlantic salmon fillet, per lb',           9.99,  40, 'images/salmon.jpg'),
  ('Shrimp',          'Peeled deveined shrimp, 1 lb',             8.49,  45, 'images/shrimp.jpg'),
  ('Spaghetti',       'Spaghetti pasta, 16 oz',                   1.19, 150, 'images/spaghetti.jpg'),
  ('Spinach',         'Baby spinach, 10 oz bag',                  2.49,  70, 'images/spinach.jpg'),
  ('Strawberries',    'Fresh strawberries, 1 lb',                 3.49,  65, 'images/strawberries.jpg'),
  ('Sugar',           'Granulated white sugar, 4 lb bag',         2.89, 100, 'images/sugar.jpg'),
  ('Turkey Breast',   'Sliced turkey breast, per lb',             6.49,  55, 'images/turkeybreast.jpg'),
  ('Watermelon',      'Seedless watermelon, each',                4.99,  35, 'images/watermelon.jpg');

-- Done. 7 tables, 33 products, 4 discount codes.
