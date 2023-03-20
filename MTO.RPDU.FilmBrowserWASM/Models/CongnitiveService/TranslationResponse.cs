namespace MTO.RPDU.FilmBrowserWASM.Models.CongnitiveService
{
    public class TranslationResponse
    {
        public List<Translation> translations { get; set; }
    }

    public class Translation
    {
        public string text { get; set; }
        public string to { get; set; }
    }
}
