namespace LibraryApp.Patterns.Observer;

public interface IBookObservable
{
    void Subscribe(IBookObserver observer);
    void Unsubscribe(IBookObserver observer);
}