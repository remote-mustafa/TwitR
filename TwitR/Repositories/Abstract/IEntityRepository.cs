using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TwitR.Models.Entities.Abstract;

namespace TwitR.Repositories.Abstract
{
    public interface IEntityRepository<T> where T : BaseEntity, new()
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> AddAsync(T entity);
    }
}
