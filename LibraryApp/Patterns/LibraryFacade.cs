using LibraryApp.Models;
using LibraryApp.Patterns.Observer;
using LibraryApp.Patterns.Subsystems;

namespace LibraryApp.Patterns;

/// <summary>
/// Единственная точка входа для клиента. Скрывает три подсистемы
/// и координирует их. Делегирует подписку каталогу (Observer).
/// </summary>
public sealed class LibraryFacade : IBookObservable
{
    private readonly CatalogService _catalog;
    private readonly InventoryService _inventory;
    private readonly ReaderRegistry _registry;

    public LibraryFacade(
        CatalogService catalog,
        InventoryService inventory,
        ReaderRegistry registry)
    {
        _catalog = catalog;
        _inventory = inventory;
        _registry = registry;
    }

    // ---- операции для читателя ----

    public void RegisterReader(Reader reader) => _registry.Register(reader);

    public void AddBook(Book book)
    {
        _catalog.AddBook(book);
        _inventory.SetStock(book.Isbn, 0);
    }

    public List<Book> SearchByAuthor(string author) => _catalog.FindByAuthor(author);
    public List<Book> SearchByTitle(string title) => _catalog.FindByTitle(title);

    /// <summary>Бронь = проверить читателя → взять на складе экземпляр в резерв.</summary>
    public bool Reserve(string isbn, string readerId)
    {
        var reader = _registry.Get(readerId);
        if (_registry.IsBlocked(readerId))
        {
            Console.WriteLine($"[фасад] читатель {reader} заблокирован, бронь отклонена");
            return false;
        }
        _catalog.Get(isbn); // проверка, что книга есть в каталоге
        var ok = _inventory.TryReserve(isbn);
        if (ok) Console.WriteLine($"[фасад] бронь оформлена: {reader} → ISBN {isbn}");
        return ok;
    }

    /// <summary>Выдача = проверить читателя → забрать бронь со склада.</summary>
    public Loan Checkout(string isbn, string readerId)
    {
        var reader = _registry.Get(readerId);
        if (_registry.IsBlocked(readerId))
            throw new InvalidOperationException($"Читатель {reader} заблокирован");
        return _inventory.Issue(isbn, readerId);
    }

    public void Return(Loan loan) => _inventory.Return(loan);

    // ---- Observer: делегируем каталогу ----

    public void Subscribe(IBookObserver observer) => _catalog.Subscribe(observer);
    public void Unsubscribe(IBookObserver observer) => _catalog.Unsubscribe(observer);
}