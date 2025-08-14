namespace Project.DataAccess;

public static class SqlQueries
{
    public static string GetCompanyById => "SELECT company_id, company_name, company_address, company_phone, description FROM companies WHERE company_id = @companyId;";

    public static string GetAllDepartments => "SELECT department_id, name, company_id FROM departments WHERE company_id = @companyId;";

    public static string GetAllPositions => "SELECT position_id, name, company_id FROM positions WHERE company_id = @companyId;";

    public static string GetEmployeesBase => "SELECT employee_id, last_name, first_name, middle_name, address, phone, birth_date, hire_date, salary, department_id, position_id, company_id FROM employees WHERE company_id = @companyId";

    public static string AppendDepartmentFilter => " AND department_id = @departmentId";
    public static string AppendPositionFilter => " AND position_id = @positionId";
    public static string AppendNameFilter => " AND (last_name LIKE '%' + @lastName + '%' AND first_name LIKE '%' + @firstName + '%' AND middle_name LIKE '%' + @middleName + '%')";

    public static string GetEmployeeById => "SELECT employee_id, last_name, first_name, middle_name, address, phone, birth_date, hire_date, salary, department_id, position_id, company_id FROM employees WHERE employee_id = @employeeId;";

    public static string UpdateEmployee => "UPDATE employees SET last_name = @lastName, first_name = @firstName, middle_name = @middleName, address = @address, phone = @phone, birth_date = @birthDate, hire_date = @hireDate, salary = @salary, department_id = @departmentId, position_id = @positionId WHERE employee_id = @employeeId;";

    public static string GetTotalSalaryByDepartment => "SELECT SUM(salary) FROM employees WHERE department_id = @departmentId AND company_id = @companyId";

    public static string GetSalaryReportBase => "SELECT employee_id, last_name, first_name, middle_name, salary, department_id, position_id FROM employees WHERE company_id = @companyId";
}