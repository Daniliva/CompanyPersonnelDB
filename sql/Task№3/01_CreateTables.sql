-- Check if the EMPLOYEES table exists and drop it to avoid conflicts during recreation
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[EMPLOYEES]') AND type IN (N'U'))
    DROP TABLE [dbo].[EMPLOYEES];

-- Check if the DEPARTMENTS table exists and drop it to avoid conflicts during recreation
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DEPARTMENTS]') AND type IN (N'U'))
    DROP TABLE [dbo].[DEPARTMENTS];

-- Create the DEPARTMENTS table with an identity primary key and department name
CREATE TABLE DEPARTMENTS (
    DEPARTMENT_ID INT IDENTITY(10,10) PRIMARY KEY,
    DEPARTMENT_NAME VARCHAR(50) NOT NULL
);

-- Create the EMPLOYEES table with a primary key, foreign key to DEPARTMENTS, and salary
CREATE TABLE EMPLOYEES (
    LAST_NAME VARCHAR(30) NOT NULL,
    DEPARTMENT_ID INT,
    SALARY DECIMAL(10,2),
    FOREIGN KEY (DEPARTMENT_ID) REFERENCES DEPARTMENTS(DEPARTMENT_ID)
);