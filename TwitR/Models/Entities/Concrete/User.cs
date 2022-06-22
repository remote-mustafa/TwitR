using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TwitR.Models.Entities.Abstract;

namespace TwitR.Models.Concrete
{
    public class User : BaseEntity
    { 
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int? Age { get; set; }

        public ICollection<Tweet> Tweets { get; set; } = new List<Tweet>();
    }
}
