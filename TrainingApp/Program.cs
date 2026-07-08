using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using System.Text.Json.Serialization;
using TrainingApp.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
        options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Автоматическое применение миграций при запуске. (создание таблиц, если их нет)
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    dbContext.Database.Migrate();
}

// Включение Swagger и Swagger UI c данными для каждого пользователя Swagger отдельно
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "TrainingApp API v1");
    c.RoutePrefix = "swagger";

    c.HeadContent = @"
    <script>
        (function() {
            function getUserId() {
                return localStorage.getItem('userId');
            }

            const originalFetch = window.fetch;
            window.fetch = function(url, options) {
                options = options || {};
                options.headers = options.headers || {};
                const userId = getUserId();
                if (userId) {
                    options.headers['X-UserId'] = userId;
                } else {
                    console.warn('UserId не найден в localStorage. Сначала откройте интерфейс, чтобы он создал ID.');
                }
                return originalFetch.call(this, url, options);
            };

            const originalXHROpen = XMLHttpRequest.prototype.open;
            XMLHttpRequest.prototype.open = function(method, url, async, user, password) {
                this._url = url;
                return originalXHROpen.apply(this, arguments);
            };
            const originalXHRSend = XMLHttpRequest.prototype.send;
            XMLHttpRequest.prototype.send = function(body) {
                const userId = getUserId();
                if (userId) {
                    this.setRequestHeader('X-UserId', userId);
                } else {
                    console.warn('UserId не найден в localStorage. Сначала откройте интерфейс.');
                }
                return originalXHRSend.call(this, body);
            };
        })();
    </script>
    ";
});


app.UseHttpsRedirection();

app.UseDefaultFiles();
app.UseStaticFiles();

app.UseAuthorization();

app.MapControllers();

app.Run();