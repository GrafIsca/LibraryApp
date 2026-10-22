using System.Globalization;
using LibraryApp.Models;
using LibraryApp.Patterns;
using LibraryApp.Patterns.Subsystems;

CultureInfo.DefaultThreadCurrentCulture = new CultureInfo("ru-RU");

var facade = new LibraryFacade(
    new CatalogService(),
    new InventoryService(),
    new ReaderRegistry());

// ============ ДЕМО 1: Facade — единый интерфейс ============
Console.WriteLine("=== ДЕМО 1. Регистрация, каталог, поиск (через фасад) ===");
var anna = new Reader("r-001", "Анна", favoriteAuthor: "Стругацкий");
var boris = new Reader("r-002", "Борис");

facade.RegisterReader(anna);
facade.RegisterReader(boris);

var book1 = new Book("978-5-17-0001", "Пикник на обочине", "Стругацкий", 1972);
var book2 = new Book("978-5-17-0002", "Трудно быть богом", "Стругацкий", 1964);
var book3 = new Book("978-5-17-0003", "Метро 2033", "Глуховский", 2005);

facade.AddBook(book1);
facade.AddBook(book2);
facade.AddBook(book3);

Console.WriteLine("\n-- поиск по автору «Стругацкий» --");
foreach (var b in facade.SearchByAuthor("Стругацкий"))
    Console.WriteLine("   " + b);

// ============ ДЕМО 2: Observer — подписки и уведомления ============
Console.WriteLine("\n=== ДЕМО 2. Подписка читателей на поступления ===");
facade.Subscribe(anna);
facade.Subscribe(boris);

Console.WriteLine("\n-- в библиотеку поступает новая книга --");
var newBook = new Book("978-5-17-0004", "Обитаемый остров", "Стругацкий", 1969);
facade.AddBook(newBook);

Console.WriteLine("\n-- Борис отписался --");
facade.Unsubscribe(boris);

Console.WriteLine("\n-- ещё одна новая книга --");
facade.AddBook(new Book("978-5-17-0005", "Понедельник начинается в субботу", "Стругацкий", 1965));

// ============ ДЕМО 3: полный цикл «бронь → выдача → возврат» ============
Console.WriteLine("\n=== ДЕМО 3. Бронь → выдача → возврат (через фасад) ===");

// увеличим фонд по book1 до 2 экземпляров
var inventory = (InventoryService)typeof(LibraryFacade)
    .GetField("_inventory", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)!
    .GetValue(facade)!;
inventory.SetStock(book1.Isbn, 2);
inventory.SetStock(book3.Isbn, 1);

Console.WriteLine("\n-- Анна бронирует book1 --");
facade.Reserve(book1.Isbn, anna.Id);

Console.WriteLine("\n-- Анна забирает бронь (выдача) --");
var loan = facade.Checkout(book1.Isbn, anna.Id);

Console.WriteLine("\n-- Анна возвращает книгу --");
facade.Return(loan);

Console.WriteLine("\n-- Борис пытается взять book3 без брони --");
try { facade.Checkout(book3.Isbn, boris.Id); }
catch (Exception ex) { Console.WriteLine($"[ошибка] {ex.Message}"); }

Console.WriteLine("\nГотово. Нажмите Enter для выхода.");
Console.ReadLine();