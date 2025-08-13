-- Delete all existing rows from EMPLOYEES to ensure a clean slate before population
DELETE FROM [EMPLOYEES];

-- Delete all existing rows from DEPARTMENTS to ensure a clean slate before population
DELETE FROM [DEPARTMENTS];

-- Reset the IDENTITY value for DEPARTMENTS to ensure sequential IDs start at 10
DBCC CHECKIDENT ('[Test1].[dbo].[DEPARTMENTS]', RESEED, 0);

-- Insert data into DEPARTMENTS table (unchanged from original)
INSERT INTO DEPARTMENTS (DEPARTMENT_NAME) VALUES ('Sales');
INSERT INTO DEPARTMENTS (DEPARTMENT_NAME) VALUES ('Marketing');
INSERT INTO DEPARTMENTS (DEPARTMENT_NAME) VALUES ('Accounts');
INSERT INTO DEPARTMENTS (DEPARTMENT_NAME) VALUES ('Administration');

-- Insert expanded data into EMPLOYEES table, adding more employees for diversity
-- This includes additional employees across departments, some with NULL department for testing
INSERT INTO EMPLOYEES (LAST_NAME, DEPARTMENT_ID, SALARY) VALUES ('Getz', 10, 3000.00);
INSERT INTO EMPLOYEES (LAST_NAME, DEPARTMENT_ID, SALARY) VALUES ('Davis', 20, 1500.00);
INSERT INTO EMPLOYEES (LAST_NAME, DEPARTMENT_ID, SALARY) VALUES ('King', 20, 2200.00);
INSERT INTO EMPLOYEES (LAST_NAME, DEPARTMENT_ID, SALARY) VALUES ('Davis', 30, 5000.00);
INSERT INTO EMPLOYEES (LAST_NAME, DEPARTMENT_ID, SALARY) VALUES ('Kochhar', 30, 5000.00);
INSERT INTO EMPLOYEES (LAST_NAME, DEPARTMENT_ID, SALARY) VALUES ('Smith', 10, 2800.00);    -- Additional in Sales
INSERT INTO EMPLOYEES (LAST_NAME, DEPARTMENT_ID, SALARY) VALUES ('Johnson', 20, 1800.00); -- Additional in Marketing
INSERT INTO EMPLOYEES (LAST_NAME, DEPARTMENT_ID, SALARY) VALUES ('Lee', 30, 4500.00);     -- Additional in Accounts
INSERT INTO EMPLOYEES (LAST_NAME, DEPARTMENT_ID, SALARY) VALUES ('Patel', 40, 3500.00);   -- Additional in Administration
INSERT INTO EMPLOYEES (LAST_NAME, DEPARTMENT_ID, SALARY) VALUES ('Brown', NULL, 4000.00); -- Additional without department
INSERT INTO EMPLOYEES (LAST_NAME, DEPARTMENT_ID, SALARY) VALUES ('Wilson', 10, 3200.00);  -- Additional in Sales
INSERT INTO EMPLOYEES (LAST_NAME, DEPARTMENT_ID, SALARY) VALUES ('Taylor', 20, 2000.00);  -- Additional in Marketing
INSERT INTO EMPLOYEES (LAST_NAME, DEPARTMENT_ID, SALARY) VALUES ('Anderson', 30, 4800.00);-- Additional in Accounts
INSERT INTO EMPLOYEES (LAST_NAME, DEPARTMENT_ID, SALARY) VALUES ('Thomas', NULL, 2500.00);-- Additional without department
INSERT INTO EMPLOYEES (LAST_NAME, DEPARTMENT_ID, SALARY) VALUES ('Jackson', 40, 3700.00); -- Additional in Administration