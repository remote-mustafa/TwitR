using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.SignalR.Client;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using TwitR.RabbitMQ.Abstract;
using System;
using System.Text;
using System.Threading.Tasks;
using TwitR.Hubs;
using TwitR.Models.Concrete;

namespace TwitR.RabbitMQ.Concrete
{
    public class RabbitHandler : ITwitRCommand
    {
        private IHubContext<TweetHub> _tweetHub;
        public RabbitHandler(IHubContext<TweetHub> tweetHub)
        {
            _tweetHub = tweetHub;
        }
        public Task<Tweet> AddTweetToQueue(Tweet tweet)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {

                    channel.ExchangeDeclare("fanout-queue", ExchangeType.Fanout, true);
                    channel.QueueDeclare("twit", true, false, true);

                    channel.QueueBind("twit", "fanout-queue", "");

                    string modifiedTweetText = $"{tweet.TweetText} --RabbitMQ ile gönderildi";

                    Tweet sendTweet = new Tweet()
                    {
                        UserId = tweet.UserId,
                        TweetText = modifiedTweetText,
                        User = tweet.User
                    };

                    var serializeTweetJson = JsonConvert.SerializeObject(sendTweet);
                    var queueMessageBody = Encoding.UTF8.GetBytes(serializeTweetJson);

                    channel.BasicPublish("fanout-queue", "", null, queueMessageBody);

                    return Task.FromResult(sendTweet);
                }
            }
        }

        public Task GetTweetFromQueue()
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            var connection = factory.CreateConnection();
            var channel = connection.CreateModel();

            channel.QueueBind("twit", "fanout-queue", "", null);

            var consumer = new EventingBasicConsumer(channel);
            channel.BasicQos(0, 1, false);
            channel.BasicConsume("twit", false, consumer);

            consumer.Received += (model, ea) =>
            {
                var tweetString = Encoding.UTF8.GetString(ea.Body.ToArray());
                var tweet = JsonConvert.DeserializeObject<Tweet>(tweetString);
                _tweetHub.Clients.All.SendAsync("ReceiveTweet", tweet);
            };

            return Task.CompletedTask;
        }
    }
}
