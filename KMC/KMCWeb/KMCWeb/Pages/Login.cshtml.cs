using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using KMCWeb.Models;
using System.Text;
using System.Text.Json;

namespace KMCWeb.Pages
{
    public class LoginModel : PageModel
    {
        private readonly IHttpClientFactory httpClientFactory;
        private static readonly JsonSerializerOptions JsonOpts = new() { PropertyNameCaseInsensitive = true };

        public string? ErrorMessage { get; set; }

        [BindProperty]
        public string Username { get; set; } = "";
        [BindProperty]
        public string Password { get; set; } = "";

        public LoginModel(IHttpClientFactory _httpClientFactory)
        {
            httpClientFactory = _httpClientFactory;
        }

        public void OnGet() { }

        public async Task<IActionResult> OnPost()
        {
            var client = httpClientFactory.CreateClient("KmcApi");
            var payload = new { Username, Password };
            var content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");
            var response = await client.PostAsync("api/Organizer/login", content);

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var organizer = JsonSerializer.Deserialize<OrganizerReadModel>(json, JsonOpts);
                if (organizer != null)
                {
                    HttpContext.Session.SetInt32("OrganizerId", organizer.OrganizerId);
                    HttpContext.Session.SetString("OrganizerName", organizer.FullName);
                    return RedirectToPage("/BO/Dashboard");
                }
            }

            var apiMessage = await response.Content.ReadAsStringAsync();
            ErrorMessage = !string.IsNullOrWhiteSpace(apiMessage) ? apiMessage.Trim('"') : "Invalid username or password.";
            return Page();
        }
    }
}
