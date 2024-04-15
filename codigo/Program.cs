using Microsoft.EntityFrameworkCore;
using LocadoraDeVeiculos.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<LocadoraContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/veiculos", async (LocadoraContext context) =>
{
    return await context.Veiculos.ToListAsync();
})
.WithName("GetVeiculos");

app.MapPost("/veiculos", async (LocadoraContext context, Veiculo veiculo) =>
{
    context.Veiculos.Add(veiculo);
    await context.SaveChangesAsync();
    return Results.Created($"/veiculos/{veiculo.VeiculoId}", veiculo);
})
.WithName("AddVeiculo");

app.Run();
