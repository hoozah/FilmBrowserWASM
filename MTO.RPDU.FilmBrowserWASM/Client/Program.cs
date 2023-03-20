using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MTO.RPDU.FilmBrowserWASM.Contexts;
using MTO.RPDU.FilmBrowserWASM.HttpClients;
using MTO.RPDU.FilmBrowserWASM.Middlewares;
using MTO.RPDU.FilmBrowserWASM.Services;
using System.Diagnostics.CodeAnalysis;
using SqliteWasmHelper;
using Microsoft.EntityFrameworkCore;

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
            builder.Services.AddHttpClient<CognitiveServiceHttpClient>(client => client.BaseAddress = new Uri("https://api.cognitive.microsofttranslator.com"));

            //builder.Services.AddSingleton<DatabaseService<FilmDbContext>>();
            //builder.Services.AddFilmsFeature();
            builder.Services.AddScoped<FilmService>();
            builder.Services.AddSqliteWasmDbContextFactory<FilmDbContext>(opts => opts.UseSqlite("Data Source=films.db"));
            var host = builder.Build();
            await host.InitializeFilmsFeature();
            await host.RunAsync();

        }

        /// <summary>
        /// https://github.com/dotnet/efcore/issues/26860
        /// https://github.com/dotnet/aspnetcore/issues/39825
        /// FIXME: This is required for EF Core 6.0 as it is not compatible with trimming.
        /// </summary>
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)]
        private static Type _keepDateOnly = typeof(DateOnly);
    }
}