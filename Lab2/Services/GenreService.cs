using Microsoft.Extensions.Options;
using MongoDB.Driver;

public class GenreService
{
    private readonly IMongoCollection<Genre> _genres;

    public GenreService(IOptions<MongoDbSettings> settings)
    {
        var client = new MongoClient(settings.Value.ConnectionString);
        var database = client.GetDatabase(settings.Value.DatabaseName);
        _genres = database.GetCollection<Genre>(settings.Value.GenresCollectionName);
    }

    public List<Genre> Get() => _genres.Find(g => true).ToList();
    public Genre Get(string id) => _genres.Find(g => g.Id == id).FirstOrDefault();
    public Genre Create(Genre genre) { _genres.InsertOne(genre); return genre; }
    public void Update(string id, Genre genreIn) => _genres.ReplaceOne(g => g.Id == id, genreIn);
    public void Remove(string id) => _genres.DeleteOne(g => g.Id == id);
}
