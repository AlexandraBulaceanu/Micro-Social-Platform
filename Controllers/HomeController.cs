using Reaction.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Reaction.Controllers
{
    public class HomeController : Controller
    {

        private Reaction.Models.ApplicationDbContext db = new Reaction.Models.ApplicationDbContext();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

      
        [HttpPost]
        public ActionResult findUsers(string Username)
        {
            Profile result = null;
            var profiles = from prof in db.Profiles
                          select prof;
            foreach (var p in profiles) {
                if (p.Username == Username)
                { result = p;  break; }
            }
            if (result == null) return View();
            return Redirect("/Profiles/Show/"+result.ProfileId);
        }
    }
}