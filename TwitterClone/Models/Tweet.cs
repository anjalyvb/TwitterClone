using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TwitterClone.Models
{
    public class Tweet
    {
        public int TweetId { get; set; }
        [Required]
        public string TweetUser { get; set; }
        [Required]
        public string TweetMessage { get; set; }
        public string TweetTime { get; set; }
    }
}