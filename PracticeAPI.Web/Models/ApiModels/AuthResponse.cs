using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PracticeAPI.Web.Models.ApiModels
{
    public class AuthResponse
    {
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }
        [JsonProperty("token_type")]
        public string TokenType { get; set; }
        [JsonProperty(".expires_in")]
        public double ExpiresIn { get; set; }
        [JsonProperty("Username")]
        public string Username { get; set; }
        [JsonProperty(".issued")]
        public DateTime Issued { get; set; }
        [JsonProperty(".expires")]
        public DateTime Expires { get; set; }
    }
}