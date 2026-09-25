using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using KMCWeb.Models;
using System.Text.Json;

namespace KMCWeb.Pages
{
    public class ConfirmationModel : PageModel
    {
        private readonly IHttpClientFactory httpClientFactory;
        private static readonly JsonSerializerOptions JsonOpts = new() { PropertyNameCaseInsensitive = true };

        public RegistrationReadModel? Booking { get; set; }

        public ConfirmationModel(IHttpClientFactory _httpClientFactory)
        {
            httpClientFactory = _httpClientFactory;
        }

        public async Task<IActionResult> OnGet(int id)
        {
            var client = httpClientFactory.CreateClient("KmcApi");
            var response = await client.GetAsync($"api/Registration/{id}");
            if (!response.IsSuccessStatusCode) return NotFound();

            var json = await response.Content.ReadAsStringAsync();
            Booking = JsonSerializer.Deserialize<RegistrationReadModel>(json, JsonOpts);
            if (Booking == null) return NotFound();

            return Page();
        }
    }
}
