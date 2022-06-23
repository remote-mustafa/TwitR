using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TwitR.Controllers;
using TwitR.Models.Concrete;
using TwitR.Models.Entities.Concrete;
using TwitR.RabbitMQ;
using TwitR.Repositories.Abstract;
using TwitR.Repositories.Concrete.Dapper;

namespace TwitR.Hubs
{
    public class TweetHub : Hub
    {
        public static List<Tweet> TweetList { get; set; } = new List<Tweet>();
        public static List<User> LoginUsers = new List<User>();

        private IEntityRepository<User> _userRepository;
        private IEntityRepository<Message> _messageRepository;

        private static Dictionary<string, List<string>> _connections =
            new Dictionary<string, List<string>>();

        public TweetHub(IEntityRepository<User> userRepository, IEntityRepository<Message> messageRepository)
        {
            _userRepository = userRepository;
            _messageRepository = messageRepository;
        }

        public short TweetCharacterLimit { get; set; } = 150;

        public Task AddToConnection(string userName)
        {
            List<string> connectionIds;
            if (GetConnectionsByUserName(userName) == null)
            {
                connectionIds = new List<string>();
                connectionIds.Add(Context.ConnectionId);
                _connections.Add(userName, connectionIds);
            }
            else
            {
                connectionIds = GetConnectionsByUserName(userName).ToList();
                connectionIds.Add(Context.ConnectionId);
            }

            return Task.CompletedTask;
        }

        public IEnumerable<string> GetConnectionsByUserName(string key)
        {
            List<string> connectionIds;

            if (_connections.TryGetValue(key,out connectionIds))
            {
                return connectionIds;
            }
            return null;
        }

        public async Task UserDetailByUserName(string userNameValue)
        {
            User user = LoginUsers.Where(x => x.UserName == userNameValue).FirstOrDefault();
            if(user != null)
            {
                await Clients.Caller.SendAsync("ReceiveUserDetail", user);
            }
        }

        //public async Task AddUser()
        //{
        //    await Clients.Caller.SendAsync("AddUser");
        //}

        public async Task GetUsers()
        {
            var users =  await _userRepository.GetAll();
            await Clients.All.SendAsync("GetUsers", users);
        }

        public void AddLoginUserList(User user)
        {
            User newUser = new User();
            newUser = user;
            LoginUsers.Add(newUser);
        }

        public async Task SendTweet(Tweet tweet)
        {
            if (tweet.TweetText.Length <= TweetCharacterLimit)
            {
                TweetList.Add(tweet);
                await Clients.All.SendAsync("ReceiveTweet", tweet);
            }
        }

        public async Task GetTweetCharacterLimit()
        {
            short limit = TweetCharacterLimit;
            await Clients.Caller.SendAsync("RecievedCharacterLimit", limit);
        }

        public async Task GetTweetList()
        {
            var tweetList = TweetList;
            if (TweetList.Count > 0)
            {
                await Clients.All.SendAsync("ReceieveAllTweets", tweetList);
            }
        }
        public async Task SendMessage(Message message, string groupName)
        {
            var addedMessage = await _messageRepository.AddAsync(message);
            await Clients.Group(groupName).SendAsync("ReceiveMessage",addedMessage);
        }

        public async Task AddToMessageGroup(string groupName,string toUserId)
        {
            if (!GroupList.Names.Contains(groupName))
            {
                GroupList.Names.Add(groupName);
                await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
                await Groups.AddToGroupAsync(toUserId, groupName);
            }
           
        }
    }
}
