using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using TwitR.Models;

namespace TwitR.RabbitMQ
{
    public class Handler : ITwitRCommand
    {
        public void SendTwit(Tweet tweet)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {

                    channel.ExchangeDeclare("fanout-queue", ExchangeType.Fanout, true);
                    var queueName = channel.QueueDeclare().QueueName;
                    channel.QueueDeclare(queueName, true, false, true);

                    channel.QueueBind(queueName, "fanout-queue", "");

                    string modifiedTweetText = $"{tweet.TweetText} RabbitMQ ile gönderildi";

                    Tweet sendTweet = new Tweet()
                    {
                        User = tweet.User,
                        TweetText = modifiedTweetText
                    };

                    var serializeTweetJson = JsonConvert.SerializeObject(sendTweet);
                    var queueMessageBody = Encoding.UTF8.GetBytes(serializeTweetJson);

                    channel.BasicPublish("fanout-queue", "", null, queueMessageBody);
                }
            }
        }

        public void GetTwit()
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    channel.ExchangeDeclare("fanout-queue", ExchangeType.Fanout, true);
                    var queueName = channel.QueueDeclare().QueueName;
                    channel.QueueBind(queueName, "fanout-queue", "");
                    
                    var consumer = new EventingBasicConsumer(channel);

                    consumer.Received += (object sender, BasicDeliverEventArgs e) =>
                    {
                        var tweet = Encoding.UTF8.GetString(e.Body.ToArray());
                        var deserializeTweetJson = JsonConvert.DeserializeObject<Tweet>(tweet);
                    };

                    channel.BasicConsume(queueName, true, consumer);
                }
            }
        }
    }
}
