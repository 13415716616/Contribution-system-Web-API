using Contribution_system_Models.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Contribution_system_Models
{
    public class SqlConnect: DbContext
    {
        public SqlConnect(DbContextOptions<SqlConnect> options) : base(options)
        {

        }
        public SqlConnect()
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=106.12.175.105;Database=Contribution-system;MultipleActiveResultSets=true;uid=ContributeTest;pwd=ContributeTest!");
        }


        public DbSet<Author> Authors { get; set; }

        public DbSet<DraftManuscript> DraftManuscript { get; set; }

        public DbSet<Editor> Editors { get; set; }

        public DbSet<ManuscriptAuthor> ManuscriptAuthor { get; set; }

        public DbSet<ManuscriptContent> ManuscriptContent { get; set; }

        public DbSet<ManuscriptSubmitted> ManuscriptSubmitted { get; set; }

        public DbSet<ManuscriptReview> ManuscriptReview { get; set; }

        public DbSet<ChiefEditor> ChiefEditor { get; set; }

        public DbSet<CompleteManuscript> CompleteManuscript { get; set; }

    }
}
