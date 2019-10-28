using Microsoft.AspNet.Identity;
using MongoDB.Driver;
using PracticeAPI.Api.Models;
using PracticeAPI.Api.Models.DbModels;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace PracticeAPI.Api.Authentication
{
    public class MongoDBUserStore : IUserStore<ApplicationUser>
    {
        public string ConnectionString { get; }
        public string DatabaseName { get; }

        public MongoDBUserStore(string connectionString, string databaseName)
        {
            ConnectionString = connectionString;
            DatabaseName = databaseName;
        }

        public Task CreateAsync(ApplicationUser user)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(ApplicationUser user)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            //throw new NotImplementedException();
        }

        public async Task<ApplicationUser> FindByIdAsync(string userId)
        {
            string constr = ConfigurationManager.AppSettings["connectionString"];
            var Client = new MongoClient(constr);
            var DB = Client.GetDatabase(ConfigurationManager.AppSettings["usersDbName"]);
            var collection = DB.GetCollection<UserModel>("Users");
            var cursor = await collection.FindAsync(x => x.Id == userId).ConfigureAwait(false);
            var userModel = await cursor.FirstOrDefaultAsync();

            if (userModel != null)
            {
                return new ApplicationUser { Id = userModel.Id, UserName = userModel.Username };
            }

            return null;
        }

        public async Task<ApplicationUser> FindByNameAsync(string userName)
        {
            string constr = ConfigurationManager.AppSettings["connectionString"];
            var Client = new MongoClient(constr);
            var DB = Client.GetDatabase(ConfigurationManager.AppSettings["usersDbName"]);
            var collection = DB.GetCollection<UserModel>("Users");
            var cursor = await collection.FindAsync(x => x.Username == userName).ConfigureAwait(false);
            var userModel = await cursor.FirstOrDefaultAsync();

            if (userModel != null)
            {
                return new ApplicationUser { Id = userModel.Id, UserName = userModel.Username };
            }

            return null;
        }

        public Task UpdateAsync(ApplicationUser user)
        {
            throw new NotImplementedException();
        }
    }
}