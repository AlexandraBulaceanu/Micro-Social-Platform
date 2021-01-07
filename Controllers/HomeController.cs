using Reaction.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;

namespace Reaction.Controllers
{
    public class HomeController : Controller
    {

        private Reaction.Models.ApplicationDbContext db = new Reaction.Models.ApplicationDbContext();
        public ActionResult Index()
        {
            if (TempData.ContainsKey("NotFound"))
                ViewBag.failedSearch = TempData["NotFound"].ToString();
            String userId = User.Identity.GetUserId();
            Profile profile = new Profile();
            var profiles = from p in db.Profiles
                           where p.UserId == userId
                           select p;
            foreach (var elem in profiles)
            {
                profile = elem;
                break;
            }
            ViewBag.ProfileId = profile.ProfileId;

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
            if (result == null) 
            { 
               TempData["NotFound"] = "There is no such user!"; 
               ViewBag.failedSearch = "There is no such user!";
                return View("NoUser"); }
            return Redirect("/Profiles/Show/"+result.ProfileId);
        }
    }
}