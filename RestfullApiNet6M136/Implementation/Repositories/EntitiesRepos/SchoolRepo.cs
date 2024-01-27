using RestfullApiNet6M136.Abstraction.IRepositories.ISchoolRepos;
using RestfullApiNet6M136.Context;
using RestfullApiNet6M136.Entities.AppdbContextEntity;

namespace RestfullApiNet6M136.Implementation.Repositories.EntitiesRepos
{
    //bu clasin             bu uygulanmasi,     bu ise abstractionu, yeni ioc'de register ucun imzasidir.
    //2si bir birine baglidir, kilitdir sanki
    public class SchoolRepo : Repository<School>, ISchoolRepository
    {
        public SchoolRepo(AppDbContext appDbContext) : base(appDbContext)
        {
        }
    }
}
