
using APIDirecciones.Model.IDAO.IRepository;
using APIDirecciones.Model.DAO.Repository;
using APIDirecciones.Model.IDAO.IServiceDAO;
using APIDirecciones.Model.DAO.ServicesDAO;
using APIDirecciones.Postgres;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Inyectamos el repositorio

builder.Services.AddScoped<IServiceDirecciones, ServiceDirecciones>();
builder.Services.AddScoped<IRepositoryDirecciones, RepositoryDirecciones>();
builder.Services.AddScoped<ISqlTools>(provider =>
new SqlTools(connectionString));
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();


