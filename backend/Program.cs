using Microsoft.EntityFrameworkCore;
using System;

var builder = WebApplication.CreateBuilder(args);

// Adiciona serviços ao container
// Configura o DbContext com SQLite para persistência local
// O arquivo de banco será criado automaticamente em "expenses.db"
builder.Services.AddDbContext<ExpensesDbContext>(options =>
    options.UseSqlite("Data Source=expenses.db"));

// Adiciona suporte a controllers
builder.Services.AddControllers();

builder.Services.AddControllers()

.AddJsonOptions(options =>
{
    options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
    options.JsonSerializerOptions.Converters.Add(
        new System.Text.Json.Serialization.JsonStringEnumConverter());
});

// Configura CORS para permitir chamadas do front-end
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
    });
});

var app = builder.Build();

// Aplica migrações automaticamente ao iniciar
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ExpensesDbContext>();
    db.Database.Migrate();
}

// Configura o pipeline HTTP
app.UseHttpsRedirection();
app.UseAuthorization();
app.UseCors();
app.MapControllers();

app.Run();