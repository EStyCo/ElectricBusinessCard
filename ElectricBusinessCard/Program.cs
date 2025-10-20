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
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultSQLConnection"));
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