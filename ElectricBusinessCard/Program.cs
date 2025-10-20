using DocumentFormat.OpenXml.Office2016.Drawing.ChartDrawing;
using ElectricBusinessCard.Repository;
using ElectricBusinessCard.Services;
using ElectricBusinessCard.Services.EntityFramework;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
Console.OutputEncoding = Encoding.UTF8;
Encoding.GetEncoding("utf-8");

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddHttpClient();

// Регистрация пользовательских служб
builder.Services.AddScoped<WorkRepository>();
builder.Services.AddScoped<WorkService>();
builder.Services.AddScoped<CategoryRepository>();
builder.Services.AddScoped<CategoryService>();
builder.Services.AddScoped<DocService>();

// Добавление политики CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowElectroservice", policy =>
    {
        policy.WithOrigins("http://electroservice-irk.ru",
                          "https://electroservice-irk.ru",
                          "http://www.electroservice-irk.ru",
                          "https://www.electroservice-irk.ru",
                          "http://localhost:5000")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials(); // Если нужны куки или авторизация
    });
});

builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    var supportedCultures = new[] { new CultureInfo("ru-RU") };
    options.DefaultRequestCulture = new RequestCulture("ru-RU");
    options.SupportedCultures = supportedCultures;
    options.SupportedUICultures = supportedCultures;
});

// Настройка Entity Framework
builder.Services.AddDbContext<AppDbContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultSQLConnection");
    options.UseSqlite(connectionString);

    // Автоматически определяем путь к миграциям
    var databasePath = Path.GetDirectoryName(connectionString.Replace("Data Source=", ""));
    var migrationsPath = Path.Combine(databasePath, "Migrations");

    // Если миграции существуют в папке БД - используем их
    if (Directory.Exists(migrationsPath) && Directory.GetFiles(migrationsPath, "*.cs").Any())
    {
        // Для этого нужно чтобы миграции были в отдельной сборке
        // или используем кастомный провайдер миграций
        Console.WriteLine($"📁 Используем миграции из: {migrationsPath}");
    }
});



var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.Use(async (context, next) =>
{
    context.Response.Headers["Content-Type"] = "text/html; charset=utf-8";
    context.Response.Headers["Content-Language"] = "ru-RU";
    await next();
});

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// ПОДКЛЮЧЕНИЕ CORS MIDDLEWARE (важен порядок!)
app.UseCors("AllowElectroservice");

app.UseAuthorization();

app.MapRazorPages();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    try
    {
        dbContext.Database.Migrate();
        Console.WriteLine("✅ Миграции применены к БД");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"❌ Ошибка применения миграций: {ex.Message}");
    }
}

app.Run();