# CompanyPersonnelDB — план доработки (тесты и документация первыми)

Ветка `spec` — приложение с заглушками (`NotImplementedException("TODO")`) и **79 тестами: 67 красных, 12 зелёных** (зелёные — описание того,
что уже работает: `DataMapper`, главная страница, защита от инъекции на уровне параметров). Ветка `reference` — все 79 зелёные.

## Что нашли в коде (подтверждено тестами)

| # | Где | Проблема | Тест, который это фиксирует |
|---|---|---|---|
| 1 | `SqlQueries.AppendNameFilter` | три условия `LIKE` соединены `AND` и добавляются всегда: у сотрудника без отчества (`middle_name IS NULL`) `LIKE` даёт «неизвестно», и он **пропадает даже при поиске только по фамилии** | `EmployeeQueryBuilderTests.An_employee_without_a_middle_name…` |
| 2 | `EmployeeService.UpdateEmployeeAsync` | `employee.MiddleName.Trim() ?? DBNull` — `Trim()` вызывается до проверки на `null` → NullReferenceException | `EmployeeServiceTests.Update_without_a_middle_name…` |
| 3 | `Models/Employee.cs` | при `<Nullable>enable` свойства `MiddleName`, `Address`, `Phone` без `?` MVC считает **обязательными**: сотрудника без отчества/адреса/телефона сохранить нельзя, хотя в БД эти поля необязательные | `HttpTests.Saving_an_employee_without_a_middle_name_works` |
| 4 | `EmployeesController.Details` (POST) | нет защиты от CSRF (`[ValidateAntiForgeryToken]`) | `HttpTests.Saving_without_an_anti_forgery_token_is_rejected` |
| 5 | `ExceptionMiddleware` | `new { error = … }.ToString()` пишет текст `{ error = … }` с типом `application/json` — это не JSON | `ExceptionMiddlewareTests.The_body_is_real_JSON…` |
| 6 | `UpdateEmployee` | если сотрудника нет, `UPDATE` меняет 0 строк, а пользователь получает редирект «как будто сохранено» | `HttpTests.Saving_an_employee_that_no_longer_exists_is_404` |
| 7 | `GetEmployees*` | нет `ORDER BY` — порядок строк не гарантирован | `…Order_by_is_the_last_clause` |
| 8 | отчёт в TXT | зарплата форматируется текущей культурой сервера (`1234,5` в de-DE), табы/переводы строк в именах ломают таблицу | `SalaryReportFormatterTests` |
| 9 | `%` и `_` в поиске | воспринимались как маски `LIKE` | `LIKE_wildcards_in_the_value_are_escaped…` |
| 10 | README | описывает регулярную проверку `ValidateInput`, которой в коде нет, и логику «OR» по трём полям; в коде — `AND` | см. «Документация» ниже |

Не вошло в тесты (решайте сами, см. этапы): пароль SA в `docker-compose.yaml`/`Dockerfile.db`, тот же код `company_id = 1` в контроллерах,
`DataMapper` проглатывает ошибки преобразования через `Console.WriteLine`.

## Что решено про поиск по имени (важно)

README обещает «OR», код делает «AND». Выбрано: **каждая заполненная часть имени фильтрует свой столбец, части объединяются через AND, пустые части игнорируются**.
Если вам нужен другой смысл (одно поле — поиск по всем трём столбцам через OR), поменяйте тесты `EmployeeQueryBuilderTests` и реализацию вместе.

## Этапы

1. **Тестируемый доступ к данным.** Интерфейс `IDataAccess` уже в заготовке; сервисы зависят от него. Код `DataAccessLayer` не меняется (кроме `: IDataAccess`).
2. **`EmployeeQueryBuilder`** — поиск (самый большой кусок логики). Зелёные: `EmployeeQueryBuilderTests`.
3. **`EmployeeService`**, **`SalaryService`** — зелёные `EmployeeServiceTests`, `SalaryServiceTests`.
4. **`SalaryReportFormatter`** — `SalaryReportFormatterTests`.
5. **`ExceptionMiddleware`** — `ExceptionMiddlewareTests`.
6. **Контроллер и модель** — CSRF, 404, обнуляемые поля — `HttpTests`.
7. **Дальше (после зелёных тестов):** вынести секреты из compose; обновить README под реальное поведение; пагинация и индексы
   (`department_id`, `position_id`, `last_name`); интеграционные тесты на настоящем SQL Server (Testcontainers); Dapper/EF Core; CI.

## Как запускать

```
cd src
dotnet test Project.Tests
```
Проект и тесты нацелены на `net9.0`. Если у вас нет .NET 9 SDK — поставьте его.
