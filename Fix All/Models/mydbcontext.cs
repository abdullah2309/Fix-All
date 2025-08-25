using Fix_All.Models;
using Microsoft.EntityFrameworkCore;
using YourProject.Models;

namespace Fix_All.Models
{
    public class mydbcontext :DbContext
    {
        public mydbcontext(DbContextOptions options ) :base(options)
        {
        }
        // public DbSet<Add_labor_Category> add_Labor_Categories { get; set;}

        public DbSet<ServiceProvider> ServiceProviders { get; set; }
        public DbSet<LaborField> LaborFields { get; set; }
        public DbSet<approve_laber> approve_labers { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<UserAccount> UserAccounts { get; set; }
        public DbSet<BookNow> BookNow { get; set; }
        public DbSet<AdminLogin> adminLogins { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Prevent multiple cascade paths
            modelBuilder.Entity<BookNow>()
                .HasOne(b => b.ApproveLaber)
                .WithMany()
                .HasForeignKey(b => b.ApproveLarberId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<BookNow>()
                .HasOne(b => b.UserAccount)
                .WithMany()
                .HasForeignKey(b => b.UserId)
                .OnDelete(DeleteBehavior.Restrict);
        }



    }
}
