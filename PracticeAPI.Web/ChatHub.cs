using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using Microsoft.AspNet.SignalR;
using PracticeAPI.Web.Models.ApiModels;

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

                HttpResponseMessage Res = client.PostAsJsonAsync<MessageModel>("api/Messages/Post", new MessageModel { Text = message, Username = name }).Result;

                if (Res.IsSuccessStatusCode)
                {
                    Clients.All.addNewMessageToPage(name, message);
                }
            }
        }

        public void Load()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(ConfigurationManager.AppSettings["apiUrl"]);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                int messageCount = int.Parse(ConfigurationManager.AppSettings["maxChatMessages"]);

                HttpResponseMessage Res = client.GetAsync($"api/Messages/Get?count={messageCount}").Result;

                if (Res.IsSuccessStatusCode)
                {
                    var messages = Res.Content.ReadAsAsync<IEnumerable<MessageModel>>().Result;

                    foreach (var message in messages)
                    {
                        Clients.Caller.addNewMessageToPage(message.Username, message.Text);
                    }
                }
            }
        }
    }
}