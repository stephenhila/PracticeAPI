using System;
using System.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using Microsoft.AspNet.SignalR;
using PracticeAPI.Web.Models.DbModels;

namespace PracticeAPI.Web
{
    public class ChatHub : Hub
    {
        [Authorize]
        public void Send(string message)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(ConfigurationManager.AppSettings["apiUrl"]);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                string name = HttpContext.Current.User.Identity.Name;

                HttpResponseMessage Res = client.PostAsJsonAsync<MessageModel>("api/Messages/Post", new MessageModel { Text = message, Username = name }).Result; // TODO: make this async

                if (Res.IsSuccessStatusCode)
                {
                    Clients.All.addNewMessageToPage(name, message);
                }
            }
        }
    }
}