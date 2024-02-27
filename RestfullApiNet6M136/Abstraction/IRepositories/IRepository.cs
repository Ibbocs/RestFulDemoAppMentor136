using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using RestfullApiNet6M136.Entities.Common;
using System.Linq.Expressions;

namespace RestfullApiNet6M136.Abstraction.IRepositories
{
    public interface IRepository<T> where T : BaseEntity
    {
        DbSet<T> Table { get; }

        IQueryable<T> GetAll();
        Task<T> GetByIdAsync(int id);

        Task<bool> AddAsync(T data);

        bool Remove(T data);
        Task<bool> RemoveById(int id);
  
        bool Update(T data);
    }

    
}
