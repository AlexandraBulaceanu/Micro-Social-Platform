using Reaction.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Reaction.Controllers
{
    public class ProfilesController : Controller
    {
        private Reaction.Models.ApplicationDbContext db = new Reaction.Models.ApplicationDbContext();

        // GET: Profiles
        public ActionResult Index()
        {
            return View();
        }

        [HttpDelete]
        public ActionResult Delete(int id)
        {
            Profile profile = db.Profiles.Find(id);
            db.Profiles.Remove(profile);
            db.SaveChanges();
            return Redirect("/Home/Index");
        }

        [HttpPost]
        public ActionResult New(Profile profile)
        {
            try
            {
                db.Profiles.Add(profile);
                db.SaveChanges();
                return Redirect("/Profile/Show/" + profile.ProfileId);
            }

            catch (Exception e)
            {
                return Redirect("/Home/Index");
            }

        }

        public ActionResult Edit(int id)
        {
            Profile profile = db.Profiles.Find(id);
            ViewBag.Profile = profile;
            return View();
        }

        [HttpPut]
        public ActionResult Edit(int id, Profile requestProfile)
        {
            try
            {
                Profile profile = db.Profiles.Find(id);
                if (TryUpdateModel(profile))
                {
                    profile.Description = requestProfile.Description;
                    profile.Username = requestProfile.Username;
                    profile.Visibility = requestProfile.Visibility;
                    db.SaveChanges();
                }
                return Redirect("/Profile/Show/" + profile.ProfileId);
            }
            catch (Exception e)
            {
                return View();
            }

        }
    }
}