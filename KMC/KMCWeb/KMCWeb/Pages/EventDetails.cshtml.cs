using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using KMCWeb.Models;
using System.Text;
using System.Text.Json;

namespace KMCWeb.Pages
{
    public class EventDetailsModel : PageModel
    {
        private readonly IHttpClientFactory httpClientFactory;
        private static readonly JsonSerializerOptions JsonOpts = new() { PropertyNameCaseInsensitive = true };

        public EventDetailModel? Ev { get; set; }
        public string? ErrorMessage { get; set; }

        [BindProperty]
        public string ParticipantName { get; set; } = "";
        [BindProperty]
        public string Email { get; set; } = "";
        [BindProperty]
        public string Phone { get; set; } = "";
        [BindProperty]
        public int Quantity { get; set; } = 1;
        [BindProperty]
        public int? TicketClassId { get; set; }
        [BindProperty]
        public string? CardHolderName { get; set; }
        [BindProperty]
        public string? CardNumber { get; set; }
        [BindProperty]
        public string? ExpiryMonth { get; set; }
        [BindProperty]
        public string? ExpiryYear { get; set; }
        [BindProperty]
        public string? Cvv { get; set; }

        public EventDetailsModel(IHttpClientFactory _httpClientFactory)
        {
            httpClientFactory = _httpClientFactory;
        }

        public async Task<IActionResult> OnGet(int id)
        {
            var loaded = await LoadEvent(id);
            if (!loaded) return NotFound();
            return Page();
        }

        public async Task<IActionResult> OnPost(int id)
        {
            var loaded = await LoadEvent(id);
            if (!loaded) return NotFound();

            var client = httpClientFactory.CreateClient("KmcApi");

            var payload = new RegistrationWriteModel
            {
                EventId = id,
                ParticipantName = ParticipantName,
                Email = Email,
                Phone = Phone,
                Quantity = Quantity < 1 ? 1 : Quantity,
                TicketClassId = Ev!.HasTickets ? TicketClassId : null,
                CardHolderName = CardHolderName,
                CardNumber = CardNumber,
                ExpiryMonth = ExpiryMonth,
                ExpiryYear = ExpiryYear,
                Cvv = Cvv
            };

            var content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");
            var response = await client.PostAsync("api/Registration", content);

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<RegistrationReadModel>(json, JsonOpts);
                if (result != null)
                {
                    return RedirectToPage("/Confirmation", new { id = result.RegistrationId });
                }
            }

            ErrorMessage = await response.Content.ReadAsStringAsync();
            if (string.IsNullOrWhiteSpace(ErrorMessage) || ErrorMessage.TrimStart().StartsWith("{"))
            {
                ErrorMessage = "We couldn't complete your booking. Please check the details and try again.";
            }

            return Page();
        }

        private async Task<bool> LoadEvent(int id)
        {
            var client = httpClientFactory.CreateClient("KmcApi");
            var response = await client.GetAsync($"api/Event/{id}");
            if (!response.IsSuccessStatusCode) return false;

            var json = await response.Content.ReadAsStringAsync();
            Ev = JsonSerializer.Deserialize<EventDetailModel>(json, JsonOpts);
            return Ev != null;
        }
    }
}
