using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RestfullApiNet6M136.Entities.AppdbContextEntity;
using RestfullApiNet6M136.Entities.Identity;
using System.Reflection;

namespace RestfullApiNet6M136.Context
{
    public class AppDbContext : IdentityDbContext<AppUser, AppRole, string>
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }


        public DbSet<Student> Students { get; set; }
        public DbSet<School> Schools { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
            //modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            //modelBuilder.Entity<School>()
            //    .HasMany(sc => sc.Students)
            //    .WithOne(st => st.School)
            //    .HasForeignKey(st => st.SchoolId);

            modelBuilder.Entity<Student>()
                .HasOne<School>(st => st.School)
                .WithMany(sc => sc.Students)
                .HasForeignKey(st => st.SchoolId)
                .OnDelete(DeleteBehavior.Cascade);

        }
    }
}
