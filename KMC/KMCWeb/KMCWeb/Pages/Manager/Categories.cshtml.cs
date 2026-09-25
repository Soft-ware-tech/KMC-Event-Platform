using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using KMCWeb.Models;
using System.Text;
using System.Text.Json;

namespace KMCWeb.Pages.Manager
{
    public class CategoriesModel : PageModel
    {
        private readonly IHttpClientFactory httpClientFactory;
        private static readonly JsonSerializerOptions JsonOpts = new() { PropertyNameCaseInsensitive = true };

        public List<CategoryModel> Categories { get; set; } = new();
        public string? ErrorMessage { get; set; }

        [BindProperty]
        public string NewCategoryName { get; set; } = "";

        public CategoriesModel(IHttpClientFactory _httpClientFactory)
        {
            httpClientFactory = _httpClientFactory;
        }

        public async Task<IActionResult> OnGet()
        {
            if (HttpContext.Session.GetInt32("ManagerId") == null)
                return RedirectToPage("/ManagerLogin");

            await LoadCategories();
            return Page();
        }

        public async Task<IActionResult> OnPostAdd()
        {
            var managerId = HttpContext.Session.GetInt32("ManagerId");
            if (managerId == null)
                return RedirectToPage("/ManagerLogin");

            var client = httpClientFactory.CreateClient("KmcApi");
            var content = new StringContent(JsonSerializer.Serialize(new { Name = NewCategoryName }), Encoding.UTF8, "application/json");
            var response = await client.PostAsync($"api/Category?managerId={managerId}", content);

            if (!response.IsSuccessStatusCode)
                ErrorMessage = await response.Content.ReadAsStringAsync();

            await LoadCategories();
            return Page();
        }

        public async Task<IActionResult> OnPostDelete(int id)
        {
            var managerId = HttpContext.Session.GetInt32("ManagerId");
            if (managerId == null)
                return RedirectToPage("/ManagerLogin");

            var client = httpClientFactory.CreateClient("KmcApi");
            await client.DeleteAsync($"api/Category/{id}?managerId={managerId}");

            return RedirectToPage();
        }

        private async Task LoadCategories()
        {
            var client = httpClientFactory.CreateClient("KmcApi");
            var response = await client.GetAsync("api/Category");
            if (response.IsSuccessStatusCode)
                Categories = JsonSerializer.Deserialize<List<CategoryModel>>(await response.Content.ReadAsStringAsync(), JsonOpts) ?? new();
        }
    }
}