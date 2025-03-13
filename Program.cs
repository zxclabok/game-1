using System;
using System.Collections.Generic;
using System.IO;

class Bookкниги
{
    public string Title { get; private set; }
    public string Author { get; private set; }
    public string Status { get; private set; }

    public Book(string title, string author, string status = "доступна")
    {
        Title = title;
        Author = author;
        Status = status;
    }

    public override string ToString()
    {
        return $"{Title} ({Author}) - {Status}";
    }
}

class User
{
    public string Name { get; private set; }

    public User(string name)
    {
        Name = name;
    }

    public override string ToString()
    {
        return Name;
    }
}

class Librarian : User
{
    public Librarian(string name) : base(name) { }

    public void AddBook(Library library, string title, string author)
    {
        var book = new Book(title, author);
        library.AddBook(book);
        Console.WriteLine($"Книга '{title}' успешно добавлена.");
    }

    public void RemoveBook(Library library, string title)
    {
        if (library.RemoveBook(title))
        {
            Console.WriteLine($"Книга '{title}' успешно удалена.");
        }
        else
        {
            Console.WriteLine($"Книга '{title}' не найдена.");
        }
    }

    public void RegisterUser(Library library, string name)
    {
        var user = new User(name);
        library.RegisterUser(user);
        Console.WriteLine($"Пользователь '{name}' успешно зарегистрирован.");
    }

    public void ViewAllBooks(Library library)
    {
        Console.WriteLine("Список всех книг:");
        foreach (var book in library.Books)
        {
            Console.WriteLine(book);
        }
    }

    public void ViewAllUsers(Library library)
    {
        Console.WriteLine("Список всех пользователей:");
        foreach (var user in library.Users)
        {
            Console.WriteLine(user);
        }
    }
}

class Library
{
    public List<Book> Books { get; private set; } = new List<Book>();
    public List<User> Users { get; private set; } = new List<User>();

    public void LoadData()
    {
        if (File.Exists("books.txt"))
        {
            foreach (var line in File.ReadAllLines("books.txt"))
            {
                var parts = line.Split('|');
                var book = new Book(parts[0], parts[1], parts[2]);
                Books.Add(book);
            }
        }

        if (File.Exists("users.txt"))
        {
            foreach (var line in File.ReadAllLines("users.txt"))
            {
                var user = new User(line);
                Users.Add(user);
            }
        }
    }

    public void SaveData()
    {
        using (var writer = new StreamWriter("books.txt"))
        {
            foreach (var book in Books)
            {
                writer.WriteLine($"{book.Title}|{book.Author}|{book.Status}");
            }
        }

        using (var writer = new StreamWriter("users.txt"))
        {
            foreach (var user in Users)
            {
                writer.WriteLine(user.Name);
            }
        }
    }

    public void AddBook(Book book)
    {
        Books.Add(book);
    }

    public bool RemoveBook(string title)
    {
        var book = Books.Find(b => b.Title == title);
        if (book != null)
        {
            Books.Remove(book);
            return true;
        }
        return false;
    }
    public void RegisterUser(User user)
    {
        Users.Add(user);
    }
}
class Program
{
    static void Main()
    {
        var library = new Library();
        library.LoadData();

        Console.WriteLine("Добро пожаловать в библиотеку!");
        Console.Write("Выберите роль (библиотекарь/пользователь): ");
        var role = Console.ReadLine().Trim().ToLower();

        if (role == "библиотекарь")
        {
            Console.Write("Введите ваше имя: ");
            var librarianName = Console.ReadLine();
            var librarian = new Librarian(librarianName);

            while (true)
            {
                Console.WriteLine("\nМеню библиотекаря:");
                Console.WriteLine("1. Добавить новую книгу");
                Console.WriteLine("2. Удалить книгу из системы");
                Console.WriteLine("3. Зарегистрировать нового пользователя");
                Console.WriteLine("4. Просмотреть список всех пользователей");
                Console.WriteLine("5. Просмотреть список всех книг");
                Console.WriteLine("6. Выход");

                Console.Write("Выберите действие: ");
                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        Console.Write("Введите название книги: ");
                        var title = Console.ReadLine();
                        Console.Write("Введите автора книги: ");
                        var author = Console.ReadLine();
                        librarian.AddBook(library, title, author);
                        break;

                    case "2":
                        Console.Write("Введите название книги для удаления: ");
                        var bookTitle = Console.ReadLine();
                        librarian.RemoveBook(library, bookTitle);
                        break;

                    case "3":
                        Console.Write("Введите имя нового пользователя: ");
                        var userName = Console.ReadLine();
                        librarian.RegisterUser(library, userName);
                        break;

                    case "4":
                        librarian.ViewAllUsers(library);
                        break;

                    case "5":
                        librarian.ViewAllBooks(library);
                        break;

                    case "6":
                        library.SaveData();
                        Console.WriteLine("Данные сохранены. До свидания!");
                        return;

                    default:
                        Console.WriteLine("Неверный выбор. Попробуйте снова.");
                        break;
                }
            }
        }
        else
        {
            Console.WriteLine("Роль 'пользователь' пока не реализована.");
        }
    }
}