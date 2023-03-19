using Microsoft.AspNetCore.Components;
using MTO.RPDU.FilmBrowserWASM.Models.FilmBrowser;
using MTO.RPDU.FilmBrowserWASM.Services;

namespace MTO.RPDU.FilmBrowserWASM.Pages
{
    public partial class Index
    {
        [Inject] private FilmService _filmService { get; set; } = default!;

        private List<Film> films;

        protected override async Task OnInitializedAsync()
        {
            if (films == null) films = new List<Film>();

            films = await _filmService.ListAsync();


            //var response = await client.SearchFilmsByTitle("Harry Potter");
            //var films = new List<FilmResult>();
            //var sb = new StringBuilder();

            //foreach(var record in response.Search)
            //{
            //    var film = await client.GetFilmById(record.imdbID!);
            //    filmResults.Add(film);
            //}


        }
    }
}
