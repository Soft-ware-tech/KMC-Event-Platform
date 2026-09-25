using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using KMC_API.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

//Read the connection string
var conn = builder.Configuration.GetConnectionString("conn");
builder.Services.AddDbContext<AppDBContext>(opt => opt.UseSqlServer(conn, sql => sql.EnableRetryOnFailure()));

//All the Repo classes must be listed down like below
builder.Services.AddScoped<OrganizerRepo>();
builder.Services.AddScoped<EventRepo>();
builder.Services.AddScoped<TicketClassRepo>();
builder.Services.AddScoped<RegistrationRepo>();
builder.Services.AddScoped<ManagerRepo>();
builder.Services.AddScoped<CategoryRepo>();
builder.Services.AddScoped<ActivityLogRepo>();
builder.Services.AddAutoMapper(cfg => { }, AppDomain.CurrentDomain.GetAssemblies());

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    //To use Scalar.
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
