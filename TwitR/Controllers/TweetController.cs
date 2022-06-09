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
            TweetListViewModel model = new TweetListViewModel();
            model.LoginUser = loginUser;
            model.Tweets = Hubs.TweetHub.TweetList;
            return View(model);
        }

        [HttpPost]
        public IActionResult Login(TweetListViewModel model)
        {
            loginUser = new User();
            loginUser = model.LoginUser;
            Hubs.TweetHub.LoginUsers.Add(loginUser);
            return RedirectToAction("Index");

        }
    }
}
