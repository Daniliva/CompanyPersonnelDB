-- Check if the EMPLOYEES table exists and drop it to avoid conflicts during recreation
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[EMPLOYEES]') AND type IN (N'U'))
    DROP TABLE [dbo].[EMPLOYEES];

-- Create the EMPLOYEES table with specified columns and constraints
CREATE TABLE EMPLOYEES (
    EMP_ID INT NOT NULL,
    LAST_NAME VARCHAR(30) NOT NULL,
    FIRST_NAME VARCHAR(30),
    DEPT_ID INT,
    JOB_CAT VARCHAR(30),
    SALARY DECIMAL(10,2),
    PRIMARY KEY (EMP_ID)
);