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
        Task<IEnumerable<T>> GetAll();
        Task<T> AddAsync(T entity);
    }
}
