using TwitR.Models;

namespace TwitR.RabbitMQ
{
    public interface ITwitRCommand
    {
        void SendTwit(Tweet tweet);
    }
}
