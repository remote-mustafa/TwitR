using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using TwitR.Models.Concrete;
using TwitR.Repositories.Abstract;
using TwitR.Repositories.Concrete.Dapper.Context;

namespace TwitR.Repositories.Concrete.Dapper
{
    public class TweetRepository : IEntityRepository<Tweet>
    {
        private readonly DapperContext _context;

        public TweetRepository(DapperContext context)
        {
            _context = context;
        }

        public async Task<Tweet> AddAsync(Tweet entity)
        {
            string query = "INSERT INTO Tweets (TweetText, UserId, CreatedDate) OUTPUT INSERTED.* VALUES (@TweetText, @UserId, @CreatedDate)";

            entity.CreatedDate = DateTime.Now;

            var parameters = new DynamicParameters();
            parameters.Add("TweetText", entity.TweetText, DbType.String);
            parameters.Add("UserId", entity.UserId, DbType.Int32);
            parameters.Add("CreatedDate", entity.CreatedDate, DbType.DateTime2);

            using (var connection = _context.CreateConnection())
            {
                var addedTweet = await connection.QueryFirstAsync<Tweet>(query, parameters);
                return addedTweet;
            }
        }

        public async Task<IEnumerable<Tweet>> GetAllAsync()
        {
            string query = "SELECT * FROM Tweets INNER JOIN Users ON Tweets.UserId = Users.Id";

            using (var connection = _context.CreateConnection())
            {
                var tweets = await connection.QueryAsync<Tweet, User, Tweet>(query, (tweet, user) =>
                {
                    tweet.User = user;
                    return tweet;
                });
                return tweets.ToList();
            }
        }
    }
}
