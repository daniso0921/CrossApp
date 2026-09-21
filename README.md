# CrossApp — Лабораторна робота №2

## Структура Solution
Рішення (`CrossApp.sln`) складається з двох основних проєктів:
* **`src/Core/`** — бібліотека класів (`classlib`), яка відповідає за збір інформації про середовище за допомогою типів `EnvironmentInfo` та `EnvironmentReport`. Не містить точки входу (`Main`).
* **`src/Cli/`** — консольний застосунок (точка входу), який підключено до бібліотеки через `ProjectReference` (`Cli -> Core`). Він відповідає виключно за вивід даних на екран.

## Команди для роботи з проєктом
* Збірка: `dotnet build`
* Запуск: `dotnet run --project src/Cli`
* Публікація Self-contained: `dotnet publish src/Cli -c Release -r win-x64 --self-contained true`
* Публікація Framework-dependent: `dotnet publish src/Cli -c Release -r win-x64 --self-contained false`

## Таблиця порівняння режимів публікації

| RID | Режим | Розмір | Чи потрібен встановлений runtime? |
| :--- | :--- | :--- | :--- |
| `win-x64` | **Self-contained** | 78 МБ | **Ні** (повний комплект середовища вшитий у пакет) |
| `win-x64` | **Framework-dependent** | 0.2 МБ | **Так** (необхідний встановлений .NET 10 Runtime) |