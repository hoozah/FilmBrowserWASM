using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MTO.RPDU.FilmBrowserWASM.Models.FilmBrowser;
using MTO.RPDU.FilmBrowserWASM.Services;

namespace MTO.RPDU.FilmBrowserWASM.Pages
{
    public partial class Index
    {
        [Inject] private FilmService _filmService { get; set; } = default!;
        [Inject] IJSRuntime JS { get; set; } = default!;

        private List<Film> films;

        protected override async Task OnInitializedAsync()
        {
            if (films == null) films = new List<Film>();

            films = await _filmService.ListAsync();
        }

        private async Task ScrollToTop()
        {
            await JS.InvokeVoidAsync("scrollToTop");
        }
    }
}
