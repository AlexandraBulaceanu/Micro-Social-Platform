using Microsoft.AspNet.Identity;
using Reaction.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Reaction.Controllers
{
    public class CommentsController : Controller
    {
        private Reaction.Models.ApplicationDbContext db = new Reaction.Models.ApplicationDbContext();

        // GET: Comments
        public ActionResult Index(int postId)
        {

            var comments = from comment in db.Comments
                           where comment.PostId == postId
                           select comment;
            ViewBag.isAdmin = User.IsInRole("Admin");

            ViewBag.PostId = postId;
            ViewBag.Comments = comments;

            if (TempData.ContainsKey("DeleteComm"))
                ViewBag.deleteComm = TempData["DeleteComm"].ToString();

            if (TempData.ContainsKey("NewComm"))
                ViewBag.addComm = TempData["NewComm"].ToString();

            if (TempData.ContainsKey("EditComm"))
                ViewBag.editComm = TempData["EditComm"].ToString();

            if (TempData.ContainsKey("Permission"))
                ViewBag.permission = TempData["Permission"].ToString();



            return View();
        }
        [Authorize(Roles ="Admin,User")]
        public ActionResult New(int id)
        {
            Comment comment = new Comment();
            comment.UserId = User.Identity.GetUserId();
            ViewBag.PostId = id;
            return View(comment);
        }

        [HttpPost]
        [Authorize(Roles = "Admin,User")]
        public ActionResult New(Comment comm)
        {
            comm.Date = DateTime.Now;

            comm.UserId = User.Identity.GetUserId();
            comm.Likes = 0;
            try
            {
                db.Comments.Add(comm);
                db.SaveChanges();
                TempData["New"] = "The comment was added";
                return Redirect("/Posts/Index/" + comm.PostId);
            }

            catch (Exception e)
            {
                return Redirect("/Posts/Index/" + comm.PostId);
            }

        }

        [Authorize(Roles = "Admin,User")]
        public ActionResult Edit(int id)
        {
            Comment comm = db.Comments.Find(id);
            ViewBag.Comments = comm;
            if(comm != null && comm.UserId != User.Identity.GetUserId() && !User.IsInRole("Admin"))
            {
                TempData["Permission"] = "Not enough permissions";
                return RedirectToAction("Index", "Comments", comm.PostId);
            }
            return View(comm);
        }

        [HttpPut]
        [Authorize(Roles = "Admin,User")]
        public ActionResult Edit(int id, Comment requestComment)
        {
            try
            {
                Comment comm = db.Comments.Find(id);
                if (comm != null && comm.UserId != User.Identity.GetUserId() && !User.IsInRole("Admin"))
                {
                    TempData["Permission"] = "Not enough permissions";
                    return RedirectToAction("Index", "Comments", comm.PostId);
                }
                if (TryUpdateModel(comm))
                {
                    comm.Content = requestComment.Content;
                    comm.Date = DateTime.Now;
                    db.SaveChanges();
                    TempData["New"] = "The comment was edited";
                }
                return Redirect("/Posts/Index/" + comm.PostId);
            }
            catch (Exception e)
            {
                return View();
            }

        }
        [HttpDelete]
        [Authorize(Roles = "Admin,User")]
        public ActionResult Delete(int id)
        {
            try
            {
                Comment comm = db.Comments.Find(id);
                if (comm != null && comm.UserId != User.Identity.GetUserId() && !User.IsInRole("Admin"))
                {
                    TempData["Permission"] = "Not enough permissions";
                    return RedirectToAction("Index", "Comments", comm.PostId);
                }
                db.Comments.Remove(comm);
                db.SaveChanges();
                TempData["Delete"] = "The comment was deleted";
                return Redirect("/Posts/Index/" + comm.PostId);
            } catch(Exception e) {
                return View();
            }
        }
    }
}