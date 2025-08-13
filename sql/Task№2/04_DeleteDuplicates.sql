-- Use a Common Table Expression (CTE) with ROW_NUMBER() to rank rows by ID descending within each 'Name' group
-- Delete rows where the rank is greater than 1 (i.e., keep only the latest row per group)
WITH Ranked AS (
    SELECT *,
           ROW_NUMBER() OVER (PARTITION BY Name ORDER BY ID DESC) AS RN
    FROM TestTable
)
DELETE FROM TestTable 
WHERE ID IN (SELECT ID FROM Ranked WHERE RN > 1);

-- Use a subquery to find the maximum ID for each unique 'Name'
-- Delete all rows where ID is not the maximum in its group (i.e., retain only the latest row)
DELETE FROM TestTable 
WHERE ID NOT IN (
    SELECT MAX(ID) 
    FROM TestTable 
    GROUP BY Name
);