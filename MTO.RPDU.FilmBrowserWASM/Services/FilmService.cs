using Microsoft.EntityFrameworkCore;
using MTO.RPDU.FilmBrowserWASM.Contexts;
using MTO.RPDU.FilmBrowserWASM.HttpClients;
using MTO.RPDU.FilmBrowserWASM.Models.FilmBrowser;
using MTO.RPDU.FilmBrowserWASM.Models.Omdb;
using System.Net.Http.Json;

namespace MTO.RPDU.FilmBrowserWASM.Services
{
    public class FilmService
    {

        private readonly IDbContextFactory<FilmDbContext> _factory;
        private readonly HttpClient _httpClient;
        private readonly CognitiveServiceHttpClient _cognitiveServiceHttpClient;


        private bool _hasSynced = false;
        public FilmService(IDbContextFactory<FilmDbContext> factory, HttpClient httpClient, CognitiveServiceHttpClient cognitiveServiceHttpClient)
        {
            _factory = factory;
            _httpClient = httpClient;
            _cognitiveServiceHttpClient = cognitiveServiceHttpClient;
        }

        public async Task InitializeAsync()
        {
            if (_hasSynced) return;

            await using var dbContext = await _factory.CreateDbContextAsync();
            dbContext.Database.EnsureCreated();
            
            if (dbContext.Films.Count() > 0) return;

            var resultList = await _httpClient.GetFromJsonAsync<List<FilmResult>>("/sample-data/film.json");
            if (resultList != null)
            {
                foreach(var result in resultList)
                {
                    var filmData = MapToFilm(result);

                    //var plotFr = await _cognitiveServiceHttpClient.TransalateAsync(result.Plot);
                    //var genreFr = await _cognitiveServiceHttpClient.TransalateAsync(result.Genre);

                    //filmData.PlotFr = plotFr;
                    //filmData.GenreFr = genreFr;

                    await dbContext.Films.AddAsync(filmData);
                }
            }
            await dbContext.SaveChangesAsync();
        }

        public async Task AddFilmAsync(FilmResult result)
        {
            await using var dbContext = await _factory.CreateDbContextAsync();

            var film = MapToFilm(result);

            var existingFilm = await dbContext.Films.FirstOrDefaultAsync(x => x.ImdbId != null && x.ImdbId.Equals(film.ImdbId));

            if (existingFilm == null)
            {
                await dbContext.Films.AddAsync(film);
                await dbContext.SaveChangesAsync();
            }
        }

        public async Task<List<Film>> ListAsync()
        {
            await using var dbContext = await _factory.CreateDbContextAsync();
            return await dbContext.Films.ToListAsync();
        }

        // refactor to automapper
        private Film MapToFilm(FilmResult result)
        {
            Film newFilm = new();
            newFilm.Title = result.Title;
            newFilm.Year = result.Year;
            newFilm.Poster = result.Poster;
            newFilm.Genre = result.Genre;
            newFilm.Actors = result.Actors;
            newFilm.Plot = result.Plot;
            newFilm.ImdbId = result.imdbID;
            return newFilm;
        }
    }
}
