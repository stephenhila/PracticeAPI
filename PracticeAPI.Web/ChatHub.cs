using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using MongoDB.Bson;
using MongoDB.Driver;
using PracticeAPI.Web.Models.DbModels;

namespace PracticeAPI.Web
{
    public class ChatHub : Hub
    {
        [Authorize]
        public void Send(string message)
        {
            string name = HttpContext.Current.User.Identity.Name;

            string constr = ConfigurationManager.AppSettings["connectionString"];
            var Client = new MongoClient(constr);
            var DB = Client.GetDatabase("SampleDB");
            var messageCollection = DB.GetCollection<MessageModel>("Messages");
            var userCollection = DB.GetCollection<UserModel>("Users");

            var user = userCollection.Find(u => u.Username == name).FirstOrDefault();

            if (user != null && !string.IsNullOrEmpty(message))
            {
                var messageModel = new MessageModel { Text = message, UserId = user.Id };
                messageCollection.InsertOne(messageModel);

                // Call the broadcastMessage method to update clients.
                Clients.All.addNewMessageToPage(name, message);
            }
        }
    }
}