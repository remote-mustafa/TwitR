using Microsoft.AspNetCore.SignalR.Client;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Threading.Tasks;
using TwitR.Models;

namespace TwitR.RabbitMQ
{
    public class Handler : ITwitRCommand
    {
        static HubConnection connectionSignalR;
        public bool SendTwitToQueue(Tweet tweet)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {

                    channel.ExchangeDeclare("fanout-queue", ExchangeType.Fanout, true);
                    channel.QueueDeclare("twit", true, false, true);

                    channel.QueueBind("twit", "fanout-queue", "");

                    string modifiedTweetText = $"{tweet.TweetText} RabbitMQ ile gönderildi";

                    Tweet sendTweet = new Tweet()
                    {
                        User = tweet.User,
                        TweetText = modifiedTweetText
                    };

                    var serializeTweetJson = JsonConvert.SerializeObject(sendTweet);
                    var queueMessageBody = Encoding.UTF8.GetBytes(serializeTweetJson);

                    //channel.BasicPublish("fanout-queue", "", null, queueMessageBody);
                    channel.BasicPublish("fanout-queue", "", null, queueMessageBody);

                    return true;
                }
            }
        }

        public void GetTwitFormQueue()
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            var connection = factory.CreateConnection();
            var channel = connection.CreateModel();

            ConnetWithSignalR().Wait();
            channel.QueueBind("twit", "fanout-queue", "", null);

            var consumer = new EventingBasicConsumer(channel);
            channel.BasicQos(0, 1, false);
            channel.BasicConsume("twit", false, consumer);

            consumer.Received += (model, ea) =>
            {
                var tweetString = Encoding.UTF8.GetString(ea.Body.ToArray());
                var tweet = JsonConvert.DeserializeObject<Tweet>(tweetString);
                connectionSignalR.InvokeAsync("SendTweet", tweet);
            };
        }
        public static async Task ConnetWithSignalR()
        {
            connectionSignalR = new HubConnectionBuilder().WithUrl("https://localhost:44366/TweetHub").Build();
            await connectionSignalR.StartAsync();
        }
    }
}
