using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using KMCWeb.Models;
using System.Text.Json;

namespace KMCWeb.Pages.BO
{
    public class AttendeesModel : PageModel
    {
        private readonly IHttpClientFactory httpClientFactory;
        private static readonly JsonSerializerOptions JsonOpts = new() { PropertyNameCaseInsensitive = true };

        public EventModel? Ev { get; set; }
        public List<RegistrationReadModel> Registrations { get; set; } = new();

        public AttendeesModel(IHttpClientFactory _httpClientFactory)
        {
            httpClientFactory = _httpClientFactory;
        }

        public async Task<IActionResult> OnGet(int eventId)
        {
            var organizerId = HttpContext.Session.GetInt32("OrganizerId");
            if (organizerId == null)
                return RedirectToPage("/Login");

            var client = httpClientFactory.CreateClient("KmcApi");

            var evResponse = await client.GetAsync($"api/Event/{eventId}");
            if (!evResponse.IsSuccessStatusCode)
                return NotFound();

            var evJson = await evResponse.Content.ReadAsStringAsync();
            Ev = JsonSerializer.Deserialize<EventModel>(evJson, JsonOpts);
            if (Ev == null)
                return NotFound();

            if (Ev.OrganizerId != organizerId)
                return Forbid();

            var regResponse = await client.GetAsync($"api/Registration/event/{eventId}");
            if (regResponse.IsSuccessStatusCode)
            {
                var regJson = await regResponse.Content.ReadAsStringAsync();
                Registrations = JsonSerializer.Deserialize<List<RegistrationReadModel>>(regJson, JsonOpts) ?? new();
            }

            return Page();
        }
    }
}
