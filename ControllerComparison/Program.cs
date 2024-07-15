using Microsoft.AspNetCore.Mvc.Razor;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// RazorViewEngineOptionsを設定
builder.Services.Configure<RazorViewEngineOptions>(options =>
{
    // 既存のビュー場所をクリア
    options.ViewLocationFormats.Clear();

    // カスタムビュー場所を追加
    options.ViewLocationFormats.Add("/Views_Core/{1}/{0}.cshtml");
    options.ViewLocationFormats.Add("/Views_Core/Shared/{0}.cshtml");
});


// セッションの設定を追加
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // セッションのタイムアウト時間を設定
    options.Cookie.HttpOnly = true; // クッキーを HttpOnly に設定
    options.Cookie.IsEssential = true; // GDPR 対応のために必須クッキーとしてマーク
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

// セッションを使用するようにミドルウェアを追加
app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();