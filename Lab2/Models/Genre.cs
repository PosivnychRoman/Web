using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

public class Genre
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    public string? Name { get; set; }
}
