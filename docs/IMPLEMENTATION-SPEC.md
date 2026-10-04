# CompanyPersonnelDB — спецификация реализации

На ветке `spec` помечены `throw new NotImplementedException("TODO")`:
`EmployeeQueryBuilder` (`BuildEmployeeSearch`, `AddNamePart`, `EscapeLike`), `EmployeeService` (3 метода + `NullIfBlank`),
`SalaryService` (2 метода), `SalaryReportFormatter` (`ToTxt`, `Clean`), `ExceptionMiddleware.HandleExceptionAsync`.
Подробные правила — в XML-комментариях рядом, поведение — в тестах `src/Project.Tests`.

## Порядок

1. **`EmployeeQueryBuilder.EscapeLike`** — `\`→`\\`, `%`→`\%`, `_`→`\_`, `[`→`\[`.
2. **`AddNamePart` + `BuildEmployeeSearch`**
   - база `SqlQueries.GetEmployeesBase`, первый параметр `@companyId`;
   - фильтры отдела/должности (`SqlQueries.AppendDepartmentFilter/AppendPositionFilter`);
   - для каждой НЕпустой (после `Trim`) части имени: `" AND {col} LIKE '%' + {param} + '%' ESCAPE '\'"`, значение экранировано `EscapeLike`;
   - в конце `SqlQueries.OrderEmployees`; текст запроса не содержит введённых пользователем значений.
3. **`EmployeeService`**
   - `GetEmployeesAsync` — запрос из построителя → `IDataAccess.QueryAsync<Employee>`;
   - `GetEmployeeByIdAsync` — первая строка или `null`;
   - `UpdateEmployeeAsync` — `ArgumentNullException` для `null`; `Trim` у фамилии и имени; `NullIfBlank` (пусто/пробелы → `DBNull.Value`, иначе `Trim`) для отчества, адреса, телефона;
     дата рождения `null` → `DBNull`; возвращает `true`, если изменено больше 0 строк. Параметры: `@employeeId @lastName @firstName @middleName @address @phone @birthDate @hireDate @salary @departmentId @positionId`.
4. **`SalaryService`**
   - отчёт: база + фильтры + `SqlQueries.OrderSalaryReport`;
   - итог: `GetTotalSalaryByDepartment` (+ фильтр по должности), результат `null` или `DBNull` → `null`, иначе `decimal`.
5. **`SalaryReportFormatter.ToTxt`** — заголовок `Header`, затем строки; разделитель — табуляция; перевод строки — `\n`; зарплата `F2` в инвариантной культуре;
   `null` в отчестве — пустая колонка; `\t`, `\r`, `\n` в именах → пробел.
6. **`ExceptionMiddleware`** — при исключении: `Response.Clear()`, 500, `application/json`, тело `{"error":"..."}` через `JsonSerializer`.
   В Production сообщение всегда «Internal Server Error»; в Development — сообщение + `\n` + стек. Если ответ уже начат (`HasStarted`) — пробросить исключение дальше (в заготовке эта проверка удалена).
7. **Контроллер и модель (вне заглушек, вернуть самому):**
   - `[ValidateAntiForgeryToken]` на POST `Employees/Details`; при `UpdateEmployeeAsync == false` — `NotFound()`;
   - в `Models/Employee.cs` сделать `MiddleName`, `Address`, `Phone` обнуляемыми (`string?`), иначе MVC считает их обязательными.

## Что проверено, а что нет

Проверено: 79 тестов на .NET 8 (проект целится в net9, при проверке цель заменялась на net8 и убирались вызовы `MapStaticAssets`/`WithStaticAssets`, которых нет в net8).
Не проверено: работа с настоящим SQL Server (запросы, `LIKE ... ESCAPE`, транзакция в `DataAccessLayer`) — в окружении нет Docker и SQL Server; все тесты работают с подменой `IDataAccess`.
Тексты SQL проверяются как строки, а не выполняются.
