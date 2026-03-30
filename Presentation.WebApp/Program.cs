using Presentation.WebApp.Placeholders;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IFaqService, FaqService>();

builder.Services.AddControllersWithViews();

var app = builder.Build();

app.UseHsts();
app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
