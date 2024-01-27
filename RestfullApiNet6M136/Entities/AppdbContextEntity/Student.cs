using RestfullApiNet6M136.Entities.Common;

namespace RestfullApiNet6M136.Entities.AppdbContextEntity
{
    public class Student:BaseEntity
    {
        public string Name { get; set; }

        public int SchoolId { get; set; }
        public School School { get; set; }
    }
}
