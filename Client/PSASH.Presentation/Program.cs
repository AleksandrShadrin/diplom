using ElectronNET.API;
using ElectronNET.API.Entities;
using PSASH.Infrastructure;
using PSASH.Presentation.Services;
using Radzen;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddElectron();
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.RegisterInfrastructure();
builder.Services.AddScoped<TooltipService>();
builder.Services.AddScoped<DialogService>();
builder.Services.AddScoped<NotificationService>();
builder.Services.AddScoped<TooltipService>();
builder.Services.AddScoped<ContextMenuService>();
builder.Services.AddScoped<ITimeSeriesInfoService, TimeSeriesInfoService>();
builder.Services.AddSingleton<ITimeSeriesTransformer, TimeSeriesTransformer>();
builder.Services.AddSingleton<AppStateService>();
builder.Services.AddTransient<ServerStateCheckerService>();

builder.WebHost.UseElectron(args);

if (HybridSupport.IsElectronActive)
{
    // Open the Electron-Window
    Task.Run(async () => {
       
        var window = await Electron.WindowManager.CreateWindowAsync(new BrowserWindowOptions 
        { 
            
            Width = 1280,
            Height = 720,
        });

        window.OnClosed += () => {
            Electron.App.Quit();
        };
    });
}

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();