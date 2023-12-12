using Microsoft.EntityFrameworkCore;
using StudentControl.Domain.Model;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace StudentControl.Infrastructure
{
    public class Context : DbContext
    {


        public DbSet<Login> Logins { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Domain.Model.Group> Groups { get; set; }
        public DbSet<Order> Orders { get; set; }


        public Context(DbContextOptions<Context> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder) { }

    }
}
