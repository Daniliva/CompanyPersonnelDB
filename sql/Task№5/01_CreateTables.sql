-- Check if the EMPLOYEES table exists and drop it to avoid conflicts during recreation
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[EMPLOYEES]') AND type IN (N'U'))
    DROP TABLE [dbo].[EMPLOYEES];

-- Check if the DEPARTMENTS table exists and drop it to avoid conflicts during recreation
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DEPARTMENTS]') AND type IN (N'U'))
    DROP TABLE [dbo].[DEPARTMENTS];

-- Create the DEPARTMENTS table with an identity primary key
CREATE TABLE DEPARTMENTS (
    DEPARTMENT_ID INT IDENTITY(10,10) PRIMARY KEY,
    DEPARTMENT_NAME VARCHAR(50) NOT NULL
);

-- Create the EMPLOYEES table with primary key, foreign key, and NOT NULL constraints
CREATE TABLE EMPLOYEES (
    EMPLOYEE_ID INT PRIMARY KEY,
    EMP_NAME VARCHAR(30) NOT NULL,
    DEPT_ID INT,
    MGR_ID INT,
    JOB_ID VARCHAR(30) NOT NULL,
    SALARY DECIMAL(10,2),
    FOREIGN KEY (DEPT_ID) REFERENCES DEPARTMENTS(DEPARTMENT_ID),
    FOREIGN KEY (MGR_ID) REFERENCES EMPLOYEES(EMPLOYEE_ID)
);