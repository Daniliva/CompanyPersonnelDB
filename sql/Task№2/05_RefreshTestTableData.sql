-- Delete all rows from the original table
DELETE FROM TestTable;

-- Insert 25 rows into TestTable, including duplicates in the 'Name' column (e.g., 'Alice', 'Bob', 'Charlie')
-- This simulates data with repetitions for testing duplicate detection and removal
INSERT INTO TestTable (Name, Value) VALUES ('Alice', 10);
INSERT INTO TestTable (Name, Value) VALUES ('Bob', 20);
INSERT INTO TestTable (Name, Value) VALUES ('Charlie', 30);
INSERT INTO TestTable (Name, Value) VALUES ('Alice', 15);
INSERT INTO TestTable (Name, Value) VALUES ('David', 25);
INSERT INTO TestTable (Name, Value) VALUES ('Eve', 35);
INSERT INTO TestTable (Name, Value) VALUES ('Bob', 22);
INSERT INTO TestTable (Name, Value) VALUES ('Frank', 40);
INSERT INTO TestTable (Name, Value) VALUES ('Grace', 50);
INSERT INTO TestTable (Name, Value) VALUES ('Alice', 12);
INSERT INTO TestTable (Name, Value) VALUES ('Henry', 60);
INSERT INTO TestTable (Name, Value) VALUES ('Ivy', 70);
INSERT INTO TestTable (Name, Value) VALUES ('Jack', 80);
INSERT INTO TestTable (Name, Value) VALUES ('Bob', 23);
INSERT INTO TestTable (Name, Value) VALUES ('Kate', 90);
INSERT INTO TestTable (Name, Value) VALUES ('Leo', 100);
INSERT INTO TestTable (Name, Value) VALUES ('Mia', 110);
INSERT INTO TestTable (Name, Value) VALUES ('Noah', 120);
INSERT INTO TestTable (Name, Value) VALUES ('Olivia', 130);
INSERT INTO TestTable (Name, Value) VALUES ('Charlie', 32);
INSERT INTO TestTable (Name, Value) VALUES ('Paul', 140);
INSERT INTO TestTable (Name, Value) VALUES ('Quinn', 150);
INSERT INTO TestTable (Name, Value) VALUES ('Riley', 160);
INSERT INTO TestTable (Name, Value) VALUES ('Sam', 170);
INSERT INTO TestTable (Name, Value) VALUES ('Tina', 180);