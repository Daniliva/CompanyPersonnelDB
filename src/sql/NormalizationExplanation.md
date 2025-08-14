# Normalization to 3NF Explanation

## Original Data Structure
The initial data consists of a flat list of attributes: Department, Position, Full Name (ПІБ), Address, Phone, Date of Birth, Hire Date, Salary, and Company Information. Storing these in a single table would lead to several issues:
- **Redundancy**: Department and Position names would be repeated for each employee, increasing storage and update anomalies.
- **Non-atomic Values**: Full Name (ПІБ) contains multiple components (surname, given name, patronymic), and Company Information includes multiple fields (name, address, phone, description).
- **Dependencies**: Attributes like department name or position name depend on identifiers rather than the employee directly, causing potential inconsistencies.

Normalization to the third normal form (3NF) addresses these issues by organizing data into separate tables with atomic attributes, eliminating redundancies, and ensuring dependencies are properly managed.

## Achieving 1NF (First Normal Form)
To achieve 1NF, data must have no repeating groups, and all attributes must be atomic (indivisible).

- **Atomic Values**:
  - **Full Name (ПІБ)**: Split into `last_name`, `first_name`, and `middle_name` to store surname, given name, and patronymic separately. This ensures each component is independently queryable and editable.
  - **Address**: Kept as a single string (e.g., "Kyiv, Shevchenka St 1") for simplicity, as further splitting (street, city, etc.) was not specified. It is treated as atomic for this purpose.
  - **Phone**: Stored as a single string (e.g., "+380501234567"), atomic for contact purposes.
  - **Date of Birth and Hire Date**: Already atomic as DATE types.
  - **Salary**: Atomic as a DECIMAL value.
  - **Company Information**: Split into `company_name`, `company_address`, `company_phone`, and `description` to store each piece of information atomically.

- **No Repeating Groups**:
  - Instead of storing multiple employees in a single row or repeating department/position names, separate tables are created for `companies`, `departments`, `positions`, and `employees`. Each table stores unique instances of its respective entities (e.g., one row per department).

**Resulting Tables**:
- `companies`: Stores company details.
- `departments`: Stores department names, linked to a company.
- `positions`: Stores position names, linked to a company.
- `employees`: Stores employee details, referencing departments, positions, and companies via foreign keys.

## Achieving 2NF (Second Normal Form)
To achieve 2NF, the database must be in 1NF, and all non-key attributes must fully depend on the entire primary key (no partial dependencies).

- **1NF Compliance**: The tables already satisfy 1NF with atomic attributes and no repeating groups.
- **Eliminating Partial Dependencies**:
  - In a denormalized table, attributes like department name or position name might depend only on a part of a composite key (e.g., `department_id` in a table with a composite key of `employee_id, department_id`). This is avoided by:
    - Moving `department_id` and its name to the `departments` table, where `name` depends fully on `department_id` (the primary key).
    - Moving `position_id` and its name to the `positions` table, where `name` depends fully on `position_id`.
    - Storing `company_name`, `company_address`, etc., in the `companies` table, where they depend on `company_id`.
  - In the `employees` table, attributes like `salary`, `address`, `phone`, `birth_date`, and `hire_date` depend fully on `employee_id` (the primary key). The foreign keys (`department_id`, `position_id`, `company_id`) ensure referential integrity without introducing partial dependencies.

**Result**: Each table's non-key attributes depend entirely on their respective primary keys:
- `companies`: All attributes depend on `company_id`.
- `departments`: `name` depends on `department_id`.
- `positions`: `name` depends on `position_id`.
- `employees`: All attributes depend on `employee_id`.

## Achieving 3NF (Third Normal Form)
To achieve 3NF, the database must be in 2NF, and there must be no transitive dependencies (non-key attributes depending on other non-key attributes via the primary key).

- **2NF Compliance**: The tables are already in 2NF, with no partial dependencies.
- **Eliminating Transitive Dependencies**:
  - A transitive dependency would exist if, for example, `salary` depended on `position_id` rather than directly on `employee_id` (i.e., employee -> position -> salary). In this design:
    - `salary` is stored in the `employees` table and depends directly on `employee_id`, as salaries are unique per employee, not per position or department.
    - `department_id` in `employees` references the `departments` table, but no attributes in `employees` (like `last_name` or `salary`) depend on `department_id` beyond the relationship.
    - Similarly, `position_id` and `company_id` are foreign keys, and no non-key attributes in `employees` depend on them transitively.
  - In the `departments` table, `name` depends directly on `department_id`, not on `company_id` beyond the foreign key relationship.
  - In the `positions` table, `name` depends directly on `position_id`.
  - In the `companies` table, all attributes (`company_name`, `company_address`, etc.) depend directly on `company_id`.

- **Foreign Keys and Integrity**:
  - Foreign keys (`company_id` in `departments`, `positions`, and `employees`; `department_id` and `position_id` in `employees`) ensure referential integrity without introducing transitive dependencies.
  - The single company constraint is enforced by linking all records to `company_id = 1`, but the structure allows scalability if more companies are added.

## Final Structure
The resulting database schema is in 3NF:
- **Atomicity**: All attributes are indivisible (e.g., Full Name split into three fields).
- **No Redundancy**: Department names, position names, and company details are stored once in their respective tables.
- **Dependency Preservation**: All dependencies are direct (on primary keys) or through foreign keys, with no transitive dependencies.
- **Scalability**: The design supports multiple companies, departments, or positions if needed, while currently handling one company as specified.

This structure minimizes storage, avoids update anomalies, and ensures data consistency, meeting the requirements for atomic storage and 3NF compliance.