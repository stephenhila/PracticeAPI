using System.Configuration;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using MongoDB.Driver;
using PracticeAPI.Api.Authentication;
using PracticeAPI.Api.Models;
using PracticeAPI.Api.Models.DbModels;

namespace PracticeAPI.Api
{
    // Configure the application user manager used in this application. UserManager is defined in ASP.NET Identity and is used by the application.

    public class ApplicationUserManager : UserManager<ApplicationUser>
    {
        public ApplicationUserManager(IUserStore<ApplicationUser> store)
            : base(store)
        {
        }

        public static ApplicationUserManager Create(IdentityFactoryOptions<ApplicationUserManager> options, IOwinContext context)
        {
            var manager = new ApplicationUserManager(new MongoDBUserStore(ConfigurationManager.AppSettings["connectionString"], ConfigurationManager.AppSettings["usersDbName"]));
            // Configure validation logic for usernames
            manager.UserValidator = new UserValidator<ApplicationUser>(manager)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = true
            };
            // Configure validation logic for passwords
            manager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 6,
                RequireNonLetterOrDigit = true,
                RequireDigit = true,
                RequireLowercase = true,
                RequireUppercase = true,
            };
            var dataProtectionProvider = options.DataProtectionProvider;
            if (dataProtectionProvider != null)
            {
                manager.UserTokenProvider = new DataProtectorTokenProvider<ApplicationUser>(dataProtectionProvider.Create("ASP.NET Identity"));
            }
            return manager;
        }

        public override async Task<ApplicationUser> FindAsync(string userName, string password)
        {
            string constr = ConfigurationManager.AppSettings["connectionString"];
            var Client = new MongoClient(constr);
            var DB = Client.GetDatabase(ConfigurationManager.AppSettings["usersDbName"]);
            var collection = DB.GetCollection<UserModel>("Users");
            var cursor = await collection.FindAsync(x => x.Username == userName && x.Password == password).ConfigureAwait(false);
            var userModel = await cursor.FirstOrDefaultAsync();

            if (userModel != null)
            {
                return new ApplicationUser { Id = userModel.Id, UserName = userModel.Username };
            }

            return null;
        }
    }
}
