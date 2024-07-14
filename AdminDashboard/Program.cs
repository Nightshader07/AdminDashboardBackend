using System.Configuration;
using AdminDashboard.Data;
using AdminDashboard.Interfaces;
using AdminDashboard.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddControllers();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularDev",
        builder =>
        {
            builder.WithOrigins("http://localhost:4200")
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
});
builder.Services.AddScoped<IEmplyeRepository, EmployeRepository>();
builder.Services.AddScoped<IRepresantantEntreprise, RepresantantEntrepriseRepository>();
builder.Services.AddScoped<IAdminGenerale, AdminGeneraleRepository>();
builder.Services.AddScoped<IUtilisateur, UtilisateurRepository>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<ApplicationDbContext>(options => { var connetionString = builder.Configuration.GetConnectionString("DefaultConnection");
    options.UseMySql(connetionString, ServerVersion.AutoDetect(connetionString)); });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

// Use the CORS policy
app.UseCors("AllowAngularDev");

app.UseAuthorization();
app.MapControllers();

app.Run();
