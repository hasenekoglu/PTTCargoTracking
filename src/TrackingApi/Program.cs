using Hangfire;
using Microsoft.EntityFrameworkCore;
using TrackingApi.BackgroundJobs.Jobs;
using TrackingApi.BackgroundJobs.Services;
using TrackingApi.Data;
using TrackingApi.Services;
using TrackingApi.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHangfire(configuration => configuration
    .UseSqlServerStorage(builder.Configuration.GetConnectionString("HangfireDB")));
builder.Services.AddHangfireServer();
// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("TrackingDB");
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(connectionString);
});

builder.Services.AddScoped<ITrackingBackgroundJob, TrackingBackgroundJob>();
builder.Services.AddScoped<ITrackingService, TrackingService>();
builder.Services.AddHttpClient<IPttScraper, PttScraper>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();



// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHangfireDashboard();
// Configure recurring job - every 10 seconds
RecurringJob.AddOrUpdate<ITrackingBackgroundJob>(
    "update-tracking-statuses",
    job => job.UpdateAllTrackingStatuses(),
    "*/10 * * * * *"); // Cron expression for every 10 seconds
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    var context = services.GetRequiredService<AppDbContext>();
    if (context.Database.GetPendingMigrations().Any())
    {
        context.Database.Migrate();
    }
}


app.Run();
