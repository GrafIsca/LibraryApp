using LibraryApp.Models;

namespace LibraryApp.Patterns.Subsystems;

/// <summary>
/// Подсистема «учёт читателей».
/// </summary>
public sealed class ReaderRegistry
{
    private readonly Dictionary<string, Reader> _readers = new();
    private readonly HashSet<string> _blocked = new();

    public void Register(Reader reader)
    {
        if (_readers.ContainsKey(reader.Id))
            throw new InvalidOperationException($"Читатель {reader.Id} уже зарегистрирован");
        _readers[reader.Id] = reader;
        Console.WriteLine($"[учёт] зарегистрирован читатель {reader}");
    }

    public Reader Get(string id) =>
        _readers.TryGetValue(id, out var r)
            ? r
            : throw new KeyNotFoundException($"Читатель {id} не найден");

    public bool IsBlocked(string id) => _blocked.Contains(id);
    public void Block(string id) => _blocked.Add(id);
}