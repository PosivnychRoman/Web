using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

public class Book
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    public string? Title { get; set; }
    public string? AuthorId { get; set; }
    public string? GenreId { get; set; }
}
