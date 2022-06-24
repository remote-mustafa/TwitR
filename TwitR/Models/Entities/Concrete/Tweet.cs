using TwitR.Models.Entities.Abstract;

namespace TwitR.Models.Concrete
{
    public class Tweet : BaseEntity
    {
        public string TweetText { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }

    }
}
