using Microsoft.AspNet.Identity;
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

        public ActionResult New()
        {
            Profile profile = new Profile();

            return View(profile);
        }

        [HttpPost]
        public ActionResult New(Profile profile)
        {
            try
            {
                profile.UserId = User.Identity.GetUserId();
                
                var userId = User.Identity.GetUserId();
            
                
                    var user = db.Users.FirstOrDefault(u => u.Id == userId);
                    profile.Email = user.Email;
                
                db.Profiles.Add(profile);
                db.SaveChanges();

                Friend friend = new Friend();
                friend.UserId = profile.UserId;
                db.Friends.Add(friend);
                db.SaveChanges();

                return Redirect("/Profiles/Show/" + profile.ProfileId);
            }

            catch (Exception e)
            {
                return Redirect("/Home/Index");
            }

        }

        public ActionResult Show(int id)
        {

            ViewBag.isAdmin = User.IsInRole("Admin");
            Profile profile = db.Profiles.Find(id);
            ViewBag.UserId = User.Identity.GetUserId();
            ViewBag.ProfileId = id;
            return View(profile);
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
                return Redirect("/Profiles/Show/" + profile.ProfileId);
            }
            catch (Exception e)
            {
                return View();
            }

        }

    }

}