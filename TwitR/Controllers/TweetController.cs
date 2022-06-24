using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using TwitR.Models.Concrete;
using TwitR.RabbitMQ;

namespace TwitR.Controllers
{
    public class TweetController : Controller
    {
        public User loginUser = null;

        [HttpGet]
        public IActionResult Index()
        {

            return View();
        }

        //[HttpPost]
        //public JsonResult Index(string model)
        //{
        //    var tweet = JsonConvert.DeserializeObject<Tweet>(model);

        //    RabbitHandler rabbitHandler = new RabbitHandler();

        //    bool result = rabbitHandler.AddTweetToQueue(tweet);

        //    if (result)
        //    {
        //        rabbitHandler.GetTweetFromQueue();
        //    }

        //    return Json(new { isSuccessful = true});
        //}
    }
}
