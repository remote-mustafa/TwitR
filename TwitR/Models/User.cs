using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TwitR.Models
{
    public class User
    {
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public ushort? Age { get; set; }
    }
}
