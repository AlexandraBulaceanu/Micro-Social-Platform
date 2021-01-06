using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Reaction.Models;

namespace Reaction.Controllers
{
    public class GroupsController : Controller
    {
        // GET: Groups
        private ApplicationDbContext db = new ApplicationDbContext();


        public ActionResult Index()
        {


            if (TempData.ContainsKey("NewToGroup"))
                ViewBag.newToGroup = TempData["NewToGroup"].ToString();

            if (TempData.ContainsKey("DeleteGroup"))
                ViewBag.deleteGroup = TempData["DeleteGroup"].ToString();

            if (TempData.ContainsKey("CreateGroup"))
                ViewBag.createGroup = TempData["CreateGroup"].ToString();

            if (TempData.ContainsKey("EditGroup"))
                ViewBag.editGroup = TempData["EditGroup"].ToString();

            ViewBag.IsAdmin = false;
            ViewBag.LoggedIn = false;
            Profile userProfile = new Profile();
            if (Request.IsAuthenticated)
            {
                ViewBag.LoggedIn = true;
                ViewBag.UserId = User.Identity.GetUserId();
                if (User.IsInRole("Admin"))
                {
                    ViewBag.ShowButton = true;
                    ViewBag.IsAdmin = true;
                }
                else
                {
                    string userLoggedId = User.Identity.GetUserId();
                    userProfile = db.Profiles.Where(p => p.UserId == userLoggedId).ToList().First();
                }
            }
            else
                ViewBag.UserId = "";


           
           
            var posts = from post in db.Posts
                        select post;
            ViewBag.Posts = posts;
            var groups = from Group in db.Groups
                         select Group;
            ViewBag.groups = groups;
            
            ViewBag.profile = userProfile;


            return View();
        }


        public ActionResult Show(int id)
        {

            bool show = false;
            Group group = db.Groups.Find(id);
            if (group.Visibility.Equals(Group.VisibleGroup.Public))
            {
                show = true;
            }
            else
            {
                if (Request.IsAuthenticated)
                {
                    string userLoggedId = User.Identity.GetUserId();
                    Profile userProfile = db.Profiles.Where(p => p.UserId == userLoggedId).ToList().First();

                    if (group.Profiles.Contains(userProfile))
                        show = true;
                    if (User.IsInRole("Admin"))
                        show = true;
                }
            }

            if (show == true)
            {
                var posts = from p in db.Posts
                            where p.GroupId == id
                            select p;
                ViewBag.groupId = id;
                ViewBag.posts = posts;
                return View();
            }
            else
                return RedirectToAction("Index");
        }

        [HttpPost]
        [Authorize(Roles = "User")]
        public ActionResult NewToGroup(int id)
        {

            Group group = db.Groups.Find(id);
            Profile newProfile = new Profile();
            string userLoggedId = User.Identity.GetUserId();

            var profiles = from profile in db.Profiles
                           where profile.UserId == userLoggedId
                           select profile;
            foreach (var profile in profiles)
            {
                newProfile = profile;
            }

            try
            {
                if (TryUpdateModel(group))
                {

                    if (group.Profiles.Contains(newProfile))
                    {
                        group.Profiles.Remove(newProfile);
                        TempData["NewToGroup"] = "You left the group!";
                    }
                    else
                    {
                        group.Profiles.Add(newProfile);
                        TempData["NewToGroup"] = "You joined the group!";
                    }
                    db.SaveChanges();
                    return RedirectToAction("Index", "Groups");
                }
                else
                    return View("Error");
            }
            catch (Exception e)
            {
                ViewBag.Exception = e;
                return View("Error");
            }

        }

        public ActionResult Update(Group group)
        {
            if (TempData.ContainsKey("DeleteGroup"))
                ViewBag.deleteMessage = TempData["DeleteGroup"].ToString();

            if (TempData.ContainsKey("CreateGroup"))
                ViewBag.addMessage = TempData["DeleteGroup"].ToString();

            if (TempData.ContainsKey("EditGroup"))
                ViewBag.editMessage = TempData["EditGroup"].ToString();
            var posts = from post in db.Posts
                        select post;
            ViewBag.Posts = posts;
            var groups = from Group in db.Groups
                         select Group;
            ViewBag.groups = groups;
            return View("Index", group);
        }

        [Authorize(Roles = "Admin,User")]
        public ActionResult New()
        {
            Group group = new Group();

            return View(group);
        }

        [HttpPost]
        [Authorize(Roles = "Admin,User")]
        public ActionResult New(Group group)
        {
            string userLoggedId = User.Identity.GetUserId();
            group.UserId = userLoggedId;
            try
            {
                if (ModelState.IsValid)
                {
                    db.Groups.Add(group);
                    db.SaveChanges();
                    TempData["CreateGroup"] = "The group was succesfully created!";
                    return RedirectToAction("Index");
                }
                else
                    return RedirectToAction("Update", group);
            }
            catch (Exception e)
            {
                return View("Error");
            }
        }

        [Authorize(Roles = "Admin,User")]
        public ActionResult Edit(int id)
        {
            Group group = db.Groups.Find(id);
            if (group == null)
                return RedirectToAction("Index");
            ViewBag.group = group;
            return View(group);
        }

        [HttpPut]
        [Authorize(Roles = "Admin,User")]
        public ActionResult Edit(int id, Group groupToEdit)
        {
            try
            {
                Group group = db.Groups.Find(id);
                if (TryUpdateModel(group))
                {
                    group.Name = groupToEdit.Name;
                    group.Visibility = groupToEdit.Visibility;
                    db.SaveChanges();
                }
                TempData["EditGroup"] = "Group " + group.Name + " was succesfully modified!";

                return RedirectToAction("Index");

            }
            catch (Exception e)
            {
                return View("FailedGroup");
            }
        }

        [HttpDelete]
        [Authorize(Roles = "Admin,User")]
        public ActionResult Delete(int id)
        {
            
            try
            {
               // if (id == 1)
                //    return RedirectToAction("Index");

                Group group = db.Groups.Find(id);
                db.Groups.Remove(group);
                db.SaveChanges();
                TempData["DeleteGroup"] = "Group " + group.Name + "is not available anymore";
                return RedirectToAction("Index","Groups");

                //return RedirectToAction("DeletePosts/" + id.ToString(), "Posts");
            }
            catch (Exception e)
            {
                return View("Error");
            }
        }


    }
}