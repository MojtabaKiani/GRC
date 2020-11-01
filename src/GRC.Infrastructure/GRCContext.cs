using GRC.Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace GRC.Infrastructure
{
    public class GRCContext : IdentityDbContext
    {
        public GRCContext(DbContextOptions<GRCContext> option) : base(option)
        {
        }
        public DbSet<StandardCategory> StandardCategories { get; set; }

        public DbSet<Standard> Standards { get; set; }

        public DbSet<Domain> Domains { get; set; }

        public DbSet<Control> Controls { get; set; }

        public DbSet<Question> Questions { get; set; }

        public DbSet<Questionary> Questionaries { get; set; }

        public DbSet<Answer> Answers { get; set; }

        public DbSet<QuestionAnswer> QuestionAnswers { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Answer>().HasOne(s => s.Question).WithMany(q => q.Answers).OnDelete(DeleteBehavior.NoAction);
            builder.Entity<Answer>().HasOne(s => s.Questionary).WithMany(q => q.Answers).OnDelete(DeleteBehavior.Cascade);
            base.OnModelCreating(builder);
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        }
    }
}
