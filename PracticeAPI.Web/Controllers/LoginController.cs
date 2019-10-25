using MongoDB.Driver;
using PracticeAPI.Web.Models.DbModels;
using PracticeAPI.Web.Models.ViewModels.Login;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(IndexViewModel user)
        {
            if (ModelState.IsValid)
            {
                string constr = ConfigurationManager.AppSettings["connectionString"];
                var Client = new MongoClient(constr);
                var DB = Client.GetDatabase("SampleDB");
                var collection = DB.GetCollection<UserModel>("Users");
                var userModel = collection.Find(x => x.Username == user.Username && x.Password == user.Password).FirstOrDefault();

                if (userModel != null)
                {
                    FormsAuthentication.SetAuthCookie(userModel.Username, false);
                    // TODO: set authenticated user
                    return RedirectToAction("Index", "Home");
                }
            }
            return View(user);
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
                string constr = ConfigurationManager.AppSettings["connectionString"];
                var Client = new MongoClient(constr);
                var DB = Client.GetDatabase("SampleDB");
                var collection = DB.GetCollection<UserModel>("Users");
                var userModel = collection.Find(x => x.Username == registration.Username).FirstOrDefault();

                if (userModel != null)
                {
                    //localize this please as soon as possible, thanks!
                    ModelState.AddModelError("Username", "Username already exists!");
                }
                else
                {
                    userModel = new UserModel { Username = registration.Username, Password = registration.Password };
                    collection.InsertOne(userModel);

                    return RedirectToAction("Index");
                }
            }
            return View(registration);
        }
    }
}