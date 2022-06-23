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
    public class MessageRepository : IEntityRepository<Message>
    {
        private readonly DapperContext _context;

        public MessageRepository(DapperContext context)
        {
            _context = context;
        }

        public async Task<Message> AddAsync(Message entity)
        {
            string query = "INSERT INTO Messages (Text, FromUserId, ToUserId, CreatedDate) OUTPUT INSERTED.* VALUES (@Text, @FromUserId, @ToUserId, @CreatedDate)";

            entity.CreatedDate = DateTime.Now;

            var parameters = new DynamicParameters();
            parameters.Add("Text", entity.Text, DbType.String);
            parameters.Add("FromUserId", entity.FromUserId);
            parameters.Add("ToUserId", entity.ToUserId);
            parameters.Add("CreatedDate", entity.CreatedDate, DbType.DateTime2);

            using (var connection = _context.CreateConnection())
            {
                var addedMessage = await connection.QueryAsync<Message>(query, parameters);
                return addedMessage.FirstOrDefault();
            }
        }

        public async Task<IEnumerable<Message>> GetAll()
        {
            string query = "SELECT * FROM Messages";

            using (var connection = _context.CreateConnection())
            {
                var users = await connection.QueryAsync<Message>(query);
                return users.ToList();
            }
        }
      
    }
}
