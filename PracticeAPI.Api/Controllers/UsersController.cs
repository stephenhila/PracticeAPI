using MongoDB.Driver;
using PracticeAPI.Api.Models.DbModels;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Security;

namespace PracticeAPI.Api.Controllers
{
    public class UsersController : ApiController
    {
        // GET: Users
        [AllowAnonymous]
        [HttpPost]
        public async Task<IHttpActionResult> Login(UserModel model)
        {
            string constr = ConfigurationManager.AppSettings["connectionString"];
            var Client = new MongoClient(constr);
            var DB = Client.GetDatabase(ConfigurationManager.AppSettings["usersDbName"]);
            var collection = DB.GetCollection<UserModel>("Users");
            var cursor = await collection.FindAsync(x => x.Username == model.Username && x.Password == model.Password).ConfigureAwait(false);

            var userModel = await cursor.FirstOrDefaultAsync();

            if (userModel != null)
            {
                //FormsAuthentication.SetAuthCookie(userModel.Username, false);
                return Ok(userModel);
            }

            return NotFound();

            //TODO: update this into token-based authentication
        }
    }
}