using Microsoft.EntityFrameworkCore;
using RestfullApiNet6M136.Abstraction.IRepositories;
using RestfullApiNet6M136.Abstraction.IUnitOfWorks;
using RestfullApiNet6M136.Context;
using RestfullApiNet6M136.Entities.AppdbContextEntity;
using RestfullApiNet6M136.Entities.Common;
using RestfullApiNet6M136.Implementation.Repositories;
using System.Security.AccessControl;

namespace RestfullApiNet6M136.Implementation.UnitOfWorks
{
    //public class UnitOfWork : IUnitOfWork
    //{
    //    private readonly AppDbContext appDbContext;
    //    private IRepository<School> schoolrepository;
    //    private IRepository<Student> studentrepository;

    //    public UnitOfWork(AppDbContext _appDbContext)
    //    {
    //        this.appDbContext = _appDbContext;
    //    }
    //    public IRepository<School> SchoolRepo => schoolrepository ??= new Repository<School>(appDbContext);
    //    public IRepository<Student> StudentRepo => studentrepository ??= new Repository<Student>(appDbContext);

    //    //Idatabase interface var ef in
    //    public void Dispose()
    //    {
    //        appDbContext.Dispose();
    //    }

    //    public async Task<int> SaveChangesAsync()
    //    {
    //        return await appDbContext.SaveChangesAsync();
    //    }
    //}

    public class UnitOfWork : IUnitOfWork
    {
        //private readonly DbContext context;
        private readonly AppDbContext context;
        //private IRepository<T> repository;
        private Dictionary<Type, object> _repositories;

        public UnitOfWork(AppDbContext _context)
        {
            context = _context;
            _repositories = new Dictionary<Type, object>();
        }

        //public IRepository<T> Repository
        //{
        //    get
        //    {
        //        if (repository == null)
        //        {
        //            repository = new Repository<T>(context);
        //        }
        //        return repository;
        //    }
        //}

        public void Dispose()
        {
            context.Dispose();
        }

        public IRepository<TEntity> GetRepository<TEntity>() where TEntity : BaseEntity
        {
            if (_repositories.ContainsKey(typeof(TEntity)))
            {
                //return _repositories[typeof(TEntity)] as IRepository<TEntity>;
                return (IRepository<TEntity>)_repositories[typeof(TEntity)];
            }


            IRepository<TEntity> repository = new Repository<TEntity>(context /*as AppDbContext*/);
            _repositories.Add(typeof(TEntity), repository);
            return repository;
        }

        public async Task<int> SaveChangesAsync()
        {
            return await context.SaveChangesAsync();
        }
    }
    //burda DbContext olur
    //public class UnitOfWork<TContext> : IUnitOfWork<TContext> where TContext : AppDbContext, new()
    //{
    //    private readonly TContext _context;
    //    //public TContext Context { get; }
    //    private Dictionary<Type, object> _repositories;

    //    public UnitOfWork()
    //    {
    //        _context = new TContext();
    //        _repositories = new Dictionary<Type, object>();
    //    }

    //    public IRepository<TEntity> GetRepository<TEntity>() where TEntity : BaseEntity
    //    {
    //        if (_repositories.ContainsKey(typeof(TEntity)))
    //        {
    //            return _repositories[typeof(TEntity)] as IRepository<TEntity>;
    //        }


    //        IRepository<TEntity> repository = new Repository<TEntity>(_context);
    //        _repositories.Add(typeof(TEntity), repository);
    //        return repository;
    //    }

    //    public async Task<int> SaveChangesAsync()
    //    {
    //        _context.SaveChangesAsync();
    //        return await _context.SaveChangesAsync();
    //    }

    //    public void Dispose()
    //    {
    //        _context.Dispose();
    //    }
    //}

    //Program.cs de:
    //builder.Services.AddScoped(typeof(IUnitOfWork<>), typeof(UnitOfWork<>));

    //public class UnitOfWork<TContext, TEntity> : IUnitOfWork<TContext, TEntity> where TContext : AppDbContext, new() where TEntity : BaseEntity
    //{
    //    private readonly TContext _context;
    //    private readonly TEntity _entity;
    //    private Dictionary<Type, object> _repositories;
    //    private IRepository<TEntity> myRepository;

    //    public UnitOfWork()
    //    {
    //        _context = new TContext();
    //        _repositories = new Dictionary<Type, object>();
    //    }

    //    public IRepository<TEntity> GetRepository<TEntity>() where TEntity : BaseEntity
    //    {
    //        if (_repositories.ContainsKey(typeof(TEntity)))
    //        {
    //            return _repositories[typeof(TEntity)] as IRepository<TEntity>;
    //        }

    //        IRepository<TEntity> repository = new Repository<TEntity>(_context);
    //        _repositories.Add(typeof(TEntity), repository);
    //        return repository;
    //    }

    //    public async Task<int> SaveChangesAsync()
    //    {
    //        return await _context.SaveChangesAsync();
    //    }

    //    public void Dispose()
    //    {
    //        _context.Dispose();
    //    }
    //}
}
