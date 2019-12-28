using Contribution_system_Models.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Contribution_system
{
    public class SqlConnect : DbContext
    {
        public SqlConnect(DbContextOptions<SqlConnect> options) : base(options)
        {

        }

        public DbSet<Author> Authors { get; set; }

        public DbSet<Manuscript> Manuscript { get; set; }
    }
}
