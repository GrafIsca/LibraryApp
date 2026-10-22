using LibraryApp.Models;

namespace LibraryApp.Patterns.Subsystems;

/// <summary>
/// Подсистема «склад»: наличие экземпляров, брони, выдача, возврат.
/// Не знает про читателей — работает по readerId.
/// </summary>
public sealed class InventoryService
{
    private readonly Dictionary<string, int> _stock = new();
    private readonly Dictionary<string, int> _reserved = new();
    private readonly List<Loan> _active = new();

    public void SetStock(string isbn, int count)
    {
        if (count < 0) throw new ArgumentOutOfRangeException(nameof(count));
        _stock[isbn] = count;
        Console.WriteLine($"[склад] на складе ISBN {isbn}: {count} экз.");
    }

    public bool TryReserve(string isbn)
    {
        var available = GetAvailable(isbn);
        if (available <= 0)
        {
            Console.WriteLine($"[склад] ISBN {isbn}: нет свободных экземпляров");
            return false;
        }
        _reserved[isbn] = GetReserved(isbn) + 1;
        Console.WriteLine($"[склад] ISBN {isbn}: забронировано (свободно {GetAvailable(isbn)})");
        return true;
    }

    public Loan Issue(string isbn, string readerId)
    {
        if (GetReserved(isbn) <= 0)
            throw new InvalidOperationException($"ISBN {isbn}: нет брони для выдачи");
        _reserved[isbn] = GetReserved(isbn) - 1;
        var loan = new Loan(isbn, readerId);
        _active.Add(loan);
        Console.WriteLine($"[склад] {loan}");
        return loan;
    }

    public void Return(Loan loan)
    {
        if (!_active.Remove(loan))
            throw new InvalidOperationException("Выдача не найдена");
        Console.WriteLine($"[склад] возврат: {loan}");
    }

    public int GetAvailable(string isbn) => _stock.TryGetValue(isbn, out var s) ? s - GetReserved(isbn) : 0;
    private int GetReserved(string isbn) => _reserved.TryGetValue(isbn, out var r) ? r : 0;
}