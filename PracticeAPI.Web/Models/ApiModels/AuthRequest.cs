using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PracticeAPI.Web.Models.ApiModels
{
    public class AuthRequest
    {
        [JsonProperty("method")]
        public string Method { get; }
        [JsonProperty("email")]
        public string Email { get; }
        [JsonProperty("password")]
        public string Password { get; }

        public AuthRequest(string email, string password)
        {
            Method = "ums";
            Email = email;
            Password = password;
        }
    }
}