SELECT 
    YEAR(t.TransactionDate) AS Year,
    MONTH(t.TransactionDate) AS Month,
    c.CategoryType,
    SUM(t.Amount) AS TotalAmount
FROM Transactions t
INNER JOIN Categories c
    ON c.CategoryId = t.CategoryId
WHERE t.TransactionDate >= '2026-04-01'
  AND t.TransactionDate < '2026-06-01'
GROUP BY YEAR(t.TransactionDate), MONTH(t.TransactionDate), c.CategoryType
ORDER BY Year, Month, c.CategoryType;
