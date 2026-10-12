using Core.Domain;

Console.OutputEncoding = System.Text.Encoding.UTF8;


Console.WriteLine("   СЦЕНАРІЙ 1: УСПІШНІ ОПЕРАЦІЇ ДОМЕННОЇ МОДЕЛІ  ");


// 1. Успішне створення примірника через фабричний метод
var book = BookCopy.Create("BC-101", "978-617-12-0001-1", "Кобзар", 1840, "Тарас Шевченко");
Console.WriteLine($"1. Створено примірник: {book}");

// 2. Оформлення видачі читачеві
var loan = Loan.Open("L-001", "READER-42", book, new DateTime(2026, 10, 1));
Console.WriteLine($"2. Оформлено видачу:   {loan}");
Console.WriteLine($"   Стан книги:        {book}");

// 3. Успішне закриття видачі (повернення книги)
loan.Close(new DateTime(2026, 10, 10));
Console.WriteLine($"3. Закрито видачу:     {loan}");
Console.WriteLine($"   Стан книги:        {book}");

Console.WriteLine();

Console.WriteLine("   СЦЕНАРІЙ 2: ПЕРЕВІРКА ПОРУШЕННЯ ІНВАРІАНТІВ   ");


// Тест 1: Порожній ISBN
TestInvariant("1. Створення книги з порожнім ISBN", () =>
{
    BookCopy.Create("BC-102", "   ", "Тіні забутих предків", 1911);
});

// Тест 2: Рік видання за межами допустимого інтервалу
TestInvariant("2. Рік видання раніший за 1450 рік", () =>
{
    BookCopy.Create("BC-103", "978-000-00-0000-0", "Літопис", 1200);
});

// Тест 3: Спроба повторно видати вже видану книгу
TestInvariant("3. Повторна видача вже зайнятого примірника", () =>
{
    var busyBook = BookCopy.Create("BC-104", "978-111-22-3333-4", "Місто", 1928);
    busyBook.Issue(); // Перша видача — успішна
    busyBook.Issue(); // Повторна видача — має кинути InvalidOperationException
});

// Тест 4: Дата повернення раніша за дату видачі
TestInvariant("4. Дата повернення раніша за дату видачі", () =>
{
    var copyForLoan = BookCopy.Create("BC-105", "978-555-66-7777-8", "Захар Беркут", 1883);
    var invalidLoan = Loan.Open("L-002", "READER-99", copyForLoan, new DateTime(2026, 10, 10));
    invalidLoan.Close(new DateTime(2026, 10, 1)); // Спроба повернути в минулому
});

// Тест 5: Повторне закриття вже закритої видачі
TestInvariant("5. Повторне закриття видачі", () =>
{
    var copy = BookCopy.Create("BC-106", "978-999-88-7777-6", "Кайдашева сім'я", 1879);
    var testLoan = Loan.Open("L-003", "READER-01", copy, new DateTime(2026, 10, 1));
    testLoan.Close(new DateTime(2026, 10, 5)); // Перше закриття
    testLoan.Close(new DateTime(2026, 10, 6)); // Повторне закриття
});

// Допоміжний метод для перехоплення винятків
static void TestInvariant(string testTitle, Action action)
{
    Console.WriteLine($"\n[ТЕСТ] {testTitle}");
    try
    {
        action();
        Console.WriteLine("  ПОМИЛКА: Інваріант НЕ спрацював!");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"  ПЕРЕХОПЛЕНО {ex.GetType().Name}: {ex.Message}");
    }
}