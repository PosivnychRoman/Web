﻿using Microsoft.Extensions.Options;
using MongoDB.Driver;

public class BookService
{
    private readonly IMongoCollection<Book> _books;

    public BookService(IOptions<MongoDbSettings> settings)
    {
        var client = new MongoClient(settings.Value.ConnectionString);
        var database = client.GetDatabase(settings.Value.DatabaseName);
        _books = database.GetCollection<Book>(settings.Value.BooksCollectionName);
    }

    public List<Book> Get() => _books.Find(book => true).ToList();
    public Book Get(string id) => _books.Find(book => book.Id == id).FirstOrDefault();
    public Book Create(Book book) { _books.InsertOne(book); return book; }
    public void Update(string id, Book bookIn) => _books.ReplaceOne(book => book.Id == id, bookIn);
    public void Remove(string id) => _books.DeleteOne(book => book.Id == id);
}
