using LibraryApp.Models;

namespace LibraryApp.Patterns.Observer;

public interface IBookObserver
{
    void Update(Book book);
}