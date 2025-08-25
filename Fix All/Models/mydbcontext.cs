using Fix_All.Models;
using Microsoft.EntityFrameworkCore;

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



    }
}
