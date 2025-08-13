-- Attempt to delete department with DEPARTMENT_ID = 40
-- This will fail due to foreign key constraint unless modified
DELETE FROM DEPARTMENTS
WHERE DEPARTMENT_ID = 40;

-- Attempt to delete specific columns from EMPLOYEES where DEPT_ID = 90
-- This will fail due to incorrect syntax; corrected version provided below
-- Incorrect syntax (for demonstration):
DELETE EMPLOYEE_ID, SALARY, JOB_ID
FROM EMPLOYEES
WHERE DEPT_ID = 90;

-- Corrected syntax to delete entire rows (if intended):
DELETE FROM EMPLOYEES
WHERE DEPT_ID = 90;