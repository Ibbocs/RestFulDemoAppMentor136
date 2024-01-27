using RestfullApiNet6M136.Abstraction.IRepositories.IStudentRepos;
using RestfullApiNet6M136.Context;
using RestfullApiNet6M136.Entities.AppdbContextEntity;

namespace RestfullApiNet6M136.Implementation.Repositories.EntitiesRepos
{
    //bu clasin                bu uygulanmasi,     bu ise abstractionu, yeni ioc'de register ucun imzasidir.
    //2si bir birine baglidir, kilitdir sanki
    public class StudentRepo : Repository<Student>, IStudentRepository
    {
        /*1*/private readonly AppDbContext _appDbContext; //Bu repoya spesifik nese yazanda bunu inject eleyirsen ve hemin methodu interfacede de bildirirsen.(Qeyd injetsiz ctor zaten yaradilir, spesifik nese olanda 1 ve 2 elemek lazimdi, ctorun moterezisindeki zaten mecburu gelir
        public StudentRepo(AppDbContext appDbContext) : base(appDbContext)
        {
           /*2*/ _appDbContext = appDbContext;
        }

        //spesifik methodum
        public void Print(string text)
        {
            Console.WriteLine(text);
        }
    }
}
