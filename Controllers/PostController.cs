using Reaction.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Reaction.Controllers
{
    public class PostController : Controller
    {
        private Reaction.Models.AppContext db = new Reaction.Models.AppContext();

        // GET: Post
        public ActionResult Index()
        {
            /*if (TempData.ContainsKey("message"))
            {
                ViewBag.message = TempData["message"].ToString();
            }*/

            ViewBag.Posts = from post in db.Posts
                             orderby post.Date
                             select post;
            
            return View();
        }

        public ActionResult Show(int id)
        {
            Post post = db.Posts.Find(id);
            return View(post);
        }

        public ActionResult New()
        {
            return View();
        }

        [HttpPost]
        public ActionResult New(Post post)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Posts.Add(post);
                    db.SaveChanges();
                    TempData["message"] = "The post has been added!";
                    return RedirectToAction("Index");
                }
                else
                {
                    return View(post);
                }
            }
            catch (Exception e)
            {
                return View(post);
            }
        }

        public ActionResult Edit(int id)
        {
            Post post = db.Posts.Find(id);
            return View(post);
        }

        [HttpPut]
        public ActionResult Edit(int id, Post requestPost)
        {
            try
            {
                Post post = db.Posts.Find(id);

                //throw new Exception();

                if (TryUpdateModel(post))
                {
                    post.Content = requestPost.Content;
                    db.SaveChanges();
                    TempData["message"] = "The post was modified!";
                    return RedirectToAction("Index");
                }

                return View(requestPost);
            }
            catch (Exception e)
            {
                return View(requestPost);
            }
        }

        [HttpDelete]
        public ActionResult Delete(int id)
        {
            Post post = db.Posts.Find(id);
            db.Posts.Remove(post);
            TempData["message"] = "The post was deleted!";
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}