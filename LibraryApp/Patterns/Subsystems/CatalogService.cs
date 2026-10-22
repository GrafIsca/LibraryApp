using LibraryApp.Models;
using LibraryApp.Patterns.Observer;

namespace LibraryApp.Patterns.Subsystems;

/// <summary>
/// Подсистема «каталог»: хранит фонд книг, умеет искать и уведомляет о новых поступлениях.
/// Реализует издателя (Subject) для паттерна Observer.
/// </summary>
public sealed class CatalogService : IBookObservable
{
    private readonly Dictionary<string, Book> _books = new();
    private readonly List<IBookObserver> _observers = new();

    public void AddBook(Book book)
    {
        if (_books.ContainsKey(book.Isbn))
            throw new InvalidOperationException($"Книга с ISBN {book.Isbn} уже в каталоге");

        _books[book.Isbn] = book;
        Console.WriteLine($"[каталог] добавлена книга {book}");
        Notify(book);
    }

    public Book Get(string isbn) =>
        _books.TryGetValue(isbn, out var b)
            ? b
            : throw new KeyNotFoundException($"Книга с ISBN {isbn} не найдена");

    public List<Book> FindByAuthor(string author) =>
        _books.Values
              .Where(b => b.Author.Equals(author, StringComparison.OrdinalIgnoreCase))
              .ToList();

    public List<Book> FindByTitle(string titleFragment) =>
        _books.Values
              .Where(b => b.Title.Contains(titleFragment, StringComparison.OrdinalIgnoreCase))
              .ToList();

    public void Subscribe(IBookObserver observer)
    {
        ArgumentNullException.ThrowIfNull(observer);
        if (!_observers.Contains(observer)) _observers.Add(observer);
        Console.WriteLine($"[каталог] оформлена подписка");
    }

    public void Unsubscribe(IBookObserver observer)
    {
        if (_observers.Remove(observer))
            Console.WriteLine($"[каталог] подписка отменена");
    }

    // Важно: обход по копии, чтобы отписка во время Update не сломала перечисление
    private void Notify(Book book)
    {
        foreach (var observer in _observers.ToArray())
            observer.Update(book);
    }
}