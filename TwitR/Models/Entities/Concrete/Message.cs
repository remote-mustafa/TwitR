using TwitR.Models.Entities.Abstract;

namespace TwitR.Models.Concrete
{
    public class Message : BaseEntity
    {
        public string Text { get; set; }
        public int FromUserId { get; set; }
        public int ToUserId { get; set; }
    }
}
