using MongoDB.Driver;
using PracticeAPI.Api.Models.DbModels;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace PracticeAPI.Api.Controllers
{
    public class RegistrationController : ApiController
    {
        [AllowAnonymous]
        [HttpGet]
        public async Task<bool> IsUsernameAvailable(string username)
        {
            string constr = ConfigurationManager.AppSettings["connectionString"];
            var Client = new MongoClient(constr);
            var DB = Client.GetDatabase(ConfigurationManager.AppSettings["usersDbName"]);
            var collection = DB.GetCollection<UserModel>("Users");
            var cursor = await collection.FindAsync(x => x.Username == username).ConfigureAwait(false);
            var userModel = await cursor.FirstOrDefaultAsync();

            if (userModel == null)
            {
                return true;
            }

            return false;
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IHttpActionResult> PostNewUser(UserModel user)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid data posted!");

            try
            {
                string constr = ConfigurationManager.AppSettings["connectionString"];
                var Client = new MongoClient(constr);
                var DB = Client.GetDatabase(ConfigurationManager.AppSettings["usersDbName"]);
                var collection = DB.GetCollection<UserModel>("Users");
                await collection.InsertOneAsync(user);

                return Ok();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}