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
        public static List<User> LoginUsers { get; set; } = new List<User>();

        private static Dictionary<string, List<string>> _connections =
            new Dictionary<string, List<string>>();

        public short TweetCharacterLimit { get; set; } = 140;

        public override Task OnConnectedAsync()
        {
            //string name = Context.User.Identity.Name;
            //List<string> connections;
            //if(!_connections.TryGetValue(name,out connections))
            //{
            //    connections = new List<string>();
            //    connections.Add(Context.ConnectionId);
            //    _connections.Add(name, connections);
            //}
            return base.OnConnectedAsync();
        }

        public IEnumerable<string> GetConnections(string key)
        {
            List<string> connections;
            if (_connections.TryGetValue(key, out connections))
            {
                return connections;
            }
            return null;
        }

        public async Task GetUserName()
        {
            string userName = "sss";
            await Clients.Caller.SendAsync("MyUserName", userName);
        }

        public async Task GetConnectionId()
        {
            string connectionId = Context.ConnectionId;
            await Clients.Caller.SendAsync("TakeConnectionId", connectionId);
        }

        public async Task SendTweet(string tweetText, string connectionId)
        {
            string deger = Context.User.Identity.Name;
            if (tweetText.Length <= TweetCharacterLimit)
            {
                Tweet receivedTweet = new Tweet();
                receivedTweet.TweetText = tweetText;
                receivedTweet.User.ConnectionId = connectionId;

                TweetList.Add(receivedTweet);

                await Clients.All.SendAsync("ReceiveTweet", receivedTweet);
            }
        }

        public async Task GetCurrentUser(string connectionId)
        {
            if (LoginUsers.Count > 0)
            {
                User currentUser = LoginUsers.Where(x => x.ConnectionId == connectionId).FirstOrDefault();

                await Clients.Caller.SendAsync("UserInfos", currentUser);
            }

        }

    }
}
