using DL.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DL.DBContext
{
    public class DBContext : DbContext
    {

        public DBContext(DbContextOptions<DBContext> options)
        : base(options)
        {
            ChangeTracker.LazyLoadingEnabled = false;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {


            base.OnModelCreating(modelBuilder);
        }
       public virtual DbSet<User> User { get; set; }
       public virtual DbSet<Post> Post { get; set; }
       public virtual DbSet<Like> Like { get; set; }
       public virtual DbSet<Comment> Comment { get; set; }


    }
}
