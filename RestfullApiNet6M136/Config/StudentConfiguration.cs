using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore;
using RestfullApiNet6M136.Entities.AppdbContextEntity;

namespace RestfullApiNet6M136.Config
{
    public class StudentConfiguration: IEntityTypeConfiguration<Student>
    {
        public void Configure(EntityTypeBuilder<Student> builder)
        {
            builder.Property(b => b.Name)//string
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(b => b.SchoolId)//int
                .IsRequired();
        }
    }
}
