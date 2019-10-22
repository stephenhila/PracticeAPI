using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PracticeAPI.Web.Models.ApiModels
{
    public class Car
    {
        [JsonProperty("model")]
        public string Model { get; set; }
    }
}