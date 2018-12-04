using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TwitterClone.Models;
using TwitterBusiness;
using Bus = TwitterBusiness.Models;
using System.Web.Security;
using System.Text;
using TwitterClone.Common;
using System.Globalization; 

namespace TwitterClone.Controllers
{
    public class LoginController : Controller
    {
        private LoginRepository loginRepo;
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SignIn(Person model)
        {
            //if (ModelState.IsValid)
            //{
                loginRepo = new LoginRepository();
                Bus.Person personModel = new Bus.Person();
                personModel.UserId = model.UserId;
                personModel.PasswordHash = Encryptor.MD5Hash(model.PasswordHash);
                bool isValid = loginRepo.Validate(personModel);
                if (isValid)
                {
                    FormsAuthentication.SetAuthCookie(model.UserId, true);
                return RedirectToAction("Dashboard", new { query = model.UserId });
                }
                ViewBag.Error = "Invalid Username or Password";
            //}
            return View("Login", model);
        }

        public ActionResult SignUp()
        {
            Person model = new Person();
            return View(model);
        }

        [HttpPost]
        public ActionResult SignUp(Person model)
        {
            if (ModelState.IsValid)
            {
                loginRepo = new LoginRepository();
                Bus.Person personModel = new Bus.Person();
                personModel.UserId = model.UserId;
                personModel.PasswordHash = Encryptor.MD5Hash(model.PasswordHash);
                personModel.FullName = model.FullName;
                personModel.Email = model.Email;
                var result = loginRepo.SignUp(personModel);
                if (result == "Success")
                {
                    ViewBag.Message = "Congratulations!You have successfully registered with Twitter Clone!";
                }
                else if (result == "Duplicate User")
                {
                    ViewBag.Error = "There is already an existing user with the same Username/Email ID.";
                }
            }
            return View(model);
        }

        public ActionResult Dashboard(string query)
        {
            loginRepo = new LoginRepository();
            DashboardModel dashModel = new DashboardModel();
            Dashboard dash = new Dashboard();
            dash = loginRepo.GetDashboardDetails(query);
            dash.TweetList = new List<Bus.Tweet>();
            dash.TweetList = loginRepo.GetAllTweets();
            
            dashModel.Following = dash.Following;
            dashModel.Followers = dash.Followers;
            dashModel.TweetCount = dash.TweetCount;
            dashModel.Person = new Person
            {
                FullName = dash.Person.FullName,
                Id = dash.Person.Id
            };
            dashModel.LoggedInUserId = dash.Person.UserId;
            Tweet tweet = new Tweet();
            dashModel.TweetList = new List<Tweet>();
            dashModel.TweetList = MapTweets(dash.TweetList);
            return View(dashModel);
        }

        [HttpPost]
        public ActionResult PostTweet(DashboardModel model)
        {
            Bus.Tweet tweetModel = new Bus.Tweet();
            tweetModel.UserId = model.LoggedInUserId;
            tweetModel.TweetMessage = model.Message;
            loginRepo = new LoginRepository();
            loginRepo.PostTweet(tweetModel);
            model.TweetList = new List<Tweet>();
            model.TweetList = MapTweets(loginRepo.GetAllTweets());
            return View("_TweetList",model);
        }

        public ActionResult Error()
        {
            return View();
        }

        private List<Tweet> MapTweets(List<Bus.Tweet> twList)
        {
            List<Tweet> list = new List<Tweet>();
            Tweet tweet = new Tweet();
            foreach (var obj in twList)
            {
                tweet.TweetUser = obj.UserId.ToUpper();
                tweet.TweetMessage = obj.TweetMessage;
                string dateFormat;
                if ( obj.Created.Subtract(DateTime.Today).Days == 0)
                {
                    dateFormat = "h:mm tt";
                }
                else
                {
                    dateFormat = "dd MMM";
                }
                tweet.TweetTime = obj.Created.ToString(dateFormat);
                list.Add(tweet);
                tweet = new Tweet();
            }
            return list;
        }

    }
}