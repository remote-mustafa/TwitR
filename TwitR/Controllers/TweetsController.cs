using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using System;
using System.Linq;
using TwitR.Hubs;
using TwitR.Models;
using TwitR.Models.Concrete;
using TwitR.RabbitMQ.Abstract;
using TwitR.Repositories.Abstract;

namespace TwitR.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class TweetsController : ControllerBase
    {
        private readonly IEntityRepository<User> _userRepository;
        private readonly IEntityRepository<Tweet> _tweetRepository;
        private readonly IEntityRepository<Message> _messageRepository;
        private IHubContext<TweetHub> _tweetHub;
        private ITwitRCommand _rabbitHandler;

        public TweetsController(IEntityRepository<User> userRepository,
            IHubContext<TweetHub> tweetHub,
            IEntityRepository<Tweet> tweetRepository,
            IEntityRepository<Message> messageRepository,
            ITwitRCommand rabbitHandler)
        {
            _userRepository = userRepository;
            _tweetRepository = tweetRepository;
            _messageRepository = messageRepository;
            _tweetHub = tweetHub;
            _rabbitHandler = rabbitHandler;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return Ok();
        }

        [HttpPost]
        public IActionResult AddUser([FromBody] User user, [FromQuery] string connId)
        {
            var addedUser = _userRepository.AddAsync(user).Result;
            MyLists.ConnectedUsers.Add(connId, addedUser);
            _tweetHub.Clients.Client(connId).SendAsync("AddUser", addedUser).Wait();
            var response = JsonConvert.SerializeObject(addedUser);
            return Ok(response);
        }

        [HttpPost]
        public IActionResult AddTweet(Tweet tweet)
        {

            Tweet rabbitTweetResult = _rabbitHandler.AddTweetToQueue(tweet).Result;
            var addedTweet = _tweetRepository.AddAsync(rabbitTweetResult).Result;

            _rabbitHandler.GetTweetFromQueue();

            var responseAddedTweetId = JsonConvert.SerializeObject(addedTweet.Id);
            return Ok(responseAddedTweetId);
        }

        [HttpPost]
        public IActionResult AddMessage(Message message)
        {
            var addedMessage = _messageRepository.AddAsync(message).Result;
            var fromUserName = _userRepository.GetAllAsync().Result.FirstOrDefault(x => x.Id == addedMessage.FromUserId).UserName;          
            string groupName = $"{addedMessage.FromUserId}_{addedMessage.ToUserId}";
            _tweetHub.Clients.Groups(groupName).SendAsync("ReceiveMessage",addedMessage, fromUserName);
            return Ok();
        }
    }
}
