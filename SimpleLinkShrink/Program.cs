using Microsoft.EntityFrameworkCore;
using SimpleLinkShrink.BackgroundServices;
using SimpleLinkShrink.Configuration;
using SimpleLinkShrink.Data;
using SimpleLinkShrink.Data.Entity;
using SimpleLinkShrink.Util;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("ShortlinkDbConnectionString");

if (connectionString == null)
    throw new InvalidOperationException("Connection string 'ShortlinkDbConnectionString' not found.");

builder.Services.AddDbContext<LinkDbContext>(o => o.UseSqlite(connectionString));

builder.Services.Configure<ShortLinkSettings>(builder.Configuration.GetRequiredSection(nameof(ShortLinkSettings)));

builder.Services.AddControllersWithViews();

builder.Services.AddSingleton<IRandomStringGenerator, RandomStringGenerator>();
builder.Services.AddScoped<IRepository, Repository>();

builder.Services.AddHostedService<InitDbService>();
builder.Services.AddHostedService<CleanupExpiredShortlinksService>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

// not necessary in production due to the use of nginx as a reverse proxy
if (app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

app.UseRouting();

app.MapStaticAssets();
app.MapDefaultControllerRoute().WithStaticAssets();

app.MapDefaultControllerRoute();

app.Run();
