using Azure.Storage.Blobs;
using CallApi.Helpers;
using Microsoft.AspNetCore.Authentication.Cookies;
using MvcCubosExamenSAM.Services;

var builder = WebApplication.CreateBuilder(args);
string connectionString = builder.Configuration.GetValue<string>("AzureKeys:StorageAccount");
builder.Services.AddTransient<BlobServiceClient>(x => new BlobServiceClient(connectionString));

// Add services to the container.
builder.Services.AddTransient<HelperCallApi>();
builder.Services.AddTransient<ServiceCubos>();
builder.Services.AddTransient<ServiceUsuarios>();
builder.Services.AddTransient<ServiceBlobs>();
builder.Services.AddControllersWithViews();

builder.Services.AddAuthentication(options =>
{
    options.DefaultSignInScheme =
    CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultAuthenticateScheme =
    CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme =
    CookieAuthenticationDefaults.AuthenticationScheme;
}).AddCookie(
    CookieAuthenticationDefaults.AuthenticationScheme,
    config => config.AccessDeniedPath = "/Managed/ErrorAcceso"
);
builder.Services.AddSession();

builder.Services.AddControllersWithViews(options => options.EnableEndpointRouting = false).AddSessionStateTempDataProvider();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseSession();
app.UseMvc(routes =>
{
    routes.MapRoute(
        name: "default",
        template: "{controller=Cubos}/{action=Cubos}/{id?}");
});

app.Run();
