using Microsoft.Owin.Security;
using PracticeAPI.Web.Models.ApiModels;
using PracticeAPI.Web.Models.ViewModels.Login;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;

namespace PracticeAPI.Web.Controllers
{
    public class LoginController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(IndexViewModel model)
        {
            if (ModelState.IsValid)
            {
                using (var client = new HttpClient())
                {
                    var loginData = new List<KeyValuePair<string, string>>();
                    loginData.Add(new KeyValuePair<string, string>("grant_type", "password"));
                    loginData.Add(new KeyValuePair<string, string>("username", model.Username));
                    loginData.Add(new KeyValuePair<string, string>("password", model.Password));
                    HttpContent loginContent = new FormUrlEncodedContent(loginData);

                    client.BaseAddress = new Uri(ConfigurationManager.AppSettings["apiUrl"]);
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    HttpResponseMessage Res = client.PostAsync("token", loginContent).Result;

                    if (Res.IsSuccessStatusCode)
                    {
                        var authResponseModel = Res.Content.ReadAsAsync<AuthResponse>().Result;

                        AuthenticationProperties options = new AuthenticationProperties();

                        options.AllowRefresh = true;
                        options.IsPersistent = true;
                        options.ExpiresUtc = authResponseModel.Expires;

                        var claims = new[]
                        {
                            new Claim(ClaimTypes.Name, authResponseModel.Username),
                            new Claim("AcessToken", string.Format("Bearer {0}", authResponseModel.AccessToken)),
                        };
                        var identity = new ClaimsIdentity(claims, "ApplicationCookie");
                        Request.GetOwinContext().Authentication.SignIn(options, identity);
                        //HttpContext.User = new CustomPrincipal(model.Username);

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
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(ConfigurationManager.AppSettings["apiUrl"]);
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    HttpResponseMessage Res = client.GetAsync($"api/Registration/IsUsernameAvailable?username={registration.Username}").Result; // TODO: make this async

                    if (Res.IsSuccessStatusCode)
                    {
                        var response = Res.Content.ReadAsStringAsync().Result;

                        bool usernameAvailable = Convert.ToBoolean(response.ToLowerInvariant());

                        if (usernameAvailable)
                        {
                            var userModel = new UserModel { Username = registration.Username, Password = registration.Password };
                            var postTask = client.PostAsJsonAsync<UserModel>("api/Registration/PostNewUser", userModel);

                            postTask.Wait();

                            var result = postTask.Result;
                            if (result.IsSuccessStatusCode)
                            {
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