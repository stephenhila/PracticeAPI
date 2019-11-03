using MongoDB.Driver;
using PracticeAPI.Api.Models.DbModels;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Threading.Tasks;
using System.Web.Http;

namespace PracticeAPI.Api.Controllers
{
    public class MessagesController : ApiController
    {
        [HttpGet]
        public async Task<IEnumerable<MessageModel>> Get(int count)
        {
            string constr = ConfigurationManager.AppSettings["connectionString"];
            var Client = new MongoClient(constr);
            var DB = Client.GetDatabase(ConfigurationManager.AppSettings["dbName"]);
            var collection = DB.GetCollection<MessageModel>("Messages");
            return await collection.Find(x => true).Limit(count).ToListAsync();
        }

        [HttpPost]
        public async Task<IHttpActionResult> Post(MessageModel message)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid data posted!");
            try
            {
                string constr = ConfigurationManager.AppSettings["connectionString"];
                var Client = new MongoClient(constr);
                var DB = Client.GetDatabase(ConfigurationManager.AppSettings["dbName"]);
                var collection = DB.GetCollection<MessageModel>("Messages");
                await collection.InsertOneAsync(message);

                return Ok();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}
