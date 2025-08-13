# TaskN3 - Employee Department Management

## Task Description
Task 3: Retrieve all employees regardless of whether they are assigned to a department. Additionally, calculate the average salary for each department.

## Files and Their Purpose

- **01_CreateTables.sql**: Creates the `DEPARTMENTS` and `EMPLOYEES` tables (with a drop if exists check) with appropriate constraints and relationships.
- **02_PopulateTables.sql**: Populates the `DEPARTMENTS` and `EMPLOYEES` tables with initial data, including employees with and without department assignments.
- **03_RetrieveAllEmployees.sql**: Executes a query to retrieve all employees, including those without department assignments, using a left join.
- **04_AverageSalaryByDepartment.sql**: Calculates the average salary for each department based on employee data.

## Execution Order
Run the scripts in numerical order (01 to 04) to set up the tables, populate data, retrieve all employees, and calculate average salaries. Adjust the order as needed for testing.

## Notes
- Designed for Microsoft SQL Server (MSSQL).
