using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using TestProj.Core.Entities.Base;

namespace TestProj.Core.Entities;

public class Product : BaseEntity
{
    [BsonElement("name")]
    public string Name { get; set; }

    [BsonElement("description")]
    public string Description { get; set; }

    [BsonElement("price")]
    public decimal Price { get; set; }

    [BsonElement("imgName")]
    public string ImgName { get; set; }
}
