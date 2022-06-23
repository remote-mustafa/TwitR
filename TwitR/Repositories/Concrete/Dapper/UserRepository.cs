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
    public class UserRepository : IEntityRepository<User>
    {
        private readonly DapperContext _context;

        public UserRepository(DapperContext context)
        {
            _context = context;
        }

        public async Task<User> AddAsync(User entity)
        {
            string query = "INSERT INTO Users (UserName, FirstName, LastName,Age,CreatedDate) OUTPUT INSERTED.* VALUES (@UserName, @FirstName, @LastName,@Age,@CreatedDate)";

            entity.CreatedDate = DateTime.Now;

            var parameters = new DynamicParameters();
            parameters.Add("UserName", entity.UserName, DbType.String);
            parameters.Add("FirstName", entity.FirstName, DbType.String);
            parameters.Add("LastName", entity.LastName, DbType.String);
            parameters.Add("Age", entity.Age, DbType.Int32);
            parameters.Add("CreatedDate", entity.CreatedDate, DbType.DateTime2);

            using (var connection = _context.CreateConnection())
            {
                var addedUser = await connection.QueryFirstAsync<User>(query, parameters);
                return addedUser;
            }           
        }

        public async Task<IEnumerable<User>> GetAll()
        {
            string query = "SELECT * FROM Users";

            using (var connection = _context.CreateConnection())
            {
                var users = await connection.QueryAsync<User>(query);
                return users.ToList();
            }
        }
    }
}
