using Abp.Domain.Entities;
using Abp.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace LockthreatCompliance.EntityFrameworkCore.RepositoryExtensions
{
    public static class RepositoryExtensions
    {
        public static async Task<T> GetIncluding<T>(this IRepository<T> repository, Expression<Func<T, bool>> filter, params string[] includes) where T : class, IEntity
        {
            return await includes.
                Aggregate(repository.GetAll(), (prev, cur) => prev.Include(cur))
                .FirstOrDefaultAsync(filter);
        }

        public static async Task InsertAllAsync<T>(this IRepository<T> repository, IEnumerable<T> entities) where T : class, IEntity
        {
            foreach (var entity in entities)
            {
                await repository.InsertAsync(entity);
            }
        }
    }
}
