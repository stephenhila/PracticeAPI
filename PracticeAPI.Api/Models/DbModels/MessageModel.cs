using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace PracticeAPI.Api.Models.DbModels
{
    public class MessageModel
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("Text")]
        public string Text { get; set; }

        [BsonElement("Username")]
        public string Username { get; set; }
    }
}