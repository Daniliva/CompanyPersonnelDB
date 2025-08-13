-- Retrieve all rows that are duplicates based on the 'Name' column
-- This query identifies groups where 'Name' appears more than once and lists all such rows, ordered by Name and ID
SELECT * FROM TestTable 
WHERE Name IN (
    SELECT Name FROM TestTable 
    GROUP BY Name HAVING COUNT(*) > 1
) ORDER BY Name, ID;

-- Retrieve all duplicate rows with the count of repetitions based on the 'Name' column
-- Uses a subquery to filter duplicates and includes a COUNT to show repetition frequency
SELECT *, 
       (SELECT COUNT(*) FROM TestTable t2 WHERE t2.Name = t1.Name) AS DuplicateCount
FROM TestTable t1
WHERE Name IN (
    SELECT Name FROM TestTable
    GROUP BY Name HAVING COUNT(*) > 1
) ORDER BY Name, ID

-- Retrieve all rows that are duplicates based on the 'Name' column using a window function
-- This query computes the count of occurrences for each 'Name' group and filters rows where the count > 1
-- The result includes an additional 'DuplicateCount' column for reference, ordered by Name and ID
SELECT *
FROM (
    SELECT *, 
           COUNT(*) OVER (PARTITION BY Name) AS DuplicateCount
    FROM TestTable
) AS Sub
WHERE DuplicateCount > 1
ORDER BY Name, ID;