using Microsoft.AspNetCore.SignalR;
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

        public short TweetCharacterLimit { get; set; } = 140;


        public async Task AddToConnection(string userName)
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

        public User CurrentUserByUserName(string userName)
        {
            return null;
        }

        public void AddLoginUser(User user)
        {
            LoginUsers.Add(user);
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
    }
}
