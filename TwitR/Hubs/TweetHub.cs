using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TwitR.Controllers;
using TwitR.Models;

namespace TwitR.Hubs
{
    public class TweetHub : Hub
    {
        public static List<Tweet> TweetList { get; set; } = new List<Tweet>();
        public static List<User> LoginUsers = new List<User>();

        private static Dictionary<string, List<string>> _connections =
            new Dictionary<string, List<string>>();

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

        public void AddLoginUser(User user)
        {
            User newUser = new User();
            newUser = user;
            LoginUsers.Add(newUser);
        }

        public async Task SendTweet(string tweetText,string userName)
        {
            if (tweetText.Length <= TweetCharacterLimit)
            {
                Tweet receivedTweet = new Tweet();
                receivedTweet.TweetText = tweetText;
                receivedTweet.User = LoginUsers.Where(x => x.UserName == userName).FirstOrDefault();

                TweetList.Add(receivedTweet);
              
                await Clients.All.SendAsync("ReceiveTweet", receivedTweet);
            }
        }

        public async Task GetTweetCharacterLimit()
        {
            short limit = TweetCharacterLimit;
            await Clients.Caller.SendAsync("RecievedCharacterLimit", limit);
        }
    }
}
