-- 01_CreateTables.sql
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