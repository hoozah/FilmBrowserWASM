using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MTO.RPDU.FilmBrowserWASM.HttpClients;
using MTO.RPDU.FilmBrowserWASM.Models.Omdb;
using MTO.RPDU.FilmBrowserWASM.Services;

namespace MTO.RPDU.FilmBrowserWASM.Pages
{
    public partial class Add
    {
        [Inject] private FilmService _filmService { get; set; } = default!;
        [Inject] private OmdbHttpClient _client { get; set; } = default!;
        [Inject] IJSRuntime JS { get; set; } = default!;

        private String? titleValue;
        private List<Search> searches = new List<Search>();
        private List<string> selectedTitles = new List<string>();

        private bool isChecklistVisible = false;
        private bool isSaved = false;

        private async Task Search()
        {
            isSaved = false;
            selectedTitles.Clear();

            await JS.InvokeVoidAsync("clearAllCheckbox");

            if (string.IsNullOrWhiteSpace(titleValue)) return;

            Console.WriteLine(titleValue ?? string.Empty);

            var response = await _client.SearchFilmsByTitle(titleValue?.Trim());
            
            if(response == null || response.Search == null || !response.Search.Any())
            {
                isChecklistVisible = false;
                return;
            }

            isChecklistVisible = true;
            searches = response.Search.Where(x => x.Type != null && !x.Type.Equals("Game")).ToList();
        }

        private void CheckboxChanged(ChangeEventArgs e, string imDbId)
        {
            if (e == null || e.Value == null) return;

            if ((bool)e.Value)
            {
                selectedTitles.Add(imDbId);
            }
            else
            {
                selectedTitles.Remove(imDbId);
            }
        }

        private async Task AddItems()
        {
            foreach(var imdbId in selectedTitles)
            {
                var filmData = await _client.GetFilmById(imdbId);
                await _filmService.AddFilmAsync(filmData);
            }

            isSaved = true;
            await ClearAsync();
        }

        private async Task ClearAsync()
        {
            titleValue = string.Empty;
            selectedTitles.Clear();
            isChecklistVisible = false;
            await JS.InvokeVoidAsync("clearAllCheckbox");
        }
    }
}
