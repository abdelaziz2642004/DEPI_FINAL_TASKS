DROP DATABASE IF EXISTS bookstore;
CREATE DATABASE bookstore;
USE bookstore;

-- ========================
-- TASK 1: Schema Design
-- ========================

CREATE TABLE categories (
    category_id INT PRIMARY KEY AUTO_INCREMENT,
    category_name VARCHAR(100) NOT NULL
);

CREATE TABLE authors (
    author_id INT PRIMARY KEY AUTO_INCREMENT,
    first_name VARCHAR(50) NOT NULL,
    last_name VARCHAR(50) NOT NULL
);

CREATE TABLE books (
    book_id INT PRIMARY KEY AUTO_INCREMENT,
    title VARCHAR(255) NOT NULL,
    price DECIMAL(10,2) NOT NULL,
    stock_quantity INT NOT NULL DEFAULT 0,
    category_id INT NOT NULL,
    author_id INT NOT NULL,
    CONSTRAINT price_must_be_positive CHECK (price > 0),
    CONSTRAINT stock_cant_go_negative CHECK (stock_quantity >= 0),
    FOREIGN KEY (category_id) REFERENCES categories(category_id),
    FOREIGN KEY (author_id) REFERENCES authors(author_id)
);

CREATE TABLE customers (
    customer_id INT PRIMARY KEY AUTO_INCREMENT,
    first_name VARCHAR(50) NOT NULL,
    last_name  VARCHAR(50) NOT NULL,
    email VARCHAR(150) NOT NULL UNIQUE,
    city VARCHAR(100),
    created_at DATE NOT NULL DEFAULT (CURRENT_DATE)
);

CREATE TABLE purchases (
    purchase_id INT PRIMARY KEY AUTO_INCREMENT,
    customer_id INT NOT NULL,
    purchase_date DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (customer_id) REFERENCES customers(customer_id)
);

CREATE TABLE purchase_items (
    item_id INT PRIMARY KEY AUTO_INCREMENT,
    purchase_id INT NOT NULL,
    book_id INT NOT NULL,
    quantity INT NOT NULL DEFAULT 1,
    unit_price DECIMAL(10,2) NOT NULL,
    CONSTRAINT qty_positive CHECK (quantity > 0),
    CONSTRAINT item_price_positive CHECK (unit_price > 0),
    FOREIGN KEY (purchase_id) REFERENCES purchases(purchase_id),
    FOREIGN KEY (book_id) REFERENCES books(book_id)
);


-- ========================
-- TASK 2: Sample Data
-- ========================

INSERT INTO categories (category_name) VALUES
('Fiction'),
('Science'),
('History'),
('Self-Help'),
('Technology'),
('Biography'),
('Philosophy'),
('Children'),
('Mystery'),
('Economics');

INSERT INTO authors (first_name, last_name) VALUES
('George', 'Orwell'),
('Yuval', 'Harari'),
('James', 'Clear'),
('Stephen', 'King'),
('Malcolm', 'Gladwell'),
('Agatha', 'Christie'),
('Walter', 'Isaacson'),
('Nassim', 'Taleb'),
('J.K.', 'Rowling'),
('Michelle', 'Obama'),
('Fyodor', 'Dostoevsky'),
('Cal', 'Newport'),
('Daniel', 'Kahneman'),
('Roald', 'Dahl'),
('Frank', 'Herbert');

INSERT INTO books (title, price, stock_quantity, category_id, author_id) VALUES
('1984', 12.99, 45, 1, 1),
('Animal Farm', 9.49, 60, 1, 1),
('Sapiens', 17.50, 30, 2, 2),
('Homo Deus', 15.99, 25, 2, 2),
('Atomic Habits', 21.00, 80, 4, 3),
('The Shining', 14.25, 20, 9, 4),
('It', 18.75, 15, 9, 4),
('Outliers', 13.50, 35, 4, 5),
('The Tipping Point', 11.99, 40, 4, 5),
('Murder on the Orient Express', 10.00, 55, 9, 6),
('And Then There Were None', 9.99, 50, 9, 6),
('Steve Jobs', 19.99, 22, 6, 7),
('Leonardo da Vinci', 22.50, 18, 6, 7),
('The Black Swan', 16.00, 28, 10, 8),
('Antifragile', 18.00, 20, 10, 8),
('Harry Potter and the Sorcerers Stone', 14.99, 100, 1, 9),
('Harry Potter and the Chamber of Secrets', 13.99, 90, 1, 9),
('Becoming', 20.00, 40, 6, 10),
('Crime and Punishment', 11.50, 30, 7, 11),
('The Brothers Karamazov', 13.00, 22, 7, 11),
('Deep Work', 16.50, 38, 4, 12),
('Digital Minimalism', 14.00, 33, 4, 12),
('Thinking Fast and Slow', 17.99, 27, 2, 13),
('Charlie and the Chocolate Factory', 8.99, 75, 8, 14),
('The BFG', 7.50, 65, 8, 14),
('Dune', 19.00, 42, 1, 15),
('Dune Messiah', 17.00, 30, 1, 15),
('A Brief History of Time', 15.00, 35, 2, 2),
('The Alchemist', 10.50, 55, 7, 11),
('Meditations', 9.00, 48, 7, 11);

INSERT INTO customers (first_name, last_name, email, city, created_at) VALUES
('Lena', 'Marchetti', 'lena.marchetti@gmail.com', 'New York', '2023-01-10'),
('Omar', 'Fathi', 'omar.fathi@outlook.com', 'London', '2023-02-14'),
('Priya', 'Nair', 'priya.nair@yahoo.com', 'Mumbai', '2023-03-05'),
('Jake', 'Kowalski', 'jake.kow@gmail.com', 'Chicago', '2023-03-20'),
('Sofia', 'Bernal', 'sofia.bernal@hotmail.com', 'Madrid', '2023-04-01'),
('Chen', 'Wei', 'chen.wei88@gmail.com', 'Shanghai', '2023-04-18'),
('Amara', 'Diallo', 'amara.diallo@mail.com', 'Paris', '2023-05-02'),
('Lucas', 'Ferreira', 'lucas.ferr@gmail.com', 'Sao Paulo', '2023-05-15'),
('Hana', 'Suzuki', 'hana.suzuki@jp.com', 'Tokyo', '2023-06-10'),
('Ethan', 'Brooks', 'ethan.brooks@gmail.com', 'New York', '2023-06-22'),
('Nia', 'Owusu', 'nia.owusu@outlook.com', 'Accra', '2023-07-08'),
('Marco', 'Ricci', 'marco.ricci@libero.it', 'Rome', '2023-07-19'),
('Fatima', 'Al-Hassan', 'fatima.alh@gmail.com', 'Dubai', '2023-08-03'),
('Yusuf', 'Demir', 'yusuf.demir@hotmail.com', 'Istanbul', '2023-08-25'),
('Clara', 'Dupont', 'clara.dupont@laposte.net', 'Paris', '2023-09-12'),
('Ravi', 'Sharma', 'ravi.sharma@gmail.com', 'Mumbai', '2023-09-30'),
('Ingrid', 'Larsen', 'ingrid.l@outlook.no', 'Oslo', '2023-10-14'),
('Carlos', 'Mendez', 'c.mendez@gmail.com', 'Mexico City', '2023-10-28'),
('Yuki', 'Tanaka', 'yuki.tanaka@docomo.jp', 'Tokyo', '2023-11-05'),
('Abby', 'Thompson', 'abby.t@gmail.com', 'New York', '2023-11-20'),
('Kwame', 'Asante', 'k.asante@gmail.com', 'London', '2023-12-01'),
('Miriam', 'Cohen', 'miriam.c@walla.co.il', 'Tel Aviv', '2023-12-15'),
('Diego', 'Vargas', 'diego.v@gmail.com', 'Buenos Aires', '2024-01-07'),
('Aisha', 'Mansour', 'aisha.m@outlook.com', 'Cairo', '2024-01-22'),
('Nour', 'El-Amin', 'nour.elamin@gmail.com', 'Chicago', '2024-02-10');

INSERT INTO purchases (customer_id, purchase_date) VALUES
(1,  '2024-01-05 10:15:00'),
(1,  '2024-03-12 14:30:00'),
(2,  '2024-01-18 09:00:00'),
(3,  '2024-02-02 11:45:00'),
(3,  '2024-04-20 16:00:00'),
(4,  '2024-02-14 13:20:00'),
(5,  '2024-03-01 08:50:00'),
(6,  '2024-03-08 17:10:00'),
(6,  '2024-05-22 12:00:00'),
(7,  '2024-03-25 10:30:00'),
(8,  '2024-04-05 15:45:00'),
(9,  '2024-04-11 09:20:00'),
(10, '2024-04-28 11:00:00'),
(10, '2024-06-03 14:15:00'),
(11, '2024-05-07 10:00:00'),
(12, '2024-05-14 16:30:00'),
(13, '2024-05-29 09:45:00'),
(14, '2024-06-10 13:00:00'),
(15, '2024-06-18 11:30:00'),
(16, '2024-07-02 10:15:00'),
(17, '2024-07-15 14:00:00'),
(18, '2024-07-28 09:30:00'),
(19, '2024-08-05 12:45:00'),
(20, '2024-08-19 15:00:00'),
(21, '2024-09-03 10:00:00'),
(22, '2024-09-17 13:30:00'),
(23, '2024-10-01 11:00:00'),
(24, '2024-10-15 09:15:00'),
(1,  '2024-10-28 14:45:00'),
(3,  '2024-11-05 10:30:00'),
(6,  '2024-11-20 16:00:00'),
(10, '2024-12-02 09:00:00'),
(2,  '2024-12-15 13:45:00'),
(4,  '2024-12-20 11:15:00');

INSERT INTO purchase_items (purchase_id, book_id, quantity, unit_price) VALUES
(1,  5,  2, 21.00),
(1,  16, 1, 14.99),
(2,  3,  1, 17.50),
(2,  23, 1, 17.99),
(3,  10, 2, 10.00),
(3,  11, 1, 9.99),
(4,  1,  1, 12.99),
(4,  8,  2, 13.50),
(5,  5,  1, 21.00),
(5,  21, 1, 16.50),
(6,  16, 3, 14.99),
(6,  17, 2, 13.99),
(7,  12, 1, 19.99),
(7,  18, 1, 20.00),
(8,  26, 2, 19.00),
(8,  3,  1, 17.50),
(9,  5,  1, 21.00),
(9,  4,  1, 15.99),
(10, 7,  1, 18.75),
(10, 6,  1, 14.25),
(11, 23, 1, 17.99),
(11, 14, 1, 16.00),
(12, 24, 2, 8.99),
(12, 25, 1, 7.50),
(13, 5,  2, 21.00),
(13, 22, 1, 14.00),
(14, 15, 1, 18.00),
(14, 26, 1, 19.00),
(15, 8,  1, 13.50),
(15, 9,  2, 11.99),
(16, 1,  1, 12.99),
(16, 2,  2, 9.49),
(17, 13, 1, 22.50),
(17, 7,  1, 18.75),
(18, 16, 2, 14.99),
(18, 5,  1, 21.00),
(19, 27, 1, 17.00),
(19, 20, 1, 13.00),
(20, 3,  2, 17.50),
(20, 28, 1, 15.00),
(21, 21, 1, 16.50),
(21, 22, 1, 14.00),
(22, 19, 1, 11.50),
(22, 29, 1, 10.50),
(23, 5,  3, 21.00),
(23, 16, 1, 14.99),
(24, 12, 1, 19.99),
(24, 18, 1, 20.00),
(25, 10, 1, 10.00),
(25, 11, 2, 9.99),
(26, 30, 2, 9.00),
(26, 19, 1, 11.50),
(27, 13, 1, 22.50),
(27, 4,  1, 15.99),
(28, 16, 1, 14.99),
(28, 5,  2, 21.00),
(29, 3,  1, 17.50),
(29, 23, 1, 17.99),
(30, 26, 1, 19.00),
(30, 15, 1, 18.00),
(31, 5,  2, 21.00),
(31, 8,  1, 13.50),
(32, 1,  1, 12.99),
(32, 7,  1, 18.75),
(33, 12, 1, 19.99),
(33, 14, 1, 16.00),
(34, 16, 2, 14.99),
(34, 17, 1, 13.99);


-- ========================
-- TASK 3
-- ========================

select title, price, stock_quantity
from books
order by price desc;


-- ========================
-- TASK 4
-- ========================

select
    upper(b.title) as title,
    lower(concat(a.first_name, ' ', a.last_name)) as author
from books b
join authors a on b.author_id = a.author_id;


-- ========================
-- TASK 5
-- ========================

select b.title, c.category_name, concat(a.first_name, ' ', a.last_name) as author
from books b
join categories c on b.category_id = c.category_id
join authors a on b.author_id = a.author_id;


-- ========================
-- TASK 6
-- ========================

select
    concat(c.first_name, ' ', c.last_name) as customer,
    count(p.purchase_id) as total_purchases
from customers c
left join purchases p on c.customer_id = p.customer_id
group by c.customer_id
order by total_purchases desc;


-- ========================
-- TASK 7
-- ========================

select b.title, sum(pi.quantity) as total_sold
from purchase_items pi
join books b on pi.book_id = b.book_id
group by b.book_id, b.title
order by total_sold desc
limit 5;


-- ========================
-- TASK 8
-- ========================

select city, count(*) as num_customers
from customers
group by city
order by num_customers desc
limit 1;


-- ========================
-- TASK 9
-- ========================

select c.category_name, count(b.book_id) as book_count
from categories c
join books b on c.category_id = b.category_id
group by c.category_id, c.category_name
having count(b.book_id) > 5;


-- ========================
-- TASK 10
-- ========================

select title, price
from books
where price > (select avg(price) from books);


-- ========================
-- TASK 11
-- ========================

select first_name, last_name, email
from customers
where customer_id not in (select customer_id from purchases);


-- ========================
-- TASK 12
-- ========================

select
    date_format(p.purchase_date, '%Y-%m') as month,
    round(sum(pi.quantity * pi.unit_price), 2) as total_revenue
from purchases p
join purchase_items pi on p.purchase_id = pi.purchase_id
group by date_format(p.purchase_date, '%Y-%m')
order by month;


-- ========================
-- TASK 13
-- ========================

create or replace view book_catalog as
select
    b.book_id,
    b.title,
    c.category_name,
    concat(a.first_name, ' ', a.last_name) as author,
    b.price
from books b
join categories c on b.category_id = c.category_id
join authors a on b.author_id = a.author_id;


-- ========================
-- TASK 14
-- ========================

DELIMITER $$
create procedure get_customer_purchases(in p_customer_id int)
begin
    select
        p.purchase_id,
        p.purchase_date,
        b.title,
        pi.quantity,
        pi.unit_price,
        round(pi.quantity * pi.unit_price, 2) as line_total
    from purchases p
    join purchase_items pi on p.purchase_id = pi.purchase_id
    join books b on pi.book_id = b.book_id
    where p.customer_id = p_customer_id;

    select
        count(distinct p.purchase_id) as total_purchases,
        round(sum(pi.quantity * pi.unit_price), 2) as grand_total
    from purchases p
    join purchase_items pi on p.purchase_id = pi.purchase_id
    where p.customer_id = p_customer_id;
end$$
DELIMITER ;

CALL get_customer_purchases(1);
