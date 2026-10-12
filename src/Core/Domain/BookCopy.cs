using Core.Dto;
namespace Core.Domain;

public sealed class BookCopy
{
    private bool _isIssued;

    public string Id { get; }
    public string Isbn { get; }
    public string Title { get; }
    public int Year { get; }
    public string? Author { get; }

    public bool IsIssued => _isIssued;

    private BookCopy(string id, string isbn, string title, int year, string? author, bool isIssued)
    {
        Id = id;
        Isbn = isbn;
        Title = title;
        Year = year;
        Author = author;
        _isIssued = isIssued;
    }

    // Фабричний метод, що перевіряє всі вхідні дані до виклику конструктора
    public static BookCopy Create(string id, string isbn, string title, int year, string? author = null, bool isIssued = false)
    {
        if (string.IsNullOrWhiteSpace(id))
            throw new ArgumentException("Ідентифікатор обов'язковий", nameof(id));

        if (string.IsNullOrWhiteSpace(isbn))
            throw new ArgumentException("ISBN не може бути порожнім", nameof(isbn));

        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("Назва книги не може бути порожньою", nameof(title));

        if (year < 1450 || year > 2026)
            throw new ArgumentOutOfRangeException(nameof(year), year, 
                "Рік видання має бути в межах від 1450 до 2026");

        return new BookCopy(
            id.Trim(), 
            isbn.Trim().ToUpperInvariant(), 
            title.Trim(), 
            year, 
            author?.Trim(), 
            isIssued);
    }

    // Методи зміни стану з бізнес-іменами
    public void Issue()
    {
        if (_isIssued)
            throw new InvalidOperationException($"Примірник '{Id}' уже виданий, повторна видача неможлива");

        _isIssued = true;
    }

    public void Return()
    {
        if (!_isIssued)
            throw new InvalidOperationException($"Примірник '{Id}' не був виданий, повернення неможливе");

        _isIssued = false;
    }
// Експорт у формат DTO
    public BookDto ToDto() => new(Id, Isbn, Title, Year, Author);

    // Відновлення з DTO з проходженням тих самих інваріантів
    public static BookCopy FromDto(BookDto dto) =>
        Create(dto.Id, dto.Isbn, dto.Title, dto.Year, dto.Author);

    public override string ToString() =>
        $"[{Id}] {Title} (ISBN: {Isbn}, {Year} р.) - {(IsIssued ? "ВИДАНО" : "В НАЯВНОСТІ")}";
}
