using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using KMCWeb.Models;
using System.Text;
using System.Text.Json;

namespace KMCWeb.Pages.BO
{
    public class ManageTicketsModel : PageModel
    {
        private readonly IHttpClientFactory httpClientFactory;
        private static readonly JsonSerializerOptions JsonOpts = new() { PropertyNameCaseInsensitive = true };

        public string[] ClassNames => TicketClassNames.All;
        public EventModel? Ev { get; set; }
        public List<TicketClassModel> TicketClasses { get; set; } = new();
        public string? ErrorMessage { get; set; }

        [TempData]
        public string? SuccessMessage { get; set; }

        [BindProperty]
        public int EventId { get; set; }
        [BindProperty]
        public string ClassName { get; set; } = "1st Class";
        [BindProperty]
        public decimal Price { get; set; }
        [BindProperty]
        public int Capacity { get; set; } = 50;

        public ManageTicketsModel(IHttpClientFactory _httpClientFactory)
        {
            httpClientFactory = _httpClientFactory;
        }

        public async Task<IActionResult> OnGet(int eventId)
        {
            var organizerId = HttpContext.Session.GetInt32("OrganizerId");
            if (organizerId == null)
                return RedirectToPage("/Login");

            var loaded = await LoadEvent(eventId, organizerId.Value);
            if (loaded != null) return loaded;

            return Page();
        }

        public async Task<IActionResult> OnPostAdd()
        {
            var organizerId = HttpContext.Session.GetInt32("OrganizerId");
            if (organizerId == null)
                return RedirectToPage("/Login");

            var client = httpClientFactory.CreateClient("KmcApi");
            var payload = new TicketClassWriteModel
            {
                EventId = EventId,
                ClassName = ClassName,
                Price = Price,
                Capacity = Capacity,
                OrganizerId = organizerId.Value
            };
            var content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");
            var response = await client.PostAsync("api/TicketClass", content);

            if (response.IsSuccessStatusCode)
                SuccessMessage = $"{ClassName} added.";
            else
                ErrorMessage = await response.Content.ReadAsStringAsync();

            return RedirectToPage(new { eventId = EventId });
        }

        public async Task<IActionResult> OnPostDelete(int ticketClassId, int eventId)
        {
            var organizerId = HttpContext.Session.GetInt32("OrganizerId");
            if (organizerId == null)
                return RedirectToPage("/Login");

            var client = httpClientFactory.CreateClient("KmcApi");
            var response = await client.DeleteAsync($"api/TicketClass/{ticketClassId}?organizerId={organizerId}");

            SuccessMessage = response.IsSuccessStatusCode ? "Ticket class removed." : null;

            return RedirectToPage(new { eventId });
        }

        private async Task<IActionResult?> LoadEvent(int eventId, int organizerId)
        {
            var client = httpClientFactory.CreateClient("KmcApi");
            var response = await client.GetAsync($"api/Event/{eventId}");
            if (!response.IsSuccessStatusCode)
                return NotFound();

            var json = await response.Content.ReadAsStringAsync();
            var ev = JsonSerializer.Deserialize<EventDetailModel>(json, JsonOpts);
            if (ev == null)
                return NotFound();

            if (ev.OrganizerId != organizerId)
                return Forbid();

            Ev = ev;
            TicketClasses = ev.TicketClasses;
            EventId = ev.EventId;
            return null;
        }
    }
}
