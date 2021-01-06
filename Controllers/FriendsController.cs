using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using Reaction.Models;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace Reaction.Controllers
{
    public class FriendsController : Controller
    {

        private Reaction.Models.ApplicationDbContext db = new Reaction.Models.ApplicationDbContext();

        // GET: Friends
        public ActionResult Index()
        {
            return View();
        }


        [Authorize(Roles = "Admin,User")]
        public ActionResult ShowFriends()
        {
            if (User.IsInRole("Admin"))
                return RedirectToAction("Index", "Home");
            Profile userProfile = new Profile();
            string userId = User.Identity.GetUserId();
            List<Profile> friendsList = new List<Profile>();
            List<Friend> allProfiles = new List<Friend>();

            var profiles = from profile in db.Profiles
                           where profile.UserId == userId
                           select profile;
            foreach (var profile in profiles)
            {
                userProfile = profile;
                break;
            }


            var friends = from profile in db.Friends
                          select profile;
            foreach (var profile in friends)
            {
                allProfiles.Add(profile);
            }

            foreach (Friend friend in allProfiles)
            {

                if (userProfile.Friends.Contains(friend))
                    friendsList.Add(db.Profiles.Find(friend.FriendId));
            }
            ViewBag.friends = friendsList;
            return View();
        }


        [Authorize(Roles = "Admin,User")]
        [HttpPost]
        public ActionResult AcceptRequest(int id)
        {
            string userId = User.Identity.GetUserId();


            Profile userProfile = new Profile();
            Profile friendProfile = new Profile();


            var currentUserProfile = from profile in db.Profiles
                                     where profile.UserId == userId
                                     select profile;

            foreach (var profile in currentUserProfile)
            {
                userProfile = profile;
                break;
            }

            friendProfile = db.Profiles.Find(id);

            Friend currentProfile = new Friend();
            Friend nextProfile = new Friend();


            var nextFriend = from friend in db.Friends
                             where friend.UserId == friendProfile.UserId
                             select friend;
            foreach (var friend in nextFriend)
            {
                nextProfile = friend;
                break;
            }

            var userFriend = from friend in db.Friends
                             where friend.UserId == userProfile.UserId
                             select friend;
            foreach (var friend in userFriend)
            {
                currentProfile = friend;
                break;
            }

            try
            {
                if (TryUpdateModel(userProfile))
                {

                    userProfile.Friends.Add(nextProfile);
                    db.SaveChanges();
                }
                else
                {
                    
                    return View("Error");
                }

                if (TryUpdateModel(friendProfile))
                {
                    friendProfile.Friends.Add(currentProfile);
                    friendProfile.FriendRequests.Remove(userProfile);
                    db.SaveChanges();
                }
                else
                {
                    return View("Error");
                }

            }
            catch (Exception e)
            {

              
                return View("Error");
            }
            return RedirectToAction("FriendRequest", "Friends");
        }


        [Authorize(Roles = "Admin,User")]
        [HttpPost]
        public ActionResult DeleteFriendRequest(int id)
        {

            string userId = User.Identity.GetUserId();

            Profile userProfile = new Profile();
            Profile friendProfile = new Profile();

            var currentUserProfile = from profile in db.Profiles
                                     where profile.UserId == userId
                                     select profile;
            foreach (var profile in currentUserProfile)
            {
                userProfile = profile;
                break;
            }

            friendProfile = db.Profiles.Find(id);

            try
            {
                if (TryUpdateModel(friendProfile))
                {
                    friendProfile.FriendRequests.Remove(userProfile);
                    db.SaveChanges();
                }
                else
                    return View("Error");
            }
            catch (Exception e)
            {
                return View("Error");
            }
            return RedirectToAction("ShowFriendRequest", "Friends");
        }


        [Authorize(Roles = "Admin,User")]
        public ActionResult FriendRequest()
        {

            string userId = User.Identity.GetUserId();
            Profile userProfile = new Profile();
            Profile otherProfile = new Profile();

            List<Profile> profilesList = new List<Profile>();
            List<Profile> otherProfileList = new List<Profile>();

            var currentUserProfile = from profile in db.Profiles
                                     where profile.UserId == userId
                                     select profile;

            foreach (var profile in currentUserProfile)
            {
                userProfile = profile;
                break;
            }

            var allProfiles = from profile in db.Profiles
                              select profile;

            foreach (var profile in allProfiles)
            {
                otherProfile = profile;
                otherProfileList.Add(otherProfile);
            }

            foreach (Profile profile in otherProfileList)
            {
                if (profile.FriendRequests.Contains(userProfile))
                    profilesList.Add(profile);
            }


            ViewBag.profiles = profilesList;

            return View("FriendRequest");
        }


        [Authorize(Roles = ("Admin, User"))]
        [HttpPost]
        public ActionResult FriendRequest(int id)
        {

            Profile futureFriendProfile = db.Profiles.Find(id);


            string userId = User.Identity.GetUserId();
            var profiles = from profile in db.Profiles
                           where profile.UserId == userId
                           select profile;

            Profile userProfile = new Profile();
            foreach (var profile in profiles)
            {
                userProfile = profile;
                break;
            }

            try
            {
                if (TryUpdateModel(userProfile))
                {
                    userProfile.FriendRequests.Add(futureFriendProfile);
                    db.SaveChanges();
                   

                    return RedirectToAction("FriendRequest", "Friends");
                }
                else
                {
                    
                    return View("Error");
                }
            }
            catch (Exception e)
            {
                
                return View("Error");
            }

        }


    }
}