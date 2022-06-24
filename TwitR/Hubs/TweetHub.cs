using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TwitR.Controllers;
using TwitR.Models;
using TwitR.Models.Concrete;
using TwitR.RabbitMQ;
using TwitR.Repositories.Abstract;
using TwitR.Repositories.Concrete.Dapper;

namespace TwitR.Hubs
{
    public class TweetHub : Hub
    {
        private readonly IEntityRepository<User> _userRepository;
        private readonly IEntityRepository<Message> _messageRepository;
        private readonly IEntityRepository<Tweet> _tweetRepository;

        private static Dictionary<string, List<string>> _connections =
            new Dictionary<string, List<string>>();

        public short TweetCharacterLimit { get; set; } = 150;

        public TweetHub(IEntityRepository<User> userRepository, IEntityRepository<Message> messageRepository, IEntityRepository<Tweet> tweetRepository)
        {
            _userRepository = userRepository;
            _messageRepository = messageRepository;
            _tweetRepository = tweetRepository;
        }

        public override async Task OnConnectedAsync()
        {
            string connectionId = Context.ConnectionId;
            await Clients.Caller.SendAsync("ReceiveConnnectionId", connectionId);
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            MyLists.ConnectedUsers.Remove(Context.ConnectionId);
            return Task.CompletedTask;
        }
       
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
                connectionIds = GetConnectionsByUserName(userName).Result.ToList();
                connectionIds.Add(Context.ConnectionId);
            }

            return Task.CompletedTask;
        }

        public async Task<IEnumerable<string>> GetConnectionsByUserName(string key)
        {
            List<string> connectionIds;

            if (_connections.TryGetValue(key, out connectionIds))
            {
                return await Task.FromResult(connectionIds);
            }
            return null;
        }

        public async Task UserDetailByUserName(string userNameValue)
        {

            User user = _userRepository.GetAllAsync().Result.FirstOrDefault(x => x.UserName == userNameValue);
            if (user != null)
            {
                await Clients.Caller.SendAsync("ReceiveUserDetails", user);
            }
        }


        public async Task GetUsers()
        {
            var users = MyLists.ConnectedUsers.Values.ToList();
            await Clients.All.SendAsync("GetUsers", users);
        }

        public async Task GetTweetCharacterLimit()
        {
            short limit = TweetCharacterLimit;
            await Clients.Caller.SendAsync("RecievedCharacterLimit", limit);
        }

        public async Task GetTweets()
        {
            var tweetList = _tweetRepository.GetAllAsync().Result.ToList();
            if (tweetList.Count() > 0)
            {
                await Clients.All.SendAsync("ReceieveAllTweets", tweetList);
            }
        }
        //public async Task SendMessage(Message message, string groupName)
        //{
        //    var addedMessage = await _messageRepository.AddAsync(message);
        //    await Clients.Group(groupName).SendAsync("ReceiveMessage", addedMessage);
        //}

        public async Task AddToMessageGroup(string groupName, string toUserId)
        {
            var connectionIdToUserId = MyLists.ConnectedUsers.FirstOrDefault(x => x.Value.Id.ToString() == toUserId).Key;
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
            await Groups.AddToGroupAsync(connectionIdToUserId, groupName);

        }
    }
}
