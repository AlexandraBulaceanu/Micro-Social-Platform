using Microsoft.AspNet.Identity;
using Reaction.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Reaction.Controllers
{
    public class PostsController : Controller
    {
        private Reaction.Models.ApplicationDbContext db = new Reaction.Models.ApplicationDbContext();

        private void SendEmailNotification(string toEmail, string subject, string content)
        {
            const string senderEmail = "alexandra-irina.bulaceanu@my.fmi.unibuc.ro";
            const string senderPassword = "parola";
            const string smtpServer = "smtp.gmail.com";
            const int smtpPort = 587;

            SmtpClient smtpClient = new SmtpClient(smtpServer, smtpPort);
            smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtpClient.EnableSsl = true;
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = new NetworkCredential(senderEmail, senderPassword);

            MailMessage email = new MailMessage(senderEmail, toEmail, subject, content);

            email.IsBodyHtml = true;

            email.BodyEncoding = UTF8Encoding.UTF8;

            try
            {
                System.Diagnostics.Debug.WriteLine("Sendin email...");
                smtpClient.Send(email);
                System.Diagnostics.Debug.WriteLine("Email sent!");
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("Error occured while trying to send email");
                System.Diagnostics.Debug.WriteLine(e.Message.ToString());
                RedirectToAction("Index", "Home");
            }
        }

        // GET: Post

        public ActionResult Index()
        {


            ViewBag.Posts = from post in db.Posts
                             orderby post.Date
                             select post;



            if (TempData.ContainsKey("Permission"))
                ViewBag.Permission = TempData["Permission"].ToString();
            
            if (TempData.ContainsKey("DeletePost"))
                ViewBag.deletePost = TempData["DeletePost"].ToString();

            if (TempData.ContainsKey("NewPost"))
                ViewBag.addPost = TempData["NewPost"].ToString();

            if (TempData.ContainsKey("EditPost"))
                ViewBag.editPost = TempData["EditPost"].ToString();

            return View();
        }

        [Authorize(Roles ="Admin,User")]
        public ActionResult New()
        {
            Post post = new Post();

            return View(post);
        }

        [HttpPost]
        [Authorize(Roles="Admin,User")]
        public ActionResult New(Post post)
        {
           // try
           // {
                if (ModelState.IsValid)
                {
                    post.UserId = User.Identity.GetUserId();
                    post.Date = DateTime.Now;
                    post.Likes = 0;
                    post.GroupId = 1;
                    //post.Comments = new List<Comment>(); ///Newly added
                    db.Posts.Add(post);
                    db.SaveChanges();
                    TempData["NewPost"] = "The post has been added!";
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["NewPost"] = "Error";
                    return RedirectToAction("Index");
                }
           // }
          //  catch (Exception e)
          //  {
          //      TempData["NewPost"] = e.Message.ToString();
           //     return RedirectToAction("Index");
          //  }
        }


      

        [Authorize(Roles = "Admin,User")]
        public ActionResult NewToGroup(int id)
        {
            Post post = new Post();
            ViewBag.groupId = id;

            return View(post);
        }

        [HttpPost]
        [Authorize(Roles = "Admin,User")]
        public ActionResult NewToGroup(int id, Post post)
        {
            // try
            // {
            if (ModelState.IsValid)
            {
                post.UserId = User.Identity.GetUserId();
                post.Date = DateTime.Now;
                post.Likes = 0;
                post.GroupId = id;
                //post.Comments = new List<Comment>(); ///Newly added
                db.Posts.Add(post);
                db.SaveChanges();
                TempData["NewPost"] = "The post has been added!";
                return RedirectToAction("Show/"+id.ToString(),"Groups");
            }
            else
            {
                TempData["NewPost"] = "Error";
                return RedirectToAction("Index");
            }
            // }
            //  catch (Exception e)
            //  {
            //      TempData["NewPost"] = e.Message.ToString();
            //     return RedirectToAction("Index");
            //  }
        }

        public ActionResult Show(int id)
        {

            ViewBag.isAdmin = User.IsInRole("Admin");
            Post post = db.Posts.Find(id);
            ViewBag.UserId = User.Identity.GetUserId();
            return View(post);
        }


        [Authorize(Roles = "Admin,User")]
        public ActionResult Edit(int id)
        {
            Post post = db.Posts.Find(id);
            if(post != null && post.UserId == User.Identity.GetUserId() && !User.IsInRole("Admin"))
            {
                TempData["Permission"] = "Not enough permissions";
                return RedirectToAction("Index", "Posts");
            }
            return View(post);
        }

        [HttpPut]
        [Authorize(Roles = "Admin,User")]
        public ActionResult Edit(int id, Post requestPost)
        {
           

            try
            {
                Post post = db.Posts.Find(id);
                if (post != null && post.UserId == User.Identity.GetUserId() && !User.IsInRole("Admin"))
                {
                    TempData["Permission"] = "Not enough permissions";
                    return RedirectToAction("Index", "Posts");
                }

                if (User.IsInRole("Admin"))
                {
                    string authorEmail = db.Profiles.Where(p => p.UserId == post.UserId).ToList().First().Email;
                    string notificationBody = "<p>The admin considered one of your posts innapropiate and modified it!</p>";

                    SendEmailNotification(authorEmail, "A post was modified", notificationBody);
                }


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

        ///newly added
        [NonAction]
        public void UpdateLikes(int id)
        {
            Post requestPost = db.Posts.Find(id);
            requestPost.Likes++;
            Edit(id, requestPost);
        }
        ///newly added

        [HttpDelete]
        [Authorize(Roles ="Admin,User")]
        public ActionResult Delete(int id)
        {
           

            try
            {
                Post post = db.Posts.Find(id);
                if (post != null && post.UserId == User.Identity.GetUserId() && !User.IsInRole("Admin"))
                {
                    TempData["Permission"] = "Not enough permissions";
                    return RedirectToAction("Index", "Posts");
                }

                var comments = from comment in db.Comments
                               where comment.PostId == id
                               select comment;
                foreach (Comment comm in comments)
                {
                    db.Comments.Remove(comm);
                }
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
