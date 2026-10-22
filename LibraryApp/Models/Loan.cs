namespace LibraryApp.Models;

public sealed class Loan
{
    public string Isbn { get; }
    public string ReaderId { get; }
    public DateTime IssuedAt { get; }

    public Loan(string isbn, string readerId)
    {
        Isbn = isbn; ReaderId = readerId; IssuedAt = DateTime.Now;
    }

    public override string ToString() =>
        $"Выдача ISBN {Isbn} читателю {ReaderId} от {IssuedAt:HH:mm:ss}";
}