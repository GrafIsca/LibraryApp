using LibraryApp.Patterns.Observer;

namespace LibraryApp.Models;

/// <summary>
/// Читатель — конкретный подписчик (Observer). Реагирует на поступление книг.
/// </summary>
public sealed class Reader : IBookObserver
{
    public string Id { get; }
    public string Name { get; }
    public string? FavoriteAuthor { get; }

    public Reader(string id, string name, string? favoriteAuthor = null)
    {
        if (string.IsNullOrWhiteSpace(id)) throw new ArgumentException("Id обязателен", nameof(id));
        Id = id; Name = name; FavoriteAuthor = favoriteAuthor;
    }

    /// <summary>Реакция на событие «в библиотеке появилась новая книга».</summary>
    public void Update(Book book)
    {
        var match = FavoriteAuthor is not null &&
                    book.Author.Equals(FavoriteAuthor, StringComparison.OrdinalIgnoreCase);
        Console.WriteLine($"  [читатель {Name}] получил уведомление: поступила {book}" +
                          (match ? " — это же мой любимый автор!" : ""));
    }

    public override string ToString() => $"{Name} (id={Id})";
}