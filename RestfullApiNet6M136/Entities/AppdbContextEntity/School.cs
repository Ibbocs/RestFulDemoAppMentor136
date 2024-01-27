using RestfullApiNet6M136.Entities.Common;
using System.ComponentModel.DataAnnotations;

namespace RestfullApiNet6M136.Entities.AppdbContextEntity
{
    public class School:BaseEntity
    {
        [Required]
        public int Number { get; set; }
        public string Name { get; set; }

        public ICollection<Student> Students { get; set; }
    }
}
