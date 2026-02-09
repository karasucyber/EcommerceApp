using Ecommerce.Application.Interfaces;
using Ecommerce.Application.Services;
using Ecommerce.Domain.Interfaces;
using Ecommerce.Domain.Entities; // Adicionado para criar o usuário
using Ecommerce.Domain.Enum;     // Adicionado para o Perfil
using Ecommerce.Infra.Auth;
using Ecommerce.Infra.Context;
using Ecommerce.Infra.Repositories;
using Ecommerce.Infra.Data;      
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("GpsPolicy", policy =>
    {
        policy.WithOrigins("http://localhost:4200")
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection") ?? "Data Source=app.db"));

builder.Services.AddScoped<IPedidoRepository, PedidoRepository>();
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IClienteRepository, ClienteRepository>();
builder.Services.AddScoped<IProdutoRepository, ProdutoRepository>();
builder.Services.AddScoped<ICarteiraRepository, CarteiraRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<AppDbContext>();
    var hasher = services.GetRequiredService<IPasswordHasher>();

    context.Database.EnsureCreated();

 
    if (!context.Usuarios.Any(u => u.Email == "admin@gps.com"))
    {
        var admin = new Usuario(
            "Administrador GPS",
            "admin@gps.com",
            hasher.Hash("admin123"),
            EPerfilUsuario.Administrador
        );
        context.Usuarios.Add(admin);
        context.SaveChanges();
    }


    if (!context.Produtos.Any())
    {
        DbSeeder.Seed(context);
    }
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("GpsPolicy");
app.UseAuthorization();
app.MapControllers();

app.Run();