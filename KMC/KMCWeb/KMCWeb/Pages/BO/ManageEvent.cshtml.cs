using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using KMCWeb.Models;
using System.Text;
using System.Text.Json;

namespace KMCWeb.Pages.BO
{
    public class ManageEventModel : PageModel
    {
        private readonly IHttpClientFactory httpClientFactory;
        private static readonly JsonSerializerOptions JsonOpts = new() { PropertyNameCaseInsensitive = true };

        public string[] Categories => EventCategories.All;
        public string? ErrorMessage { get; set; }
        public bool IsEdit => EventId.HasValue;

        [BindProperty]
        public int? EventId { get; set; }
        [BindProperty]
        public string Title { get; set; } = "";
        [BindProperty]
        public string? Description { get; set; }
        [BindProperty]
        public string Category { get; set; } = "Community";
        [BindProperty]
        public string Location { get; set; } = "";
        [BindProperty]
        public DateTime StartDate { get; set; } = DateTime.Now.Date.AddDays(7);
        [BindProperty]
        public DateTime EndDate { get; set; } = DateTime.Now.Date.AddDays(7);
        [BindProperty]
        public string? ImageUrl { get; set; }
        [BindProperty]
        public int RegistrationLimit { get; set; }
        [BindProperty]
        public bool HasTickets { get; set; }

        public ManageEventModel(IHttpClientFactory _httpClientFactory)
        {
            httpClientFactory = _httpClientFactory;
        }

        public async Task<IActionResult> OnGet(int? id)
        {
            var organizerId = HttpContext.Session.GetInt32("OrganizerId");
            if (organizerId == null)
                return RedirectToPage("/Login");

            if (id != null)
            {
                var client = httpClientFactory.CreateClient("KmcApi");
                var response = await client.GetAsync($"api/Event/{id}");
                if (!response.IsSuccessStatusCode)
                    return NotFound();

                var json = await response.Content.ReadAsStringAsync();
                var ev = JsonSerializer.Deserialize<EventDetailModel>(json, JsonOpts);
                if (ev == null)
                    return NotFound();

                // Enforce ownership on the client side too (the API enforces it again on save).
                if (ev.OrganizerId != organizerId)
                {
                    ErrorMessage = "You are not allowed to edit this event.";
                    return NotFound();
                }

                EventId = ev.EventId;
                Title = ev.Title;
                Description = ev.Description;
                Category = ev.Category;
                Location = ev.Location;
                StartDate = ev.StartDate;
                EndDate = ev.EndDate;
                ImageUrl = ev.ImageUrl;
                RegistrationLimit = ev.RegistrationLimit;
                HasTickets = ev.HasTickets;
            }

            return Page();
        }

        public async Task<IActionResult> OnPost()
        {
            var organizerId = HttpContext.Session.GetInt32("OrganizerId");
            if (organizerId == null)
                return RedirectToPage("/Login");

            if (EndDate < StartDate)
            {
                ErrorMessage = "End date cannot be before the start date.";
                return Page();
            }

            var payload = new EventWriteModel
            {
                Title = Title,
                Description = Description,
                Category = Category,
                Location = Location,
                StartDate = StartDate,
                EndDate = EndDate,
                ImageUrl = ImageUrl,
                RegistrationLimit = RegistrationLimit,
                HasTickets = HasTickets,
                OrganizerId = organizerId.Value
            };

            var client = httpClientFactory.CreateClient("KmcApi");
            var content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");

            HttpResponseMessage response;
            if (EventId.HasValue)
            {
                response = await client.PutAsync($"api/Event/{EventId.Value}", content);
            }
            else
            {
                response = await client.PostAsync("api/Event", content);
            }

            if (response.IsSuccessStatusCode)
            {
                return RedirectToPage("/BO/Dashboard");
            }

            ErrorMessage = response.StatusCode == System.Net.HttpStatusCode.Forbidden
                ? "You are not allowed to edit this event."
                : "We couldn't save this event. Please check the details and try again.";
            return Page();
        }
    }
}
