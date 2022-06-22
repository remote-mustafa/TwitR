using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TwitR.Models.Entities.Abstract;
using TwitR.Repositories.Abstract;
using TwitR.Repositories.Concrete.Dapper.Context;

namespace TwitR.Repositories.Concrete.Dapper
{
    //public class DapperEntityRepositoryBase<T> : IEntityRepository<T> where T : BaseEntity, new()
    //{

    //    private readonly DapperContext _context;
    //    private readonly string _tableName;

    //    public DapperEntityRepositoryBase(DapperContext context, string tableName)
    //    {
    //        _context = context;
    //        _tableName = tableName;
    //    }

    //    public IEnumerable<T> Query(string query, object parameters = null)
    //    {
    //        using(var connection = _context.CreateConnection())
    //        {
    //            return connection.Query<T>(query, parameters).ToList();
    //        }
    //    }
    //}
}
