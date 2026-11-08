using System.Collections.Generic;

namespace Core;

public static class SampleData
{
    // Метод повертає список об'єктів для тестування звітів
    // Замініть конструктор або виклики під ваші реальні поля сутності (наприклад, Book.Create або конструктор)
    public static IReadOnlyList<Book> GetBooks() => new List<Book>
    {
        Book.Create("ISBN-101", "CLR via C#", "Програмування", 14),
        Book.Create("ISBN-102", "C# in Depth", "Програмування", 18),
        Book.Create("ISBN-103", "Clean Architecture", "Програмування", 25),
        Book.Create("ISBN-104", "Domain-Driven Design", "Програмування", 4),  // < 10 (дефіцит)
        Book.Create("ISBN-105", "Refactoring", "Програмування", 12),
        Book.Create("ISBN-106", "Design Patterns", "Програмування", 14),      // для перевірки стабільності сортування (ThenBy)

        Book.Create("ISBN-201", "Introduction to Algorithms", "Наука", 20),
        Book.Create("ISBN-202", "Concrete Mathematics", "Наука", 6),         // < 10 (дефіцит)
        Book.Create("ISBN-203", "Discrete Mathematics", "Наука", 15),
        Book.Create("ISBN-204", "Linear Algebra Done Right", "Наука", 11),
        Book.Create("ISBN-205", "Structure and Interpretation", "Наука", 2), // < 10 (дефіцит)

        Book.Create("ISBN-301", "Dune", "Фантастика", 22),
        Book.Create("ISBN-302", "Neuromancer", "Фантастика", 9),             // < 10 (дефіцит)
        Book.Create("ISBN-303", "Foundation", "Фантастика", 16),
        Book.Create("ISBN-304", "Solaris", "Фантастика", 12),

        Book.Create("ISBN-401", "Тигролови", "Класика", 19),
        Book.Create("ISBN-402", "Місто", "Класика", 14),
        Book.Create("ISBN-403", "Тіні забутих предків", "Класика", 7)        // < 10 (дефіцит)
    };
}