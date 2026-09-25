using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using KMCWeb.Models;
using System.Text.Json;

namespace KMCWeb.Pages.BO
{
    public class DashboardModel : PageModel
    {
        private readonly IHttpClientFactory httpClientFactory;
        private static readonly JsonSerializerOptions JsonOpts = new() { PropertyNameCaseInsensitive = true };

        public List<EventModel> MyEvents { get; set; } = new();
        public string? OrganizerName { get; set; }

        [TempData]
        public string? SuccessMessage { get; set; }
        [TempData]
        public string? FailMessage { get; set; }
        public List<ActivityLogModel> RecentActivity { get; set; } = new();

        public DashboardModel(IHttpClientFactory _httpClientFactory)
        {
            httpClientFactory = _httpClientFactory;
        }

        public async Task<IActionResult> OnGet()
        {
            var organizerId = HttpContext.Session.GetInt32("OrganizerId");
            if (organizerId == null)
                return RedirectToPage("/Login");

            OrganizerName = HttpContext.Session.GetString("OrganizerName");

            var client = httpClientFactory.CreateClient("KmcApi");
            var response = await client.GetAsync($"api/Event/organizer/{organizerId}");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                MyEvents = JsonSerializer.Deserialize<List<EventModel>>(json, JsonOpts) ?? new();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostDelete(int id)
        {
            var organizerId = HttpContext.Session.GetInt32("OrganizerId");
            if (organizerId == null)
                return RedirectToPage("/Login");

            var client = httpClientFactory.CreateClient("KmcApi");
            var response = await client.DeleteAsync($"api/Event/{id}?organizerId={organizerId}");

            SuccessMessage = response.IsSuccessStatusCode ? "Event deleted." : null;
            FailMessage = response.IsSuccessStatusCode ? null : "Could not delete that event.";

            return RedirectToPage();
        }
    }
}
