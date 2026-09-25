using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using KMCWeb.Models;
using System.Text.Json;

namespace KMCWeb.Pages.Manager
{
    public class AllBookingsModel : PageModel
    {
        private readonly IHttpClientFactory httpClientFactory;
        private static readonly JsonSerializerOptions JsonOpts = new() { PropertyNameCaseInsensitive = true };
        public List<RegistrationReadModel> Bookings { get; set; } = new();

        public AllBookingsModel(IHttpClientFactory _httpClientFactory)
        {
            httpClientFactory = _httpClientFactory;
        }

        public async Task<IActionResult> OnGet()
        {
            if (HttpContext.Session.GetInt32("ManagerId") == null)
                return RedirectToPage("/ManagerLogin");

            var client = httpClientFactory.CreateClient("KmcApi");
            var response = await client.GetAsync("api/Registration");
            if (response.IsSuccessStatusCode)
                Bookings = JsonSerializer.Deserialize<List<RegistrationReadModel>>(await response.Content.ReadAsStringAsync(), JsonOpts) ?? new();

            return Page();
        }
    }
}