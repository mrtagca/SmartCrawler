using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using SmartCrawler.DbAccess.MongoDB.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartCrawler.DbAccess.MongoDB.Entities
{
    public abstract class MongoDbEntity : IEntity<string>
    {
        [BsonRepresentation(BsonType.ObjectId)]
        [BsonId]
        [BsonElement(Order = 0)]
        public string Id { get; set; }

        [BsonRepresentation(BsonType.DateTime)]
        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        [BsonElement(Order = 101)]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
