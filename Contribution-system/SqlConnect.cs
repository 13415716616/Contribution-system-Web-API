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

        public DbSet<Editor> Editors { get; set; }

        public DbSet<ManuscriptAuthor> ManuscriptAuthor { get; set; }

        public DbSet<ManuscriptContent> ManuscriptContent { get; set; }

        public DbSet<ManuscriptSubmitted> ManuscriptSubmitted { get; set; }

        public DbSet<ManuscriptReview> ManuscriptReview { get; set; }

        public DbSet<ChiefEditor> ChiefEditor { get; set; }
    }
}
