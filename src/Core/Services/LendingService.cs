using Core.Abstractions;
using Core.Domain;

namespace Core.Services;

public sealed class LendingService(IBookStore store)
{
    private readonly IBookStore _store = store ?? throw new ArgumentNullException(nameof(store));

    // Додавання нової книги до каталогу
    public BookCopy AddBook(string isbn, string title, int year, string? author = null)
    {
        var id = $"BC-{Guid.NewGuid().ToString("N")[..8].ToUpperInvariant()}";
        var book = BookCopy.Create(id, isbn, title, year, author);
        _store.Add(book);
        return book;
    }

    // Бізнес-операція: видача примірника
    public void IssueCopy(string id)
    {
        var book = _store.GetById(id)
            ?? throw new InvalidOperationException($"Немає запису книги з id={id}.");

        book.Issue();
        _store.Update(book);
    }

    // Бізнес-операція: повернення примірника
    public void ReturnCopy(string id)
    {
        var book = _store.GetById(id)
            ?? throw new InvalidOperationException($"Немає запису книги з id={id}.");

        book.Return();
        _store.Update(book);
    }

    // Отримання повного списку
    public IReadOnlyList<BookCopy> All() => _store.List();

    // Пошук за ідентифікатором
    public BookCopy? Find(string id) => _store.GetById(id);
}