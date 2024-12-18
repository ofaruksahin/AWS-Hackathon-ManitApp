using ManitApp.API.Application.Services;
using ManitApp.API.Application.Services.Contracts;
using ManitApp.API.Infrastructure;
using ManitApp.API.Workers;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ManitAppDbContext>();
builder.Services.AddScoped<IVectorizeService, VectorizeService>();
//builder.Services.AddHostedService<SeedWorker>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
