namespace LibraryApp.Models;

public sealed class Book
{
    public string Isbn { get; }
    public string Title { get; }
    public string Author { get; }
    public int Year { get; }

    public Book(string isbn, string title, string author, int year)
    {
        if (string.IsNullOrWhiteSpace(isbn)) throw new ArgumentException("ISBN обязателен", nameof(isbn));
        if (string.IsNullOrWhiteSpace(title)) throw new ArgumentException("Название обязательно", nameof(title));
        Isbn = isbn; Title = title; Author = author; Year = year;
    }

    public override string ToString() => $"«{Title}» — {Author}, {Year} (ISBN {Isbn})";
}