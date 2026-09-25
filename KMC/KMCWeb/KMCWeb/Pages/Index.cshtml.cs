using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using KMCWeb.Models;
using System.Text.Json;

namespace KMCWeb.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IHttpClientFactory httpClientFactory;
        private static readonly JsonSerializerOptions JsonOpts = new() { PropertyNameCaseInsensitive = true };

        public List<EventModel> UpcomingEvents { get; set; } = new();
        public string[] Categories => EventCategories.All;

        public IndexModel(IHttpClientFactory _httpClientFactory)
        {
            httpClientFactory = _httpClientFactory;
        }

        public async Task OnGet()
        {
            var client = httpClientFactory.CreateClient("KmcApi");
            var response = await client.GetAsync("api/Event");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var events = JsonSerializer.Deserialize<List<EventModel>>(json, JsonOpts) ?? new();
                UpcomingEvents = events
                    .Where(e => e.IsUpcoming)
                    .OrderBy(e => e.StartDate)
                    .Take(6)
                    .ToList();
            }
        }
    }
}
