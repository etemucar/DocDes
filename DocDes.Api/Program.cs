using Microsoft.EntityFrameworkCore;
using MediatR;
using DocDes.Data;
using DocDes.Service.Behaviors;
using DocDes.Core.Base; // AppSettings ve diğer base modeller için
using System.Text.Json;
using System.Text.Json.Serialization;
using Scalar.AspNetCore;
using DocDes.Settings.Core;

var builder = WebApplication.CreateBuilder(args);

// 1. Veritabanı Yapılandırması (PostgreSQL)
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
                        ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services.AddDbContext<DocDesDbContext>(options =>
    options.UseNpgsql(connectionString, npgsqlOptions =>
        npgsqlOptions
            .MigrationsAssembly("DocDes.Data")
            .MigrationsHistoryTable("__EFMigrationsHistory"))
           .UseSnakeCaseNamingConvention());

// 2. MediatR & Pipeline Behaviors
builder.Services.AddMediatR(cfg =>
{
    // Mevcut assembly'deki Handler'ları kaydet
    cfg.RegisterServicesFromAssembly(typeof(Program).Assembly);
    
    // Service katmanındaki Handler'ları ve TransactionalBehavior'ı kaydet
    cfg.RegisterServicesFromAssembly(typeof(DocDes.Service.Behaviors.TransactionalBehavior<,>).Assembly);
    
    // Pipeline'a TransactionalBehavior ekle (CQRS yönetimi için)
    cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(TransactionalBehavior<,>));
});

// 3. AutoMapper Yapılandırması
builder.Services.AddAutoMapper(cfg => 
{
    cfg.AddMaps(new[] { 
        typeof(DocDes.Service.Mapper.MappingProfile).Assembly,
        typeof(Program).Assembly
    });
});

// 4. Ayarlar ve Uygulama Servisleri
// appsettings.json içindeki AppSettings bölümünü bind eder
builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));

// Extension method ile gelen diğer servis kayıtları
// builder.Services.AddApplicationServices(); 

// 5. OpenAPI & Scalar (Modern Dokümantasyon)
builder.Services.AddOpenApi(); 

// 6. CORS Politikası
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// 7. Controller ve JSON Optimizasyonları
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

// 8. Altyapı Servisleri
builder.Services.AddHttpContextAccessor();
builder.Services.AddMemoryCache();
builder.Services.AddAuthentication();
builder.Services.AddAuthorization();

try 
{
    var app = builder.Build();

    // 9. Middleware Pipeline (Sıralama Önemlidir)
    if (app.Environment.IsDevelopment()) 
    {
        app.UseDeveloperExceptionPage();
        
        // OpenAPI ve Scalar UI'ı aktif et
        app.MapOpenApi();
        app.MapScalarApiReference(options => {
            options.WithTitle("DocDes API Reference")
                   .WithTheme(ScalarTheme.Moon)
                   .WithDefaultHttpClient(ScalarTarget.CSharp, ScalarClient.HttpClient);
        });
    }
    else
    {
        app.UseHsts();
    }

    app.UseHttpsRedirection();
    app.UseStaticFiles();
    app.UseRouting();
    
    app.UseCors("AllowAll");

    app.UseAuthentication();
    app.UseAuthorization();

    app.MapControllers();

    // WeatherForecast Minimal API örneği (Test amaçlı kalabilir)
    app.MapGet("/weatherforecast", () =>
    {
        var summaries = new[] { "Freezing", "Bracing", "Chilly", "Cool", "Mild" };
        var forecast = Enumerable.Range(1, 5).Select(index =>
            new WeatherForecast
            (
                DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                Random.Shared.Next(-20, 55),
                summaries[Random.Shared.Next(summaries.Length)]
            )).ToArray();
        return forecast;
    }).WithName("GetWeatherForecast");

    app.Run();
}
catch (System.Reflection.ReflectionTypeLoadException ex)
{
    // Derleme veya çalışma anı yükleme hatalarını yakalamak için
    foreach (var le in ex.LoaderExceptions)
    {
        Console.WriteLine($"[CRITICAL ERROR]: {le?.Message}");
    }
    throw;
}

// Minimal API Record
record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}