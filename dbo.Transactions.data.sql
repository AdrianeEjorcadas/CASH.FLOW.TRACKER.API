SELECT b.CategoryType, sum(a.Amount) as "total amount" FROM Transactions a
join Categories b
on a.CategoryId = b.CategoryId
where a.UserId = 'ced9f3da-ca69-4b1a-52bd-08de9b76e356'
and a.TransactionDate between '2026-04-01' and '2026-04-30'
group by b.CategoryType;




SELECT a.CategoryId, a.CategoryName, a.CategoryType from Categories a;





where a.UserId = 'ced9f3da-ca69-4b1a-52bd-08de9b76e356'
and a.TransactionDate between '2026-05-01' and '2026-05-30'
group by b.CategoryType
order by b.CategoryType