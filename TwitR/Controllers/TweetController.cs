using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using TwitR.Models;

namespace TwitR.Controllers
{
    public class TweetController : Controller
    {
        public User loginUser = null;

        public IActionResult Index()
        {
           
            return View();
        }
    }
}
