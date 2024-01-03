using Microsoft.eShopOnContainers.Services.Ordering.API.Infrastructure;
using Microsoft.eShopOnContainers.Services.Ordering.Infrastructure;
using Ordening.API.Extensions;
using Ordering.API.Infrastructure.Middlewares;
using Ordering.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Registro de dependencias de otras capas de la aplicaci�n (patr�n cadena de responsabilidad)
builder.Services.RegisterApplicationServices(builder.Configuration);
builder.Services.RegisterInfrastructureServices(builder.Configuration);

var app = builder.Build();

//Rellena la base de datos con los datos b�sicos y necesarios para configurar la aplicaci�n
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    var context = services.GetRequiredService<OrderingContext>();
    new OrderingContextSeed()
                .SeedAsync(context)
                .Wait();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// A�ade los Middleware, en este caso solo uno para hacer byPass en la autenticaci�n y autorizaci�n
app.UseByPassAuthMiddleware();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
