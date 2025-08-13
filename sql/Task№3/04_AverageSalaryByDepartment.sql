-- Calculate the average salary for each department, including only departments with employees
SELECT d.DEPARTMENT_NAME, AVG(e.SALARY) AS Average_Salary
FROM EMPLOYEES e
LEFT JOIN DEPARTMENTS d ON e.DEPARTMENT_ID = d.DEPARTMENT_ID
GROUP BY d.DEPARTMENT_NAME
HAVING AVG(e.SALARY) IS NOT NULL
ORDER BY d.DEPARTMENT_NAME;