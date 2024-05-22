using FunlabProgramChallenge.Models;
using Microsoft.EntityFrameworkCore;

namespace FunlabProgramChallenge.Model
{
    public partial class AppDbContext : DbContext
    {
        public AppDbContext()
        {
        }

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }
        public DbSet<Member> Member { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
