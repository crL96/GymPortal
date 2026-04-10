using Application.Extensions;
using Infrastructure.Data;
using Infrastructure.Extensions;
using Presentation.WebApp.Placeholders;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplication(builder.Configuration, builder.Environment);
builder.Services.AddInfrastructure(builder.Configuration, builder.Environment);

builder.Services.AddScoped<IFaqService, FaqService>();

builder.Services.AddSession();
builder.Services.AddControllersWithViews();
builder.Services.AddRouting(x => x.LowercaseUrls = true);

var app = builder.Build();

app.UseHsts();
app.UseHttpsRedirection();
app.UseRouting();

app.UseSession();
app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

await InfrastructureInitializer.InitializeAsync(app.Services, app.Configuration, app.Environment);

app.Run();
