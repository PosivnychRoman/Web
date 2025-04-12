using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text;
using System;

public class Book
{
    public string? Id { get; set; }
    public string? Title { get; set; }
    public string? AuthorId { get; set; }
    public string? GenreId { get; set; }
}

class Program
{
    static HttpClient client = new HttpClient
    {
        BaseAddress = new Uri("https://localhost:7160/api/")
    };

    static async Task Main()
    {
        while (true)
        {
            Console.WriteLine("\n--- CRUD Menu ---");
            Console.WriteLine("1. Get all books");
            Console.WriteLine("2. Add a book");
            Console.WriteLine("3. Update a book");
            Console.WriteLine("4. Delete a book");
            Console.WriteLine("0. Exit");
            Console.Write("Your choice: ");
            string? choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    await GetAllBooks();
                    break;
                case "2":
                    await CreateBook();
                    break;
                case "3":
                    await UpdateBook();
                    break;
                case "4":
                    await DeleteBook();
                    break;
                case "0":
                    return;
                default:
                    Console.WriteLine("Invalid choice.");
                    break;
            }
        }
    }

    static async Task GetAllBooks()
    {
        var books = await client.GetFromJsonAsync<List<Book>>("books");
        foreach (var book in books!)
        {
            Console.WriteLine($"📚 ID: {book.Id}, Title: {book.Title}, AuthorId: {book.AuthorId}, GenreId: {book.GenreId}");
        }
    }

    static async Task CreateBook()
    {
        Console.Write("Enter book title: ");
        string? title = Console.ReadLine();

        Console.Write("Enter AuthorId: ");
        string? authorId = Console.ReadLine();

        Console.Write("Enter GenreId: ");
        string? genreId = Console.ReadLine();

        var book = new Book { Title = title, AuthorId = authorId, GenreId = genreId };
        var response = await client.PostAsJsonAsync("books", book);
        Console.WriteLine(response.IsSuccessStatusCode ? "✅ Book added!" : $"❌ Error: {response.StatusCode}");
    }

    static async Task UpdateBook()
    {
        Console.Write("Enter ID of the book to update: ");
        string? id = Console.ReadLine();

        Console.Write("New book title: ");
        string? title = Console.ReadLine();

        Console.Write("New AuthorId: ");
        string? authorId = Console.ReadLine();

        Console.Write("New GenreId: ");
        string? genreId = Console.ReadLine();

        var book = new Book { Id = id, Title = title, AuthorId = authorId, GenreId = genreId };
        var response = await client.PutAsJsonAsync($"books/{id}", book);
        Console.WriteLine(response.IsSuccessStatusCode ? "✅ Book updated!" : $"❌ Error: {response.StatusCode}");
    }

    static async Task DeleteBook()
    {
        Console.Write("Enter ID of the book to delete: ");
        string? id = Console.ReadLine();

        var response = await client.DeleteAsync($"books/{id}");
        Console.WriteLine(response.IsSuccessStatusCode ? "✅ Book deleted!" : $"❌ Error: {response.StatusCode}");
    }
}
