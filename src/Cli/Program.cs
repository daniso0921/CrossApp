using Core;
using Core.Abstractions;
using Core.Services;
using Core.Storage;

Console.OutputEncoding = System.Text.Encoding.UTF8;

// Визначаємо режим сховища за наявністю прапорця --file
bool useFile = args.Contains("--file");
string dataPath = Path.Combine(AppContext.BaseDirectory, "data", "catalog.json");

// Composition Root: ручне впровадження залежності (DI)
IBookStore store = useFile
    ? new FileBookStore(dataPath)
    : new InMemoryBookStore(SampleData.Books());

var service = new LendingService(store);

Console.WriteLine($"Сховище: {store.GetType().Name}");
if (useFile)
{
    Console.WriteLine($"Шлях до файлу даних: {dataPath}");
}

Console.WriteLine("\n--- Успішні операції ---");

// Додавання нової книги через сервіс
var newBook = service.AddBook("978-617-12-9999-9", "Спляче місто", 2024, "Сучасний Автор");
Console.WriteLine($"Додано: [{newBook.Id}] \"{newBook.Title}\" ({newBook.Year} р.)");

// Видача щойно створеного примірника
service.IssueCopy(newBook.Id);
Console.WriteLine($"Видано примірник з id={newBook.Id}");

// Відображення списку книг
Console.WriteLine("\nПоточний каталог книг:");
foreach (var book in service.All())
{
    Console.WriteLine($"  [{book.Id,-11}] {book.Isbn,-20} \"{book.Title,-24}\" {book.Year,4} | {(book.IsIssued ? "ВИДАНО" : "В НАЯВНОСТІ")}");
}

Console.WriteLine("\n--- Сценарій обробки помилок ---");

// 1. Спроба повторної видачі зайнятої книги
TryExecute("Повторна видача зайнятого примірника", () =>
{
    service.IssueCopy(newBook.Id);
});

// 2. Спроба операції над неіснуючим ідентифікатором
TryExecute("Видача книги з неіснуючим id", () =>
{
    service.IssueCopy("NON-EXISTENT-ID");
});

static void TryExecute(string actionDescription, Action action)
{
    try
    {
        action();
        Console.WriteLine($"[ПРОРАХУНОК] {actionDescription}: операція мала завершитися помилкою!");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"[ПЕРЕХОПЛЕНО {ex.GetType().Name}] {actionDescription}: {ex.Message}");
    }
}