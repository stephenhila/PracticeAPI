using MongoDB.Driver;
using Newtonsoft.Json;
using PracticeAPI.Web.Models.DbModels;
using PracticeAPI.Web.Models.ViewModels.Login;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace PracticeAPI.Web.Controllers
{
    public class LoginController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Index(IndexViewModel user)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        string constr = ConfigurationManager.AppSettings["connectionString"];
        //        var Client = new MongoClient(constr);
        //        var DB = Client.GetDatabase("SampleDB");
        //        var collection = DB.GetCollection<UserModel>("Users");
        //        var userModel = collection.Find(x => x.Username == user.Username && x.Password == user.Password).FirstOrDefault();

        //        if (userModel != null)
        //        {
        //            FormsAuthentication.SetAuthCookie(userModel.Username, false);
        //            // TODO: set authenticated user
        //            return RedirectToAction("Index", "Home");
        //        }
        //    }
        //    return View(user);
        //}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(IndexViewModel model)
        {
            if (ModelState.IsValid)
            {
                using (var client = new HttpClient()) //TODO: code smell, find how to use HttpClient
                {
                    //Passing service base url  
                    client.BaseAddress = new Uri(ConfigurationManager.AppSettings["apiUrl"]);

                    client.DefaultRequestHeaders.Clear();
                    //Define request data format  
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    //Sending request to find web api REST service resource GetAllEmployees using HttpClient  
                    HttpResponseMessage Res = client.PostAsJsonAsync<UserModel>("api/Users/Login", new UserModel { Username = model.Username, Password = model.Password }).Result; // TODO: make this async

                    //Checking the response is successful or not which is sent using HttpClient  
                    if (Res.IsSuccessStatusCode)
                    {
                        //Storing the response details recieved from web api   
                        var userModel = Res.Content.ReadAsAsync<UserModel>().Result;

                        //Deserializing the response recieved from web api and storing into the Employee list  
                        FormsAuthentication.SetAuthCookie(userModel.Username, false);
                        return RedirectToAction("Index", "Home");
                    }
                }
            }
            return View(model);
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Register(UserRegistrationViewModel registration)
        {
            if (ModelState.IsValid)
            {
                using (var client = new HttpClient()) //TODO: code smell, find how to use HttpClient
                {
                    //Passing service base url  
                    client.BaseAddress = new Uri(ConfigurationManager.AppSettings["apiUrl"]);

                    client.DefaultRequestHeaders.Clear();
                    //Define request data format  
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    //Sending request to find web api REST service resource GetAllEmployees using HttpClient  
                    HttpResponseMessage Res = client.GetAsync($"api/Registration/IsUsernameAvailable?username={registration.Username}").Result; // TODO: make this async

                    //Checking the response is successful or not which is sent using HttpClient  
                    if (Res.IsSuccessStatusCode)
                    {
                        //Storing the response details recieved from web api   
                        var response = Res.Content.ReadAsStringAsync().Result;

                        //Deserializing the response recieved from web api and storing into the Employee list  
                        bool usernameAvailable = Convert.ToBoolean(response.ToLowerInvariant());

                        if (usernameAvailable)
                        {
                            var userModel = new UserModel { Username = registration.Username, Password = registration.Password };
                            var postTask = client.PostAsJsonAsync<UserModel>("api/Registration/PostNewUser", userModel);

                            postTask.Wait();

                            var result = postTask.Result;
                            if (result.IsSuccessStatusCode)
                            {
                                //return Success(new RegistrationSuccessViewModel { Username = registration.Username });
                                TempData["model"] = new RegistrationSuccessViewModel { Username = registration.Username };
                                return RedirectToAction("Success", "Login");
                            }
                            else
                            {
                                ModelState.AddModelError(string.Empty, "Server Error. Please contact administrator.");
                            }
                        }
                        else
                        {
                            //localize this please as soon as possible, thanks!
                            ModelState.AddModelError("Username", "Username already exists!");
                        }
                    }
                }
            }
            return View(registration);
        }

        public ActionResult Success()
        {
            return View(TempData["model"]);
        }
    }
}