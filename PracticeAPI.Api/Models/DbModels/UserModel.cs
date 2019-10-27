using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace PracticeAPI.Api.Models.DbModels
{
    public class UserModel
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("Username")]
        public string Username { get; set; }

        public string Password { get; set; }
    }
}