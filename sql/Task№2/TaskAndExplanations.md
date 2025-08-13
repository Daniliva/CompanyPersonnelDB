# TaskN2 - Duplicate Row Management

## Task Description
Task 2: A table contains data with duplicate rows based on certain column values. Retrieve the list of duplicate rows. Then, delete duplicate rows, retaining only one unique row per group (keep the latest rows, determined by the highest ID). Provide multiple solution variants for deletion where possible.

## Files and Their Purpose

- **01_CreateAndPopulateTestTable.sql**: Creates the `TestTable` (with a drop if exists check) and populates it with 25 rows, including duplicates in the `Name` column for testing purposes.
- **02_SelectAllTestTable.sql**: Executes a `SELECT *` query to view all rows in `TestTable` for initial inspection.
- **03_FindDuplicates.sql**: Retrieves all duplicate rows.
- **04_DeleteDuplicates.sql**: Deletes duplicate rows using.
- **05_RefreshTestTableData.sql**: Clears all rows from `TestTable` and re-inserts the 25 default rows to reset the dataset.

## Execution Order
Run the scripts in numerical order (01 to 05) to set up the table, inspect data, analyze duplicates, delete duplicates, and refresh the dataset. Adjust the order as needed for testing.