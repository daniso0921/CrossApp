namespace Core;

public sealed class Book
{
    public string Isbn { get; }
    public string Title { get; }
    public string Genre { get; }
    public int Quantity { get; }

    private Book(string isbn, string title, string genre, int quantity)
    {
        Isbn = isbn;
        Title = title;
        Genre = genre;
        Quantity = quantity;
    }

    public static Book Create(string isbn, string title, string genre, int quantity)
    {
        return new Book(isbn, title, genre, quantity);
    }
}