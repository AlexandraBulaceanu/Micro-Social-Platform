using Reaction.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Reaction.Controllers
{
    public class PostsController : Controller
    {
        private Reaction.Models.AppContext db = new Reaction.Models.AppContext();

        // GET: Post
        public ActionResult Index()
        {


            ViewBag.Posts = from post in db.Posts
                             orderby post.Date
                             select post;

            if (TempData.ContainsKey("DeletePost"))
                ViewBag.deletePost = TempData["DeletePost"].ToString();

            if (TempData.ContainsKey("NewPost"))
                ViewBag.addPost = TempData["NewPost"].ToString();

            if (TempData.ContainsKey("EditPost"))
                ViewBag.editPost = TempData["EditPost"].ToString();

            return View();
        }

        public ActionResult New()
        {
            Post post = new Post();

            return View(post);
        }

        [HttpPost]
        public ActionResult New(Post post)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    post.Date = DateTime.Now;
                    post.Likes = 0;
                    db.Posts.Add(post);
                    db.SaveChanges();
                    TempData["NewPost"] = "The post has been added!";
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

        public ActionResult Show(int id)
        {
            Post post = db.Posts.Find(id);
            return View(post);
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

                if (TryUpdateModel(post))
                {
                    post.Content = requestPost.Content;
                    post.Date = DateTime.Now;
                    db.SaveChanges();
                    TempData["EditPost"] = "The post was modified!";
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
        public ActionResult Delete(int postId)
        {
            try
            {
                var comments = from comment in db.Comments
                               where comment.PostId == postId
                               select comment;
                foreach (Comment comm in comments)
                {
                    db.Comments.Remove(comm);
                }
                Post post = db.Posts.Find(postId);
                db.Posts.Remove(post);
                db.SaveChanges();
                TempData["deletePost"] = "The post was deleted!";
                return RedirectToAction("Index");
            } catch (Exception e) {
                return View();
            }

        }
    }
}
