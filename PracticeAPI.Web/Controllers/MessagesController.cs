using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PracticeAPI.Web.Controllers
{
    public class MessagesController : Controller
    {
        // GET: Message
        [Authorize]
        public ActionResult Index()
        {
            return View();
        }
    }
}