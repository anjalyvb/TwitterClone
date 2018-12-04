using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TwitterClone.Models
{
    public class DashboardModel
    {
        public string LoggedInUserId { get; set; }
        public int TweetCount { get; set; }
        public int Following { get; set; }
        public int Followers { get; set; }
        public string Message { get; set; }
        public virtual Person Person { get; set; }
        public virtual List<Tweet> TweetList { get; set; }
    }
}