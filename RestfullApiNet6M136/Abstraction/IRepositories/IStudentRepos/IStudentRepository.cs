using RestfullApiNet6M136.Entities.AppdbContextEntity;

namespace RestfullApiNet6M136.Abstraction.IRepositories.IStudentRepos
{
    public interface IStudentRepository:IRepository<Student>
    {
        void Print(string text);
    }
}
