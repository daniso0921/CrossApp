namespace Core.Domain;

public sealed class Loan
{
    public string Id { get; }
    public string ReaderId { get; }
    public BookCopy Copy { get; }
    public DateTime IssuedOn { get; }
    public DateTime? ReturnedOn { get; private set; }

    public bool IsActive => ReturnedOn is null;

    // Приватний конструктор 
    private Loan(string id, string readerId, BookCopy copy, DateTime issuedOn)
    {
        Id = id;
        ReaderId = readerId;
        Copy = copy;
        IssuedOn = issuedOn;
        ReturnedOn = null;
    }

    // Фабричний метод з перевіркою інваріантів 
    public static Loan Open(string id, string readerId, BookCopy copy, DateTime issuedOn)
    {
        if (string.IsNullOrWhiteSpace(id))
            throw new ArgumentException("Ідентифікатор видачі обов'язковий", nameof(id));

        if (string.IsNullOrWhiteSpace(readerId))
            throw new ArgumentException("Ідентифікатор читача обов'язковий", nameof(readerId));

        if (copy is null)
            throw new ArgumentNullException(nameof(copy), "Примірник книги обов'язковий для видачі");

        if (copy.IsIssued)
            throw new InvalidOperationException($"Примірник '{copy.Id}' уже зайнятий, оформлення неможливе");

        // Змінюємо стан примірника на виданий
        copy.Issue();

        return new Loan(id.Trim(), readerId.Trim(), copy, issuedOn);
    }

    // Бізнес-метод закриття видачі 
    public void Close(DateTime returnedOn)
    {
        if (!IsActive)
            throw new InvalidOperationException($"Видача '{Id}' уже була закрита раніше");

        if (returnedOn < IssuedOn)
            throw new ArgumentOutOfRangeException(nameof(returnedOn), returnedOn,
                $"Дата повернення ({returnedOn:yyyy-MM-dd}) не може бути раніше дати видачі ({IssuedOn:yyyy-MM-dd})");

        ReturnedOn = returnedOn;
        Copy.Return();
    }

    public override string ToString() =>
        $"Видача [{Id}]: Читач {ReaderId} -> \"{Copy.Title}\" | Статус: {(IsActive ? "Активна" : $"Повернено {ReturnedOn:yyyy-MM-dd}")}";
}