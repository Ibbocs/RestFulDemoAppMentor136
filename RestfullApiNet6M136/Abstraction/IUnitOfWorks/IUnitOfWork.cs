using Microsoft.EntityFrameworkCore;
using RestfullApiNet6M136.Abstraction.IRepositories;
using RestfullApiNet6M136.Context;
using RestfullApiNet6M136.Entities.AppdbContextEntity;
using RestfullApiNet6M136.Entities.Common;
using RestfullApiNet6M136.Implementation.Repositories;
using System.Threading.Tasks;

namespace RestfullApiNet6M136.Abstraction.IUnitOfWorks
{
    //public interface IUnitOfWork : IDisposable
    //{
    //    IRepository<School> SchoolRepo { get; }
    //    IRepository<Student> StudentRepo { get; }
    //    Task<int> SaveChangesAsync();
    //}

    public interface IUnitOfWork : IDisposable
    {
        IRepository<TEntity> GetRepository<TEntity>() where TEntity : BaseEntity;
        Task<int> SaveChangesAsync();
    }

    //public interface IUnitOfWork<out TContext> /*: IDisposable */where TContext : DbContext
    //{
    //    IRepository<TEntity> GetRepository<TEntity>() where TEntity : BaseEntity;
    //    Task<int> SaveChangesAsync();
    //}

    //public interface IUnitOfWork<TContext, TEntity> : IDisposable where TContext : AppDbContext
    //{
    //    IRepository<TEntity> GetRepository<TEntity>() where TEntity : BaseEntity;
    //    Task<int> SaveChangesAsync();
    //}
}
