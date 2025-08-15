-- 01_CreateDatabaseAndTables.sql
-- Create the CompanyPersonnelDB database if it does not exist
IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'CompanyPersonnelDB')
BEGIN
    CREATE DATABASE CompanyPersonnelDB;
END;
GO

-- Switch to the CompanyPersonnelDB database
USE CompanyPersonnelDB;
GO

-- Clear the database by dropping tables if they exist to ensure a clean state
IF OBJECT_ID('employees', 'U') IS NOT NULL DROP TABLE employees;
IF OBJECT_ID('departments', 'U') IS NOT NULL DROP TABLE departments;
IF OBJECT_ID('positions', 'U') IS NOT NULL DROP TABLE positions;
IF OBJECT_ID('companies', 'U') IS NOT NULL DROP TABLE companies;

-- Create the Companies table to store company information
CREATE TABLE companies (
    company_id INT IDENTITY(1,1) PRIMARY KEY,
    company_name NVARCHAR(255) NOT NULL,
    company_address NVARCHAR(255),
    company_phone NVARCHAR(50),
    description NVARCHAR(MAX)
);

-- Create the Departments table to store department names
CREATE TABLE departments (
    department_id INT IDENTITY(1,1) PRIMARY KEY,
    name NVARCHAR(100) NOT NULL,
    company_id INT NOT NULL,
    FOREIGN KEY (company_id) REFERENCES companies(company_id)
);

-- Create the Positions table to store position names
CREATE TABLE positions (
    position_id INT IDENTITY(1,1) PRIMARY KEY,
    name NVARCHAR(100) NOT NULL,
    company_id INT NOT NULL,
    FOREIGN KEY (company_id) REFERENCES companies(company_id)
);

-- Create the Employees table to store employee details
CREATE TABLE employees (
    employee_id INT IDENTITY(1,1) PRIMARY KEY,
    last_name NVARCHAR(100) NOT NULL,
    first_name NVARCHAR(100) NOT NULL,
    middle_name NVARCHAR(100),
    address NVARCHAR(255),
    phone NVARCHAR(50),
    birth_date DATE,
    hire_date DATE NOT NULL,
    salary DECIMAL(10, 2) NOT NULL,
    department_id INT NOT NULL,
    position_id INT NOT NULL,
    company_id INT NOT NULL,
    FOREIGN KEY (department_id) REFERENCES departments(department_id),
    FOREIGN KEY (position_id) REFERENCES positions(position_id),
    FOREIGN KEY (company_id) REFERENCES companies(company_id)
);

-- 02_PopulateTables.sql
-- Clear existing data to ensure atomic insertion
DELETE FROM employees;
DELETE FROM departments;
DELETE FROM positions;
DELETE FROM companies;

-- Insert the single company record
INSERT INTO companies (company_name, company_address, company_phone, description)
VALUES ('TechCorp', '456 Innovation Ave, Kyiv, Ukraine', '+380441234567', 'A leading technology company specializing in software development.');

-- Insert departments, all linked to company_id 1
INSERT INTO departments (name, company_id) VALUES ('Sales', 1);
INSERT INTO departments (name, company_id) VALUES ('HR', 1);
INSERT INTO departments (name, company_id) VALUES ('IT', 1);
INSERT INTO departments (name, company_id) VALUES ('Finance', 1);
INSERT INTO departments (name, company_id) VALUES ('Marketing', 1);

-- Insert positions, all linked to company_id 1
INSERT INTO positions (name, company_id) VALUES ('Manager', 1);
INSERT INTO positions (name, company_id) VALUES ('Engineer', 1);
INSERT INTO positions (name, company_id) VALUES ('Analyst', 1);
INSERT INTO positions (name, company_id) VALUES ('Developer', 1);
INSERT INTO positions (name, company_id) VALUES ('Accountant', 1);

-- Insert 25 employee records, all linked to company_id 1
INSERT INTO employees (last_name, first_name, middle_name, address, phone, birth_date, hire_date, salary, department_id, position_id, company_id)
VALUES 
('Ivanov', 'Ivan', 'Ivanovych', 'Kyiv, Shevchenka St 1', '+380501234567', '1980-01-01', '2020-01-01', 50000.00, 1, 1, 1),
('Petrova', 'Anna', 'Oleksandrivna', 'Lviv, Franka St 2', '+380502345678', '1985-02-02', '2021-02-02', 45000.00, 2, 2, 1),
('Sidorov', 'Oleksandr', 'Petrovych', 'Odesa, Deribasivska St 3', '+380503456789', '1990-03-03', '2022-03-03', 60000.00, 3, 3, 1),
('Kovalenko', 'Maria', 'Ivanivna', 'Kharkiv, Sumska St 4', '+380504567890', '1975-04-04', '2019-04-04', 55000.00, 4, 4, 1),
('Shevchenko', 'Taras', 'Hryhorovych', 'Dnipro, Yavornytskoho St 5', '+380505678901', '1982-05-05', '2020-05-05', 52000.00, 5, 5, 1),
('Bondarenko', 'Olga', 'Mykhailivna', 'Kyiv, Khreshchatyk St 6', '+380506789012', '1987-06-06', '2021-06-06', 48000.00, 1, 1, 1),
('Tkachenko', 'Serhii', 'Viktorovych', 'Lviv, Rynok Sq 7', '+380507890123', '1992-07-07', '2022-07-07', 61000.00, 2, 2, 1),
('Melnyk', 'Natalia', 'Andriivna', 'Odesa, Lanzheron St 8', '+380508901234', '1978-08-08', '2019-08-08', 53000.00, 3, 3, 1),
('Kozak', 'Dmytro', 'Olehovych', 'Kharkiv, Nauky Ave 9', '+380509012345', '1983-09-09', '2020-09-09', 57000.00, 4, 4, 1),
('Lysenko', 'Viktoria', 'Serhiivna', 'Dnipro, Karl Marx Ave 10', '+380501023456', '1988-10-10', '2021-10-10', 49000.00, 5, 5, 1),
('Marchenko', 'Andrii', 'Pavlovych', 'Kyiv, Saksahanskoho St 11', '+380501134567', '1993-11-11', '2022-11-11', 62000.00, 1, 1, 1),
('Rudenko', 'Olena', 'Vasylivna', 'Lviv, Svobody Ave 12', '+380501245678', '1979-12-12', '2019-12-12', 54000.00, 2, 2, 1),
('Havrylenko', 'Yurii', 'Mykolayovych', 'Odesa, Richelieu St 13', '+380501356789', '1984-01-13', '2020-01-13', 58000.00, 3, 3, 1),
('Zahorodnii', 'Svitlana', 'Ihorivna', 'Kharkiv, Poltavskyi Shlyah 14', '+380501467890', '1989-02-14', '2021-02-14', 50000.00, 4, 4, 1),
('Boiko', 'Vitalii', 'Oleksiyovych', 'Dnipro, Slobozhanskyi Ave 15', '+380501578901', '1994-03-15', '2022-03-15', 63000.00, 5, 5, 1),
('Kravchenko', 'Inna', 'Stepanivna', 'Kyiv, Volodymyrska St 16', '+380501689012', '1980-04-16', '2019-04-16', 55000.00, 1, 1, 1),
('Pavlenko', 'Roman', 'Borysovych', 'Lviv, Halytska St 17', '+380501790123', '1985-05-17', '2020-05-17', 59000.00, 2, 2, 1),
('Oliinyk', 'Tetiana', 'Fedirivna', 'Odesa, Gretska St 18', '+3805018901234', '1990-06-18', '2021-06-18', 51000.00, 3, 3, 1),
('Symonenko', 'Maksym', 'Artemovych', 'Kharkiv, Moskovskyi Ave 19', '+3805019012345', '1976-07-19', '2022-07-19', 64000.00, 4, 4, 1),
('Honcharenko', 'Yana', 'Vitaliivna', 'Dnipro, Centralna St 20', '+3805020123456', '1981-08-20', '2019-08-20', 56000.00, 5, 5, 1),
('Fedorenko', 'Ihor', 'Yuriyovych', 'Kyiv, Bazhana Ave 21', '+3805021234567', '1986-09-21', '2020-09-21', 60000.00, 1, 1, 1),
('Soroka', 'Kateryna', 'Romanivna', 'Lviv, Kopernika St 22', '+3805022345678', '1991-10-22', '2021-10-22', 52000.00, 2, 2, 1),
('Lytvyn', 'Pavlo', 'Denysovych', 'Odesa, Fontanska Rd 23', '+3805023456789', '1977-11-23', '2022-11-23', 65000.00, 3, 3, 1),
('Mazur', 'Alina', 'Bohdanivna', 'Kharkiv, Heroiv Pratsi St 24', '+3805024567890', '1982-12-24', '2019-12-24', 57000.00, 4, 4, 1),
('Polishchuk', 'Oleh', 'Valeriyovych', 'Dnipro, Haharina Ave 25', '+3805025678901', '1987-01-25', '2020-01-25', 61000.00, 5, 5, 1);