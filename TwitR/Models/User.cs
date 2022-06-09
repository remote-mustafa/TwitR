using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TwitR.Models
{
    public class User
    {
        public string ConnectionId { get; set; }
        [Required(ErrorMessage ="Lütfen bir kullanıcı adı giriniz!")]
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public ushort? Age { get; set; }

        public List<Tweet> Tweets { get; set; }

        public User()
        {
            Tweets = new List<Tweet>();
        }
    }
}
