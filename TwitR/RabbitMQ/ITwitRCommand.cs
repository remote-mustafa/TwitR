using TwitR.Models;

namespace TwitR.RabbitMQ
{
    public interface ITwitRCommand
    {
        Tweet SendTwit(Tweet tweet);
    }
}
