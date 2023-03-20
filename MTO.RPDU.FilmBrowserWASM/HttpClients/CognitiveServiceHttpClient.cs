using MTO.RPDU.FilmBrowserWASM.Models.CongnitiveService;
using System.Net.Http.Headers;
using System;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace MTO.RPDU.FilmBrowserWASM.HttpClients
{
    public class CognitiveServiceHttpClient
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<OmdbHttpClient> _logger;

        public CognitiveServiceHttpClient(HttpClient httpClient, ILogger<OmdbHttpClient> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<string> TransalateAsync(string text)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(text))
                {
                    _logger.LogWarning($"CognitiveServiceHttpClient.Transalate - text is null or empty string");
                    return null!;
                }

                var body = new List<TranslationRequest>();
                body.Add(new TranslationRequest() { Text = text });
                _logger.LogInformation($"CognitiveServiceHttpClient.Transalate - Initiating Translation for {text}");
                _httpClient.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", "b8c74a423710421886de3d7fd0d34993");
                _httpClient.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Region", "eastus");
                var response = await _httpClient.PostAsJsonAsync($"translate?api-version=3.0&from=en&to=fr", body);

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError($"CognitiveServiceHttpClient.Transalate - Unable to retrieve result. {await response.Content.ReadAsStringAsync()}");
                    return string.Empty;
                }

                var responseContent = await response.Content.ReadAsStringAsync();
                JArray jsonResponse = JArray.Parse(responseContent);
                return (string)jsonResponse[0]["translations"][0]["text"];
              
            }
            catch (Exception ex)
            {
                _logger.LogError($"CognitiveServiceHttpClient.Transalate - Unable to retrieve result. {ex.Message}");
                return null!;
            }
        }
    }
}
