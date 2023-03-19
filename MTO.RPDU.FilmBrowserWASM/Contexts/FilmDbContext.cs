using Microsoft.EntityFrameworkCore;
using Microsoft.JSInterop;
using MTO.RPDU.FilmBrowserWASM.Models.FilmBrowser;

namespace MTO.RPDU.FilmBrowserWASM.Contexts
{
    public class FilmDbContext : DbContext
    {
        public DbSet<Film> Films { get; set; }

        private readonly Lazy<Task<IJSObjectReference>> _moduleTask;

        public FilmDbContext(DbContextOptions<FilmDbContext> options)
        : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Film>()
                .HasKey(f => new { f.ImdbId });


            base.OnModelCreating(modelBuilder);
        }
    }
}
