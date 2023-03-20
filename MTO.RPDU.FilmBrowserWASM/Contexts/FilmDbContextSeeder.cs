namespace MTO.RPDU.FilmBrowserWASM.Contexts
{
    public class FilmDbContextSeeder
    {
        public static async void Seed(FilmDbContext dbContext)
        {
            dbContext.Database.EnsureCreated();
            await dbContext.SaveChangesAsync();
        }
    }
}
