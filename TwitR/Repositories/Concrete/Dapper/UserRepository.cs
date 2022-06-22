using Dapper;
using System.Collections.Generic;
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

        public async Task AddAsync(User entity)
        {
            /*
             public string UserName { get; set;
             public string FirstName { get; set
             public string LastName { get; set;
             public ushort? Age { get; set; }
             */
            string query = "INSERT INTO Users (UserName, FirstName, LastName,Age,CreatedDate) VALUES (@UserName, @FirstName, @LastName,@Age,@CreatedDate)";
            var parameters = new DynamicParameters();
            parameters.Add("UserName", entity.UserName);
            parameters.Add("FirstName", entity.FirstName);
            parameters.Add("LastName", entity.LastName);
            parameters.Add("Age", entity.Age);
            parameters.Add("CreatedDate", entity.CreatedDate);

            using (var connection = _context.CreateConnection())
            {
                await connection.ExecuteAsync(query, parameters);
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
