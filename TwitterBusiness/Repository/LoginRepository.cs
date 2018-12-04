using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwitterBusiness.Models;
using System.Data.Entity.SqlServer;


namespace TwitterBusiness
{
    public class LoginRepository
    {
        private TwitterEntities context;
        public LoginRepository()
        {
            context = new TwitterEntities();
        }
        public bool Validate(Person model)
        {
            return context.People.FirstOrDefault(p => p.UserId == model.UserId
                && p.PasswordHash == model.PasswordHash) != null ? context.People.FirstOrDefault(p => p.UserId == model.UserId
                && p.PasswordHash == model.PasswordHash).Active : false;
        }

        public string SignUp(Person model)
        {
            Person oldUser = new Person();
            oldUser = context.People.FirstOrDefault(p => p.UserId == model.UserId || p.Email == model.Email);
            if (oldUser == null) 
            {
                model.Active = true;
                model.Joined = DateTime.Now;
                context.People.Add(model);
                context.SaveChanges();
                return "Success";
            }
            else
            {
                return "Duplicate User";
            }

        }

        public Dashboard GetDashboardDetails(string userId)
        {
            Dashboard dash = new Dashboard();
            dash.TweetCount = context.Tweets.Select(t => t.UserId == userId).Count();
            dash.Followers = context.People.FirstOrDefault(p => p.UserId == userId).People.Count();
            dash.Following = context.People.FirstOrDefault(p => p.UserId == userId).Person1.Count();
            dash.Person = new Person();
            dash.Person = context.People.FirstOrDefault(p => p.UserId == userId);
            dash.TweetList = new List<Tweet>();
            dash.TweetList = GetAllTweets();
            return dash;
        }

        public List<Tweet> GetAllTweets()
        {
            Dashboard dash = new Dashboard();            
            dash.TweetList = new List<Tweet>();
            dash.TweetList = context.Tweets.Select(t => t).OrderByDescending(m => m.Created).ToList();
            return dash.TweetList;
        }

        public string PostTweet(Tweet model)
        {
            model.Created = DateTime.Now;
            context.Tweets.Add(model);
            context.SaveChanges();
            return "Success";
        }
    }

    public class Dashboard
    {
        public int TweetCount { get; set; }
        public int Following { get; set; }
        public int Followers { get; set; }
        public string Message { get; set; }
        public virtual Person Person { get; set; }
        public virtual List<Tweet> TweetList { get; set; }
    }

}
