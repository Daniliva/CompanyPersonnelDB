-- Select DEPT_ID, minimum salary, and maximum salary from EMPLOYEES
-- where minimum salary < 5000 and maximum salary > 15000
SELECT DEPT_ID, MIN(SALARY) AS MIN_SALARY, MAX(SALARY) AS MAX_SALARY
FROM EMPLOYEES
GROUP BY DEPT_ID
HAVING MIN(SALARY) < 5000 AND MAX(SALARY) > 15000;