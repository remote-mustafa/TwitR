using System.Collections.Generic;

namespace TwitR.Models
{
    public class TweetListViewModel
    {
        public List<User> Users { get; set; }
        public List<Tweet> Tweets { get; set; }
        public User LoginUser { get; set; }
    }
}
