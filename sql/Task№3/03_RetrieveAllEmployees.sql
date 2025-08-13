-- Retrieve all employees with a left join to include those without department assignments
SELECT e.LAST_NAME, d.DEPARTMENT_NAME, e.SALARY
FROM EMPLOYEES e
LEFT JOIN DEPARTMENTS d ON e.DEPARTMENT_ID = d.DEPARTMENT_ID
ORDER BY e.LAST_NAME;