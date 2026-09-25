using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using KMCWeb.Models;
using System.Text.Json;

namespace KMCWeb.Pages.Manager
{
    public class DashboardModel : PageModel
    {
        private readonly IHttpClientFactory httpClientFactory;
        private static readonly JsonSerializerOptions JsonOpts = new() { PropertyNameCaseInsensitive = true };

        public string? ManagerName { get; set; }
        public DashboardStatsModel Stats { get; set; } = new();
        public List<OrganizerReadModel> Organizers { get; set; } = new();
        public List<ActivityLogModel> RecentActivity { get; set; } = new();

        [TempData]
        public string? SuccessMessage { get; set; }

        public DashboardModel(IHttpClientFactory _httpClientFactory)
        {
            httpClientFactory = _httpClientFactory;
        }

        public async Task<IActionResult> OnGet()
        {
            var managerId = HttpContext.Session.GetInt32("ManagerId");
            if (managerId == null)
                return RedirectToPage("/ManagerLogin");

            ManagerName = HttpContext.Session.GetString("ManagerName");
            var client = httpClientFactory.CreateClient("KmcApi");

            var statsResp = await client.GetAsync($"api/Manager/dashboard-stats?managerId={managerId}");
            if (statsResp.IsSuccessStatusCode)
                Stats = JsonSerializer.Deserialize<DashboardStatsModel>(await statsResp.Content.ReadAsStringAsync(), JsonOpts) ?? new();

            var orgResp = await client.GetAsync("api/Organizer");
            if (orgResp.IsSuccessStatusCode)
                Organizers = JsonSerializer.Deserialize<List<OrganizerReadModel>>(await orgResp.Content.ReadAsStringAsync(), JsonOpts) ?? new();
            var logResp = await client.GetAsync($"api/Manager/activity-log?managerId={managerId}");
            if (logResp.IsSuccessStatusCode)
                RecentActivity = JsonSerializer.Deserialize<List<ActivityLogModel>>(await logResp.Content.ReadAsStringAsync(), JsonOpts) ?? new();
            return Page();
        }

        public async Task<IActionResult> OnPostDeleteOrganizer(int id)
        {
            var managerId = HttpContext.Session.GetInt32("ManagerId");
            if (managerId == null)
                return RedirectToPage("/ManagerLogin");

            var client = httpClientFactory.CreateClient("KmcApi");
            var response = await client.DeleteAsync($"api/Organizer/{id}?managerId={managerId}");
            SuccessMessage = response.IsSuccessStatusCode ? "Organizer removed." : "Could not remove that organizer.";
            return RedirectToPage();

        }
        public async Task<IActionResult> OnPostToggleActive(int id)
        {
            var managerId = HttpContext.Session.GetInt32("ManagerId");
            if (managerId == null)
                return RedirectToPage("/ManagerLogin");

            var client = httpClientFactory.CreateClient("KmcApi");
            var response = await client.PutAsync($"api/Organizer/{id}/toggle-active", null);
            SuccessMessage = response.IsSuccessStatusCode ? "Organizer status updated." : "Could not update status.";
            return RedirectToPage();
        }
    }
}