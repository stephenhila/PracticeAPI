using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace PracticeAPI.Web.Models.DbModels
{
    public class MessageModel
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("Text")]
        public string Text { get; set; }

        [BsonElement("UserId")]
        public string UserId { get; set; }
    }
}