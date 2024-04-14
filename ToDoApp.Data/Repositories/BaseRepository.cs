using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoApp.Data.DataContext;
using ToDoApp.Data.Models;
using ToDoApp.Data.Repositories.Interfaces;

namespace ToDoApp.Data.Repositories
{
    public abstract class BaseRepository<T, K> : IBaseRepository<T, K> where T : BaseEntity
    {
        protected ToDoDataContext context;
        protected DbSet<T> _dbSet;

        public BaseRepository(ToDoDataContext context)
        {
            this.context = context;
            _dbSet = this.context.Set<T>();
        }

        public virtual async Task<T> Add(T entity)
        {
            _dbSet.Add(entity);
            await context.SaveChangesAsync();
            return entity;
        }

        public virtual async Task<T> Delete(K id)
        {
            var entity = await _dbSet.FindAsync(id);
            if (entity == null)
            {
                return entity;
            }

            _dbSet.Remove(entity);
            await context.SaveChangesAsync();

            return entity;
        }

        public virtual async Task<T> Get(K id)
        {
            var result = await _dbSet.FindAsync(id);
            return result;
        }

        public virtual async Task<List<T>> GetAll()
        {
            return await _dbSet.ToListAsync();
        }

        public virtual async Task<T> Update(T entity)
        {
            context.Entry(entity).State = EntityState.Modified;
            await context.SaveChangesAsync();
            return entity;
        }
        public virtual IQueryable<T> Find()
        {
            return _dbSet;
        }


    }

}
