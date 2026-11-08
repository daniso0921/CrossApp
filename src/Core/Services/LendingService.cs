using Core.Abstractions;
using Core.Domain;
using Core.Events;
using Core.Notifications;

namespace Core.Services;

public sealed class LendingService(IBookStore store, IEnumerable<INotificationSink>? sinks = null)
{
    private readonly IBookStore _store = store ?? throw new ArgumentNullException(nameof(store));
    private readonly IEnumerable<INotificationSink> _sinks = sinks ?? [];

    // Подія 6-ї лабораторної
    public event EventHandler<CatalogChangedEventArgs>? Changed;

    private void RaiseChanged(BookCopy book, CatalogChangeKind kind, int delta)
    {
        // 1. Виклик події
        Changed?.Invoke(this, new CatalogChangedEventArgs(book.Id, kind, delta, book.IsIssued));

        // 2. Додаткове завдання 1: сповіщення через INotificationSink
        var msg = $"{DateTimeOffset.Now:HH:mm:ss} {kind,-8} {book.Id} | дельта {delta} | видано: {book.IsIssued}";
        foreach (var sink in _sinks)
        {
            sink.Notify(msg);
        }
    }

    // Додавання нової книги до каталогу
    public BookCopy AddBook(string isbn, string title, int year, string? author = null)
    {
        var id = $"BC-{Guid.NewGuid().ToString("N")[..8].ToUpperInvariant()}";
        var book = BookCopy.Create(id, isbn, title, year, author);
        _store.Add(book);
        RaiseChanged(book, CatalogChangeKind.Added, +1);
        return book;
    }

    // Бізнес-операція: видача примірника
    public void IssueCopy(string id)
    {
        var book = _store.GetById(id)
            ?? throw new InvalidOperationException($"Немає запису книги з id={id}.");

        book.Issue();
        _store.Update(book);
        RaiseChanged(book, CatalogChangeKind.Issued, 0);
    }

    // Бізнес-операція: повернення примірника
    public void ReturnCopy(string id)
    {
        var book = _store.GetById(id)
            ?? throw new InvalidOperationException($"Немає запису книги з id={id}.");

        book.Return();
        _store.Update(book);
        RaiseChanged(book, CatalogChangeKind.Returned, 0);
    }

    // Отримання повного списку
    public IReadOnlyList<BookCopy> All() => _store.List();

    // Пошук за ідентифікатором
    public BookCopy? Find(string id) => _store.GetById(id);

    
    
    public IReadOnlyList<BookCopy> Search(Func<BookCopy, bool> predicate) =>
        _store.List().Where(predicate).ToList();

    // Додаткове завдання 2: Імпорт із використанням Action<string> для звіту про пропущені рядки
    public int ImportBooks(IEnumerable<string> lines, Action<string> onSkippedLine)
    {
        int imported = 0;
        foreach (var line in lines)
        {
            var parts = line.Split(';');
            if (parts.Length < 3 || !int.TryParse(parts[2].Trim(), out int year))
            {
                onSkippedLine?.Invoke($"Некоректний формат рядка: \"{line}\"");
                continue;
            }

            try
            {
                AddBook(parts[0].Trim(), parts[1].Trim(), year, parts.Length > 3 ? parts[3].Trim() : null);
                imported++;
            }
            catch (Exception ex)
            {
                onSkippedLine?.Invoke($"Порушення інваріанту в рядку \"{line}\": {ex.Message}");
            }
        }
        return imported;
    }
}