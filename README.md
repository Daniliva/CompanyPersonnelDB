# Company Personnel Database

### Features
- **Task 1: Employee Management System**
  - **Database**: Normalized to 3NF with `companies`, `departments`, `positions`, and `employees` tables, ensuring atomic storage (e.g., Full Name split into `last_name`, `first_name`, `middle_name`).
  - **Main Page**: Displays company details (`/Home`).
  - **Employee Listing**: Filters employees by department, position, or partial name matches (`/Employees`), with `OR` logic for flexible name searches (e.g., `lastName: Smith` matches "Smithson" in any name field).
  - **Employee Editing**: Updates employee details (`/Employees/Details/{id}`).
  - **Salary Reports**: Displays filtered salaries with total salary calculation and TXT export (`/SalaryReport`).
  - **Security**: Parameterized queries and regex-based validation (`ValidateInput`) prevent SQL injection.
  - **UI**: Bootstrap 5 for a responsive interface.
- **Task 2: Duplicate Row Management** (`sql/Task2/`):
  - Identifies and deletes duplicate rows in a test table, keeping the latest row (highest `ID`).
  - Provides multiple deletion methods (CTE, self-join, subquery).
  - Includes a script to refresh test data.
- **Task 3: Employee Department Management** (`sql/Task3/`):
  - Retrieves all employees, including those without departments, using `LEFT JOIN`.
  - Calculates average salary per department.
- **Task 4: Department Salary Aggregates** (`sql/Task4/`):
  - Selects departments with `MIN(SALARY) < 5000` and `MAX(SALARY) > 15000`.
- **Task 5: Delete Operations Analysis** (`sql/Task5/`):
  - Analyzes outcomes of department and employee delete operations, handling foreign key and syntax issues.

## Prerequisites
- **.NET SDK**: Version 9.0.
- **SQL Server**: Microsoft SQL Server 2022.
- **Tools**:
  - SQL Server Management Studio (SSMS) or Azure Data Studio for database management.
  - Visual Studio 2022 or VS Code for development.

## Folder Structure
```
CompanyPersonnelDB/
├── src/
│   ├── Project/
│   │   ├── Controllers/
│   │   │   ├── HomeController.cs           # Displays company info
│   │   │   ├── EmployeesController.cs      # Manages employee listing and editing
│   │   │   ├── SalaryReportController.cs   # Handles salary reports and TXT export
│   │   ├── Services/
│   │   │   ├── EmployeeService.cs          # Employee operations with flexible name filtering
│   │   │   ├── CompanyService.cs           # Company, department, position operations
│   │   │   ├── SalaryService.cs            # Salary calculations
│   │   ├── DataAccess/
│   │   │   ├── DataAccessLayer.cs          # Executes SQL queries
│   │   │   ├── DataMapper.cs               # Maps database results to objects
│   │   │   ├── SqlQueries.cs               # Defines SQL queries
│   │   ├── Models/
│   │   │   ├── Company.cs                  # Company model
│   │   │   ├── Department.cs               # Department model
│   │   │   ├── Position.cs                 # Position model
│   │   │   ├── Employee.cs                 # Employee model
│   │   │   ├── ErrorViewModel.cs           # Error handling model
│   │   ├── Views/
│   │   │   ├── Home/
│   │   │   │   ├── Index.cshtml            # Company info page
│   │   │   ├── Employees/
│   │   │   │   ├── Index.cshtml            # Employee list with filters
│   │   │   │   ├── Details.cshtml          # Employee edit page
│   │   │   ├── SalaryReport/
│   │   │   │   ├── Index.cshtml            # Salary report with export
│   │   │   ├── Shared/
│   │   │   │   ├── Error.cshtml            # Error display
│   │   ├── Middlewares/
│   │   │   ├── ExceptionMiddleware.cs      # Handles exceptions, including SQL injection
│   │   ├── wwwroot/
│   │   │   ├── lib/
│   │   │   │   ├── bootstrap/             # Bootstrap CSS and JS
│   │   │   │   ├── jquery/                # jQuery library
│   │   ├── Program.cs                      # Application startup
│   │   ├── appsettings.json                # Configuration (connection string)
│   ├── sql/
│   │   ├── 01_CreateTables.sql            # Creates database schema
│   │   ├── 02_PopulateTables.sql          # Populates sample data
├── sql/
│   ├── Task2/
│   │   ├── 01_CreateAndPopulateTestTable.sql  # Creates table with duplicates
│   │   ├── 02_SelectAllTestTable.sql         # Views all rows
│   │   ├── 03_FindDuplicates.sql             # Identifies duplicate rows
│   │   ├── 04_DeleteDuplicates.sql           # Deletes duplicates (multiple methods)
│   │   ├── 05_RefreshTestTableData.sql       # Resets test data
│   ├── Task3/
│   │   ├── 01_CreateTables.sql               # Creates tables for Task 3
│   │   ├── 02_PopulateTables.sql             # Populates Task 3 data
│   │   ├── 03_RetrieveAllEmployees.sql       # Retrieves all employees
│   │   ├── 04_AverageSalaryByDepartment.sql  # Calculates average salaries
│   ├── Task4/
│   │   ├── 01_CreateTable.sql                # Creates EMPLOYEES table
│   │   ├── 02_PopulateTable.sql              # Populates Task 4 data
│   │   ├── 03_PerformSelectQuery.sql         # Selects salary aggregates
│   ├── Task5/
│   │   ├── 01_CreateTables.sql               # Creates tables for Task 5
│   │   ├── 02_PopulateTables.sql             # Populates Task 5 data
│   │   ├── 03_PerformDeleteOperations.sql    # Analyzes delete operations
├── README.md                                 # Project documentation
```

1. **Build and Run the Application**:
   ```bash
   cd src/Project
   dotnet build
   dotnet run
   ```
   - Access at `http://localhost:XXXX` (port may vary).

2. **Run SQL Tasks**:
   - Execute scripts in `sql/Task2/`, `sql/Task3/`, `sql/Task4/`, and `sql/Task5/` using SSMS in numerical order.
   - Example for Task 2:
     ```sql
     -- Run in order: 01_CreateAndPopulateTestTable.sql, 02_SelectAllTestTable.sql, 03_FindDuplicates.sql, 04_DeleteDuplicates.sql, 05_RefreshTestTableData.sql
     ```

## Database Schema
### Task 1 (Application)
Normalized to 3NF:
- **companies**: `company_id` (PK), `company_name` (NOT NULL), `company_address`, `company_phone`, `description`.
- **departments**: `department_id` (PK), `name` (NOT NULL), `company_id` (FK to `companies`).
- **positions**: `position_id` (PK), `name` (NOT NULL), `company_id` (FK to `companies`).
- **employees**: `employee_id` (PK), `last_name` (NOT NULL), `first_name` (NOT NULL), `middle_name`, `address`, `phone`, `birth_date`, `hire_date` (NOT NULL), `salary` (NOT NULL), `department_id` (FK to `departments`), `position_id` (FK to `positions`), `company_id` (FK to `companies`).
- **Atomicity**: Full Name split into `last_name`, `first_name`, `middle_name`; company details separated into distinct fields.
- **Constraints**: Foreign keys ensure referential integrity; `NOT NULL` on critical fields.

### Tasks 2–5
- **Task 2**: `TestTable` with `ID` (PK), `Name` (NOT NULL).
- **Task 3**: Uses `employees` and `departments` (similar to Task 1 but allows null `department_id`).
- **Task 4**: `EMPLOYEES` with `EMP_ID` (PK, NOT NULL), `LAST_NAME` (NOT NULL), `FIRST_NAME`, `DEPT_ID`, `JOB_CAT`, `SALARY`.
- **Task 5**: `DEPARTMENTS` with `department_id` (PK); `EMPLOYEES` with `EMPLOYEE_ID` (PK), `MGR_ID` (FK to `EMPLOYEE_ID`), `DEPT_ID` (FK to `DEPARTMENTS`), `JOB_ID` (NOT NULL).

## Usage
### Task 1: Employee Management System
1. **Main Page** (`http://localhost:XXXX`):
   - Displays company details (e.g., `TechCorp`, address, phone, description).
2. **Employee Listing** (`http://localhost:XXXX/Employees`):
   - Filter by department, position, or partial name matches using dropdowns or text inputs.
   - **Updated Feature**: Name filters use `OR` logic, matching any name field (e.g., `lastName: Smith` finds employees with "Smith" in `last_name`, `first_name`, or `middle_name`).
   - Example: `firstName: John` returns all employees with "John" in any name field.
3. **Employee Editing** (`http://localhost:XXXX/Employees/Details/{id}`):
   - Edit fields like name, address, phone, salary, department, and position.
   - Redirects to the employee list upon successful update.
4. **Salary Reports** (`http://localhost:XXXX/SalaryReport`):
   - Filter by department or position to view salaries in a table.
   - Displays total salary for the selected department.
   - Export filtered data to a TXT file via a download button.
5. **Error Handling**:
   - Malicious inputs (e.g., `lastName: ; DROP TABLE employees; --`) trigger `Shared/Error.cshtml` with "Potential SQL injection attempt detected in [parameterName]."

### Tasks 2–5: SQL Operations
- **Task 2: Duplicate Row Management** (`sql/Task2/`):
  - `01_CreateAndPopulateTestTable.sql`: Creates `TestTable` with 25 rows, including duplicates (e.g., 'John Smith' appears 5 times).
  - `02_SelectAllTestTable.sql`: Views all rows for inspection.
  - `03_FindDuplicates.sql`: Lists duplicate rows by `Name` and their details.
  - `04_DeleteDuplicates.sql`: Deletes duplicates, keeping the highest `ID` using CTE, self-join, and subquery methods. (It is also possible to focus on the date and take the entry that was added earlier.)
  - `05_RefreshTestTableData.sql`: Resets `TestTable` to its initial state.
- **Task 3: Employee Department Management** (`sql/Task3/`):
  - `01_CreateTables.sql`: Creates `DEPARTMENTS` and `EMPLOYEES` tables.
  - `02_PopulateTables.sql`: Populates with sample data.
  - `03_RetrieveAllEmployees.sql`: Retrieves all employees with `LEFT JOIN` to include those without departments.
  - `04_AverageSalaryByDepartment.sql`: Calculates average salary per department.
- **Task 4: Department Salary Aggregates** (`sql/Task4/`):
  - `01_CreateTable.sql`: Creates `EMPLOYEES` table.
  - `02_PopulateTable.sql`: Populates with sample data.
  - `03_PerformSelectQuery.sql`: Selects `DEPT_ID`, `MIN(SALARY)`, and `MAX(SALARY)` where minimum salary < XXXX and maximum > 1XXXX.
- **Task 5: Delete Operations Analysis** (`sql/Task5/`):
  - `01_CreateTables.sql`: Creates `DEPARTMENTS` and `EMPLOYEES` tables.
  - `02_PopulateTables.sql`: Populates with sample data, including `DEPT_ID = 90`.
  - `03_PerformDeleteOperations.sql`:
    - **Operation 1**: `DELETE FROM departments WHERE department_id = 40` fails due to foreign key constraints.
    - **Operation 2 (Incorrect)**: `DELETE employee_id, salary, job_id FROM employees WHERE dept_id = 90` fails due to syntax error.
    - **Operation 2 (Corrected)**: `DELETE FROM employees WHERE dept_id = 90` succeeds if no constraints are violated.