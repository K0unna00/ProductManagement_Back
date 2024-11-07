using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace TestProj.Core.Entities;

public class Product
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    [BsonElement("name")]
    public string Name { get; set; }

    [BsonElement("description")]
    public string Description { get; set; }

    [BsonElement("price")]
    public decimal Price { get; set; }

    [BsonElement("createdAt")]
    public DateTime CreatedAt { get; set; }
}
