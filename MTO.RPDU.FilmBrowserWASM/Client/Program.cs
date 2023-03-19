using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MTO.RPDU.FilmBrowserWASM;
using MTO.RPDU.FilmBrowserWASM.Clients;

namespace MTO.RPDU.FilmBrowserWASM
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");
            builder.RootComponents.Add<HeadOutlet>("head::after");

            builder.Logging.SetMinimumLevel(LogLevel.Information);

            // API initialization
            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
            builder.Services.AddHttpClient<OmdbHttpClient>(client => client.BaseAddress = new Uri("https://www.omdbapi.com/"));

            await builder.Build().RunAsync();
        }
    }
}