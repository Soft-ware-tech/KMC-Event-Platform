using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using KMCWeb.Models;
using System.Text.Json;
using System.Web;

namespace KMCWeb.Pages
{
    public class EventsModel : PageModel
    {
        private readonly IHttpClientFactory httpClientFactory;
        private static readonly JsonSerializerOptions JsonOpts = new() { PropertyNameCaseInsensitive = true };

        public List<EventModel> Results { get; set; } = new();
        public string[] Categories => EventCategories.All;

        [BindProperty(SupportsGet = true)]
        public string? Keyword { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? Category { get; set; }

        [BindProperty(SupportsGet = true)]
        public DateTime? Date { get; set; }

        public EventsModel(IHttpClientFactory _httpClientFactory)
        {
            httpClientFactory = _httpClientFactory;
        }

        public async Task OnGet()
        {
            var client = httpClientFactory.CreateClient("KmcApi");

            var query = new List<string>();
            if (!string.IsNullOrWhiteSpace(Keyword)) query.Add("keyword=" + HttpUtility.UrlEncode(Keyword));
            if (!string.IsNullOrWhiteSpace(Category)) query.Add("category=" + HttpUtility.UrlEncode(Category));
            if (Date.HasValue) query.Add("date=" + Date.Value.ToString("yyyy-MM-dd"));

            string url = "api/Event/search" + (query.Count > 0 ? "?" + string.Join("&", query) : "");

            var response = await client.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                Results = JsonSerializer.Deserialize<List<EventModel>>(json, JsonOpts) ?? new();
            }
        }
    }
}
