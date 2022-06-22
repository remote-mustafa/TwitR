using System.Threading.Tasks;
using TwitR.Models.Concrete;

namespace TwitR.RabbitMQ
{
    public interface ITwitRCommand
    {
        bool SendTwitToQueue(Tweet tweet);
        void GetTwitFormQueue();
    }
}
