using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore;
using RestfullApiNet6M136.Abstraction.IRepositories;
using RestfullApiNet6M136.Entities.Common;
using System.Linq.Expressions;
using RestfullApiNet6M136.Context;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq;

namespace RestfullApiNet6M136.Implementation.Repositories
{
    public class Repository<T> : IRepository<T> where T : BaseEntity
    {
        private readonly AppDbContext appDbContext;

        public Repository(AppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }

        //...bu conextde guya T tipinde entiti oldugnu (genericde) bildirmek ucundu
        public DbSet<T> Table => appDbContext.Set<T>();

        public async Task<bool> AddAsync(T data)
        {
            EntityEntry<T> entityEntry = await Table.AddAsync(data);
            return entityEntry.State == EntityState.Added;
        }


        public IQueryable<T> GetAll()
        {
            //query halinda table aliram
            var query = Table.AsQueryable();
            return query;
        }

        public async Task<T> GetByIdAsync(int id)
        //=> await Table.FirstOrDefaultAsync(data => data.Id == id);
        //=> await Table.FindAsync(id);
        {
            //var data = await Table.FindAsync(id);
            //return data;
            var query = Table.AsQueryable();

            return await query.FirstOrDefaultAsync(data => data.Id == id);
        }

        public IQueryable<T> Query()
        {
            return appDbContext.Set<T>();
        }

        public bool Remove(T data)
        {
            EntityEntry<T> entityEntry = Table.Remove(data);
            return entityEntry.State == EntityState.Deleted;
        }

        public async Task<bool> RemoveById(int id)
        {
            T data = await Table.FindAsync(id);
            return Remove(data);
        }

        public bool Update(T data)
        {
            EntityEntry<T> entityEntry = Table.Update(data);
            return entityEntry.State == EntityState.Modified;
        }


        //public Task<bool> AddRangeAsync(ICollection<T> datas)
        //{
        //    throw new NotImplementedException();
        //}

        //public bool RemoveRange(ICollection<T> datas)
        //{
        //    throw new NotImplementedException();
        //}

        //public bool UpdateRange(ICollection<T> datas)
        //{
        //    throw new NotImplementedException();
        //}

    }
}
