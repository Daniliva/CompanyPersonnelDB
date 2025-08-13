# TaskN5 - Employee and Department Delete Operations

## Task Description
Task 5: Analyze the outcomes of two delete operations:
1. `DELETE FROM departments WHERE department_id = 40` given that `EMPLOYEE_ID` is the primary key, `MGR_ID` references `EMPLOYEE_ID`, and `DEPT_ID` is a foreign key to `DEPARTMENT_ID` in the `DEPARTMENTS` table.
2. `DELETE employee_id, salary, job_id FROM employees WHERE dept_id = 90` given that `EMPLOYEE_ID` is the primary key, `MGR_ID` references `EMPLOYEE_ID`, and `JOB_ID` is NOT NULL.

## Files and Their Purpose

- **01_CreateTables.sql**: Creates the `DEPARTMENTS` and `EMPLOYEES` tables (with a drop if exists check) with primary and foreign key constraints.
- **02_PopulateTables.sql**: Deletes existing rows from `EMPLOYEES` and `DEPARTMENTS`, then populates them with sample data reflecting manager-employee relationships and job assignments, including a test row with `DEPT_ID = 90`.
- **03_PerformDeleteOperations.sql**: Executes the specified delete operations and provides a corrected version for the second operation.

## Execution Order
Run the scripts in numerical order (01 to 03) to set up the tables, populate data, and perform the delete operations. Review the results and error messages for analysis.

## Notes
- Designed for Microsoft SQL Server (MSSQL).
- **Operation 1 Analysis**: The `DELETE FROM departments WHERE department_id = 40` will fail with a foreign key constraint violation because employees (e.g., EMPLOYEE_ID 106, 110) have `DEPT_ID = 40`. This occurs due to the `FOREIGN KEY` constraint on `DEPT_ID` unless `ON DELETE CASCADE` is enabled (not specified here).
- **Operation 2 Analysis**: The `DELETE employee_id, salary, job_id FROM employees WHERE dept_id = 90` will fail with a syntax error because `DELETE` removes rows, not columns. The corrected `DELETE FROM EMPLOYEES WHERE DEPT_ID = 90` will succeed and delete the test row (EMPLOYEE_ID 130) added for this purpose, as it does not violate primary key or NOT NULL constraints.