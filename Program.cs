using PgJobs.Data;
using PgJobs.Utils;
using Serilog;

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

Log.Logger = new LoggerConfiguration()
               .MinimumLevel.Debug()
               .WriteTo.Console()
               .WriteTo.File("logs/pg_jobs.log", rollingInterval: RollingInterval.Day)
               .CreateLogger();

if (args.Length == 1)
    JobDbContext.DbConn = args[0];


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSingleton<JobDbContext>();
builder.Services.AddHostedService<JobWork>();
builder.Services.AddDbContext<JobDbContext>();
builder.Services.AddRazorPages();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}
app.UseStaticFiles();



app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
