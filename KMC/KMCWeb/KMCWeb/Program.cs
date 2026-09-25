var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

// Session is used to remember which organizer is currently logged in
// (OrganizerId / OrganizerName) so the Dashboard / Manage Event pages know
// who is allowed to edit what.
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromHours(2);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// Named HttpClient used by every page to call the KMC_API project.
// The base address is read from appsettings.json -> ApiSettings:BaseUrl.
// NOTE: if Visual Studio assigns KMC_API a different HTTPS port than 7101,
// update ApiSettings:BaseUrl in appsettings.json to match.
builder.Services.AddHttpClient("KmcApi", (sp, client) =>
{
    var baseUrl = builder.Configuration["ApiSettings:BaseUrl"] ?? "https://localhost:7101/";
    client.BaseAddress = new Uri(baseUrl);
})
.ConfigurePrimaryHttpMessageHandler(() =>
{
    var handler = new HttpClientHandler();
    if (builder.Environment.IsDevelopment())
    {
        // Development-only: accept the localhost self-signed dev certificate used
        // by the KMC_API project so the two local projects can talk to each other.
        handler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
    }
    return handler;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseSession();

app.UseAuthorization();

app.MapStaticAssets();
app.MapRazorPages()
   .WithStaticAssets();

app.Run();
