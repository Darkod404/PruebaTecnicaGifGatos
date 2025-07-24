using Microsoft.AspNetCore.Mvc;
using PruebaTecnica.Data;
using PruebaTecnica.Models;
using Microsoft.EntityFrameworkCore;

namespace PruebaTecnica.Controllers
{
    [ApiController]
    [Route("api")]
    public class CatGifController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IHttpClientFactory _httpClientFactory;

        public CatGifController(AppDbContext context, IHttpClientFactory httpClientFactory)
        {
            _context = context;
            _httpClientFactory = httpClientFactory;
        }

        [HttpGet("fact")]
        public async Task<IActionResult> GetFact()
        {
            try
            {
                var client = _httpClientFactory.CreateClient();
                var response = await client.GetAsync("https://catfact.ninja/fact");
                if (!response.IsSuccessStatusCode)
                    return StatusCode(502, "Error al consultar CatFact API");

                var data = await response.Content.ReadFromJsonAsync<CatFactResponse>();
                return Ok(new { fact = data?.Fact ?? "No fact available" });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[fact] ERROR: {ex.Message}\n{ex.StackTrace}");
                return StatusCode(500, "Error interno en el endpoint de fact.");
            }
        }

        [HttpGet("gif")]
        public async Task<IActionResult> GetGif([FromQuery] string query, [FromQuery] int? offset)
        {
            var apiKey = "voaNIOg1u7ONPbckzWK71C48YqCOkhVP";
            var offsetValue = offset ?? 0;
            var client = _httpClientFactory.CreateClient();
            var giphyUrl = $"https://api.giphy.com/v1/gifs/search?api_key={apiKey}&q={query}&limit=1&offset={offsetValue}";
            var response = await client.GetAsync(giphyUrl);
            if (!response.IsSuccessStatusCode)
                return StatusCode(502, "Error al consultar Giphy API");

            var data = await response.Content.ReadFromJsonAsync<GiphyResponse>();
            var gifUrl = data?.Data.FirstOrDefault()?.Images.Original.Url;

            var history = new SearchHistory
            {
                Fecha = DateTime.UtcNow,
                CatFact = Request.Headers["catfact"].FirstOrDefault() ?? "",
                Query = query,
                GifUrl = gifUrl
            };
            _context.SearchHistories.Add(history);
            await _context.SaveChangesAsync();

            return Ok(new { gifUrl });
        }


        [HttpGet("history")]
        public async Task<IActionResult> GetHistory()
        {
            try
            {
                var items = await _context.SearchHistories
                    .OrderByDescending(h => h.Fecha)
                    .Take(100)
                    .ToListAsync();
                return Ok(items);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[history] ERROR: {ex.Message}\n{ex.StackTrace}");
                return StatusCode(500, "Error interno en el endpoint de history.");
            }
        }
    }
    public class CatFactResponse
    {
        public string? Fact { get; set; }
    }

    public class GiphyResponse
    {
        public List<GifData>? Data { get; set; }
    }

    public class GifData
    {
        public GifImages? Images { get; set; }
    }

    public class GifImages
    {
        public GifOriginal? Original { get; set; }
    }

    public class GifOriginal
    {
        public string? Url { get; set; }
    }
}
