using Microsoft.Extensions.Options;
using MongoDB.Driver;

public class AuthorService
{
    private readonly IMongoCollection<Author> _authors;

    public AuthorService(IOptions<MongoDbSettings> settings)
    {
        var client = new MongoClient(settings.Value.ConnectionString);
        var database = client.GetDatabase(settings.Value.DatabaseName);
        _authors = database.GetCollection<Author>(settings.Value.AuthorsCollectionName);
    }

    public List<Author> Get() => _authors.Find(author => true).ToList();
    public Author Get(string id) => _authors.Find(author => author.Id == id).FirstOrDefault();
    public Author Create(Author author) { _authors.InsertOne(author); return author; }
    public void Update(string id, Author authorIn) => _authors.ReplaceOne(author => author.Id == id, authorIn);
    public void Remove(string id) => _authors.DeleteOne(author => author.Id == id);
}
