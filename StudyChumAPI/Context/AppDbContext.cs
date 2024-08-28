using Microsoft.EntityFrameworkCore;
using StudyChumAPI.Models;

namespace StudyChumAPI.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options): base(options)
        {
                
        }

        public DbSet<User> Users { get; set; }
        public DbSet<UserTask> Tasks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().ToTable("users");
            modelBuilder.Entity<UserTask>().ToTable("tasks");
        }
    }
}
 