using System.Collections.Generic;
using TwitR.Models.Concrete;

namespace TwitR.Models
{
    public static class MyLists
    {
        public static List<string> Names { get; set; } = new List<string>();
        public static Dictionary<string, User> ConnectedUsers { get; set; } = new Dictionary<string, User>();
    }
}
