namespace PruebaTecnica.Models
{
    public class SearchHistory
    {
        public int Id { get; set; }
        public DateTime Fecha { get; set; }
        public string? CatFact { get; set; }
        public string? Query { get; set; }
        public string? GifUrl { get; set; }
    }

}
