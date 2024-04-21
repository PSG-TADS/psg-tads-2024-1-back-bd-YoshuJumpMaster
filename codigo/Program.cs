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
    var veiculos = await context.Veiculos.ToListAsync();
    return Results.Ok(veiculos);
});

app.MapGet("/veiculos/{id}", async (LocadoraContext context, int id) =>
{
    var veiculo = await context.Veiculos.FindAsync(id);
    if (veiculo != null)
    {
        return Results.Ok(veiculo);
    }
    else
    {
        return Results.NotFound();
    }
});

app.MapPost("/veiculos", async (LocadoraContext context, Veiculo veiculo) =>
{
    context.Veiculos.Add(veiculo);
    await context.SaveChangesAsync();
    return Results.Created($"/veiculos/{veiculo.VeiculoId}", veiculo);
});

app.MapPut("/veiculos/{id}", async (LocadoraContext context, int id, Veiculo veiculoInput) =>
{
    var veiculo = await context.Veiculos.FindAsync(id);
    if (veiculo == null)
    {
        return Results.NotFound();
    }
    veiculo.Marca = veiculoInput.Marca;
    veiculo.Modelo = veiculoInput.Modelo;
    veiculo.Placa = veiculoInput.Placa;
    veiculo.Status = veiculoInput.Status;

    await context.SaveChangesAsync();
    return Results.NoContent();
});

app.MapDelete("/veiculos/{id}", async (LocadoraContext context, int id) =>
{
    var veiculo = await context.Veiculos.FindAsync(id);
    if (veiculo == null)
    {
        return Results.NotFound();
    }
    context.Veiculos.Remove(veiculo);
    await context.SaveChangesAsync();
    return Results.NoContent();
});
app.MapGet("/clientes", async (LocadoraContext context) =>
{
    var clientes = await context.Clientes.ToListAsync();
    return Results.Ok(clientes);
});

app.MapGet("/clientes/{id}", async (LocadoraContext context, int id) =>
{
    var cliente = await context.Clientes.FindAsync(id);
    if (cliente != null)
    {
        return Results.Ok(cliente);
    }
    else
    {
        return Results.NotFound();
    }
});

app.MapPost("/clientes", async (LocadoraContext context, Cliente cliente) =>
{
    context.Clientes.Add(cliente);
    await context.SaveChangesAsync();
    return Results.Created($"/clientes/{cliente.ClienteId}", cliente);
});

app.MapPut("/clientes/{id}", async (LocadoraContext context, int id, Cliente clienteInput) =>
{
    var cliente = await context.Clientes.FindAsync(id);
    if (cliente == null)
    {
        return Results.NotFound();
    }
    cliente.Nome = clienteInput.Nome;
    cliente.CPF = clienteInput.CPF;

    await context.SaveChangesAsync();
    return Results.NoContent();
});

app.MapDelete("/clientes/{id}", async (LocadoraContext context, int id) =>
{
    var cliente = await context.Clientes.FindAsync(id);
    if (cliente == null)
    {
        return Results.NotFound();
    }
    context.Clientes.Remove(cliente);
    await context.SaveChangesAsync();
    return Results.NoContent();
});
app.MapGet("/reservas", async (LocadoraContext context) =>
{
    var reservas = await context.Reservas.ToListAsync();
    return Results.Ok(reservas);
});

app.MapGet("/reservas/{id}", async (LocadoraContext context, int id) =>
{
    var reserva = await context.Reservas.FindAsync(id);
    if (reserva != null)
    {
        return Results.Ok(reserva);
    }
    else
    {
        return Results.NotFound();
    }
});

app.MapPost("/reservas", async (LocadoraContext context, Reserva reserva) =>
{
    context.Reservas.Add(reserva);
    await context.SaveChangesAsync();
    return Results.Created($"/reservas/{reserva.ReservaId}", reserva);
});


app.MapPut("/reservas/{id}", async (LocadoraContext context, int id, Reserva reservaInput) =>
{
    var reserva = await context.Reservas.FindAsync(id);
    if (reserva == null)
    {
        return Results.NotFound();
    }
    reserva.DataInicio = reservaInput.DataInicio;
    reserva.DataFim = reservaInput.DataFim;
    reserva.ClienteId = reservaInput.ClienteId;
    reserva.VeiculoId = reservaInput.VeiculoId;
    reserva.ValorTotal = reservaInput.ValorTotal;

    await context.SaveChangesAsync();
    return Results.NoContent();
});

app.MapDelete("/reservas/{id}", async (LocadoraContext context, int id) =>
{
    var reserva = await context.Reservas.FindAsync(id);
    if (reserva == null)
    {
        return Results.NotFound();
    }
    context.Reservas.Remove(reserva);
    await context.SaveChangesAsync();
    return Results.NoContent();
});

app.Run();
