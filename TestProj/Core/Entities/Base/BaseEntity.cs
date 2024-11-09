using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace TestProj.Core.Entities.Base;

public abstract class BaseEntity
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    [BsonElement("createdAt")]
    public DateTime CreatedAt { get; set; }
}
