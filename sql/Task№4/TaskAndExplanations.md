# Task4 - Department Salary Aggregates

## Task Description
Task 4: Write a SQL query to select the department identifier (DEPT_ID), the minimum salary, and the maximum salary paid in that department from the EMPLOYEES table, considering that the minimum salary is less than 5000 and the maximum salary is greater than 15000.

## Files and Their Purpose

- **01_CreateTable.sql**: Creates the EMPLOYEES table (with a drop if exists check) with the specified columns and constraints, including NOT NULL for EMP_ID and LAST_NAME, and a primary key on EMP_ID.
- **02_PopulateTable.sql**: Deletes existing rows from EMPLOYEES and populates it with sample data reflecting various salary ranges across departments to test the query conditions.
- **03_PerformSelectQuery.sql**: Executes the select query to retrieve DEPT_ID, MIN(SALARY), and MAX(SALARY) for departments meeting the specified salary conditions.

## Execution Order
Run the scripts in numerical order (01 to 03) to set up the table, populate data, and perform the select query. Review the results for analysis.

## Notes
- Designed for Microsoft SQL Server (MSSQL).
- **Query Analysis**: The query uses `GROUP BY DEPT_ID` to aggregate the minimum and maximum salaries per department. The `HAVING` clause filters groups where `MIN(SALARY) < 5000` and `MAX(SALARY) > 15000`. Based on the sample data in `02_PopulateTable.sql`, departments 10 and 20 will be returned because:
  - Department 10: MIN(SALARY) = 3000.00 (< 5000), MAX(SALARY) = 16000.00 (> 15000).
  - Department 20: MIN(SALARY) = 4500.00 (< 5000), MAX(SALARY) = 18000.00 (> 15000).
  - Department 30: MIN(SALARY) = 2800.00 (< 5000), but MAX(SALARY) = 6000.00 (< 15000), so it is excluded.
  - Department 40: MIN(SALARY) = 3200.00 (< 5000), but MAX(SALARY) = 14000.00 (< 15000), so it is excluded.
- The query does not violate any constraints (e.g., NOT NULL on EMP_ID and LAST_NAME) and assumes valid data. If no departments meet the criteria, an empty result set is returned.