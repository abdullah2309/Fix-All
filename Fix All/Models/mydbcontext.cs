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

    }
}
