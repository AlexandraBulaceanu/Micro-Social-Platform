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
        private Reaction.Models.AppContext db = new Reaction.Models.AppContext();

        // GET: Comments
        public ActionResult Index(int postId)
        {

            var comments = from comment in db.Comments
                           where comment.PostId == postId
                           select comment;

            ViewBag.PostId = postId;
            ViewBag.Comments = comments;

            if (TempData.ContainsKey("DeleteComm"))
                ViewBag.deleteComm = TempData["DeleteComm"].ToString();

            if (TempData.ContainsKey("NewComm"))
                ViewBag.addComm = TempData["NewComm"].ToString();

            if (TempData.ContainsKey("EditComm"))
                ViewBag.editComm = TempData["EditComm"].ToString();

            return View();
        }

        public ActionResult New()
        {
            return View();
        }

        [HttpPost]
        public ActionResult New(Comment comm)
        {
            comm.Date = DateTime.Now;
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

        public ActionResult Edit(int id)
        {
            Comment comm = db.Comments.Find(id);
            ViewBag.Comments = comm;
            return View();
        }

        [HttpPut]
        public ActionResult Edit(int id, Comment requestComment)
        {
            try
            {
                Comment comm = db.Comments.Find(id);
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
        public ActionResult Delete(int id)
        {
            try
            {
                Comment comm = db.Comments.Find(id);
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