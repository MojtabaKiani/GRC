using GRC.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace GRC.Infrastructure
{
    public class GRCContext : DbContext
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
