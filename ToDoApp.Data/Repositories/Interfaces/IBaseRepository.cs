using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoApp.Data.Models;

namespace ToDoApp.Data.Repositories.Interfaces
{
    public interface IBaseRepository<T, K> where T : BaseEntity
    {
        Task<List<T>> GetAll();
        IQueryable<T> Find();
        Task<T> Get(K id);
        Task<T> Add(T entity);
        Task<T> Update(T entity);
        Task<T> Delete(K id);
    }

}
