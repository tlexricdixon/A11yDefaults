using A11yDefaults.Mvc;

var builder = WebApplication.CreateBuilder(args);
builder.WebHost.UseStaticWebAssets();

builder.Services.AddControllersWithViews();
builder.Services.AddA11yDefaults(options =>
{
    options.ButtonTargetSize = A11yTargetSize.Minimum;
    options.InputTargetSize = A11yTargetSize.Minimum;
    options.LinkHintVisibility = A11yLinkHintVisibility.ScreenReaderOnly;
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
