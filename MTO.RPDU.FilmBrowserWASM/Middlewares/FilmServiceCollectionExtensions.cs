using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using MTO.RPDU.FilmBrowserWASM.Contexts;
using MTO.RPDU.FilmBrowserWASM.Services;

namespace MTO.RPDU.FilmBrowserWASM.Middlewares
{
    public static class FilmServiceCollectionExtensions
    {

        public static async Task InitializeFilmsFeature(this WebAssemblyHost host)
        {
            // Sync Films
            var Filmservice = host.Services.GetRequiredService<FilmService>();
            await Filmservice.InitializeAsync();
        }
    }
}
