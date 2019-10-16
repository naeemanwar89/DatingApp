using Microsoft.EntityFrameworkCore;
using DatingApp.API.Models;

namespace DatingApp.API.Data
{
    public class DataContext :DbContext
    {
        /// <summary>
        /// powe shell commands for migarations are
        /// enable migration
        /// add-migration initialcreate
        /// update-database 
        /// </summary>
        /// <param name="dbContextOptions"></param>
        public DataContext(DbContextOptions<DataContext> dbContextOptions):base(dbContextOptions){}
        public DbSet<Value> Values { get; set; }
    }
}
