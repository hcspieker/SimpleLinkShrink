var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();

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
