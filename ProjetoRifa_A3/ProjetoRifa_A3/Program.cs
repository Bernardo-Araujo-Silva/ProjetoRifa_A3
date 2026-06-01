using Microsoft.EntityFrameworkCore;
using ProjetoRifa.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "ProjetoRifa API", Version = "v1" });
});

builder.Services.AddDbContext<AppDbContext>(opt =>
    opt.UseSqlite("Data Source=rifa.db"));

var app = builder.Build();

// Cria o banco e as tabelas automaticamente
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.EnsureCreated();
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "ProjetoRifa API v1"));
}

app.MapControllers();

app.Run();
