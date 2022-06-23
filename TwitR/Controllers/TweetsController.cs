using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using System;
using TwitR.Hubs;
using TwitR.Models.Concrete;
using TwitR.Repositories.Abstract;

namespace TwitR.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class TweetsController : ControllerBase
    {
        private readonly IEntityRepository<User> _userRepository;
        public IHubContext<TweetHub> _tweetHub;
        public TweetsController(IEntityRepository<User> userRepository, IHubContext<TweetHub> tweetHub)
        {
            _userRepository = userRepository;
            _tweetHub = tweetHub;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return Ok();
        }

        [HttpPost]
        public IActionResult AddUser(User user)
        {
            var addedUser = _userRepository.AddAsync(user).Result;
            _tweetHub.Clients.All.SendAsync("AddUser", addedUser).Wait();
            var response = JsonConvert.SerializeObject(addedUser);
            return Ok(response);
        }

        [HttpGet]
        public IActionResult GetUsers()
        {
            
            return Ok();
        }
    }
}
