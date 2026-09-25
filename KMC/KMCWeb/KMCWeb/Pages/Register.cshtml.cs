using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using KMCWeb.Models;
using System.Text;
using System.Text.Json;

namespace KMCWeb.Pages
{
    public class RegisterModel : PageModel
    {
        private readonly IHttpClientFactory httpClientFactory;
        private static readonly JsonSerializerOptions JsonOpts = new() { PropertyNameCaseInsensitive = true };

        public string? ErrorMessage { get; set; }

        [BindProperty]
        public string FullName { get; set; } = "";
        [BindProperty]
        public string? OrganizationName { get; set; }
        [BindProperty]
        public string Email { get; set; } = "";
        [BindProperty]
        public string Phone { get; set; } = "";
        [BindProperty]
        public string Username { get; set; } = "";
        [BindProperty]
        public string Password { get; set; } = "";
        [BindProperty]
        public string ConfirmPassword { get; set; } = "";

        public RegisterModel(IHttpClientFactory _httpClientFactory)
        {
            httpClientFactory = _httpClientFactory;
        }

        public void OnGet() { }

        public async Task<IActionResult> OnPost()
        {
            if (Password != ConfirmPassword)
            {
                ErrorMessage = "Passwords do not match.";
                return Page();
            }

            var payload = new
            {
                FullName,
                OrganizationName,
                Email,
                Phone,
                Username,
                Password
            };

            var client = httpClientFactory.CreateClient("KmcApi");
            var content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");
            var response = await client.PostAsync("api/Organizer/register", content);

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

            ErrorMessage = response.StatusCode == System.Net.HttpStatusCode.Conflict
                ? await response.Content.ReadAsStringAsync()
                : "We couldn't create your account. Please check your details and try again.";
            return Page();
        }
    }
}
