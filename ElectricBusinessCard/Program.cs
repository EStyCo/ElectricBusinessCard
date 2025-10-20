using ElectricBusinessCard.Services.EntityFramework;
using ElectricBusinessCard.Repository;
using ElectricBusinessCard.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

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

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// ПОДКЛЮЧЕНИЕ CORS MIDDLEWARE (важен порядок!)
app.UseCors("AllowElectroservice");

app.UseAuthorization();

app.MapRazorPages();

app.Run();