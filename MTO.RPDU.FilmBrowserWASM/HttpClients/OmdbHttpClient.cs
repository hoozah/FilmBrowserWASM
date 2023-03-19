using MTO.RPDU.FilmBrowserWASM.Models.Omdb;
using System.Net.Http.Json;
using System.Text.Json;

namespace MTO.RPDU.FilmBrowserWASM.HttpClients
{
    public class OmdbHttpClient
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<OmdbHttpClient> _logger;

        public OmdbHttpClient(HttpClient httpClient, ILogger<OmdbHttpClient> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<SearchResult> SearchFilmsByTitle(string title)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(title)) 
                {
                    _logger.LogWarning($"OmdbHttpClient.SearchFilmsByTitle - title is null or empty string");
                    return null!; 
                }

                _logger.LogInformation($"OmdbHttpClient.SearchFilmsByTitle - Initiating Search for {title}");
                return await _httpClient.GetFromJsonAsync<SearchResult>($"?apikey=38514c56&type=movie&s={title}");
            
            }
            catch (Exception ex)
            {
                _logger.LogError($"OmdbHttpClient.SearchFilmsByTitle - Unable to retrieve search result. {ex.Message}");
                return null!;
            }
        }

        public async Task<FilmResult> GetFilmById(string imdbId)
        {
            try
            {
                if(string.IsNullOrWhiteSpace(imdbId))
                {
                    _logger.LogWarning($"OmdbHttpClient.GetFilmById - imdbId is null or empty string");
                    return null!;
                }
                _logger.LogInformation($"OmdbHttpClient.SearchFilmsByTitle - Initiating Get Film for {imdbId}");
                return await _httpClient.GetFromJsonAsync<FilmResult>($"?apikey=38514c56&plot=short&i={imdbId}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"OmdbHttpClient.GetFilmById - Unable to retrieve film info. {ex.Message}");
                return null!;
            }
        }
    }
}
