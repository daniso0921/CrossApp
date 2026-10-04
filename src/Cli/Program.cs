using Core.Dto;
using Core.Import;

string path = args.Length > 0 ? args[0] : Path.Combine("data", "sample.csv");

if (!File.Exists(path))
{
    Console.WriteLine($"Файл не знайдено: {Path.GetFullPath(path)}");
    return 1;
}

// Використовуємо універсальний диспетчер за розширенням
ImportResult<BookDto> result = BookImporter.Load(path);

// Статистика імпорту одним рядком (додаткове завдання)
int total = result.Items.Count + result.Errors.Count;
double errorPercent = total > 0 ? (double)result.Errors.Count / total * 100 : 0;
Console.WriteLine($"[СТАТИСТИКА] Усього: {total} | Прийнято: {result.Items.Count} | Пропущено: {result.Errors.Count} | Помилок: {errorPercent:F1}%");
Console.WriteLine(new string('-', 70));

foreach (BookDto b in result.Items.Take(5))
{
    Console.WriteLine($" {b.Id,-6} {b.Isbn,-20} {b.Title,-25} {b.Year,4}");
}
Console.WriteLine(new string('-', 70));

if (result.Errors.Count > 0)
{
    Console.WriteLine($"Пропущено рядків: {result.Errors.Count}");
    foreach (string e in result.Errors)
    {
        Console.WriteLine($" ! {e}");
    }
}

return 0;