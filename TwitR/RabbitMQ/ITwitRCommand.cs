using System.Threading.Tasks;
using TwitR.Models;

namespace TwitR.RabbitMQ
{
    public interface ITwitRCommand
    {
        bool SendTwitToQueue(Tweet tweet);
        void GetTwitFormQueue();
    }
}
