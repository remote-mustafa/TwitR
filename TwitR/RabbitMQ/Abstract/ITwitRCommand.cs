using System.Threading.Tasks;
using TwitR.Models.Concrete;

namespace TwitR.RabbitMQ.Abstract
{
    public interface ITwitRCommand
    {
        Task<Tweet> AddTweetToQueue(Tweet tweet);
        Task GetTweetFromQueue();
    }
}
