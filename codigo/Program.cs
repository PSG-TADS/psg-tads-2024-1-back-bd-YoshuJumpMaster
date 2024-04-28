using Microsoft.EntityFrameworkCore;
using LocadoraDeVeiculos.Models;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.IO;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<LocadoraContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "LocadoraDeVeiculos", Version = "v1" });
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFilename);
    c.IncludeXmlComments(xmlPath);
});



var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

/// <summary>
/// Obtém uma lista de todos os veículos.
/// </summary>
/// <returns>Uma lista de veículos.</returns>
/// <response code="200">Se a lista de veículos for recuperada com sucesso.</response>
app.MapGet("/veiculos", async (LocadoraContext context) =>
{
    var veiculos = await context.Veiculos.ToListAsync();
    return Results.Ok(veiculos);
});



/// <summary>
/// Obtém o véiculo com aquele ID.
/// </summary>
/// <param name="id">O ID do veículo a ser obtido.</param>
/// <returns>Os detalhes do veículo.</returns>
/// <response code="200">Retorna o veículo encontrado.</response>
/// <response code="404">Se nenhum veículo for encontrado com aquele ID.</response>
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


/// <summary>
/// Cria veículo.
/// </summary>
/// <param name="veiculo">O objeto veículo com este nome será criado.</param>
/// <returns>O veículo criado.</returns>
/// <response code="201"> veículo criado com sucesso.</response>

app.MapPost("/veiculos", async (LocadoraContext context, Veiculo veiculo) =>
{
    context.Veiculos.Add(veiculo);
    await context.SaveChangesAsync();
    return Results.Created($"/veiculos/{veiculo.VeiculoId}", veiculo);
});



/// <summary>
/// Atualiza veículo.
/// </summary>
/// <param name="id">ID do veículo a ser atualizado.</param>
/// <param name="veiculoInput">dados atualizados do veículo.</param>
/// <returns>Nenhum conteúdo, confirmando que o veículo foi atualizado.</returns>
/// <response code="204">O veículo foi atualizado com sucesso.</response>
/// <response code="404">Nenhum veículo encontrado com este ID.</response>

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



/// <summary>
/// Deleta veículo.
/// </summary>
/// <param name="id">O ID do veículo a ser deletado.</param>
/// <returns>Nenhum conteúdo, confirmando que o veículo foi deletado.</returns>
/// <response code="204"> veículo deletado com sucesso.</response>
/// <response code="404">Nenhum veículo encontrado com esse ID.</response>
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



/// <summary>
/// Obtém todos os clientes armazenados.
/// </summary>
/// <returns>Uma lista de clientes.</returns>
/// <response code="200">Retorna a lista de clientes.</response>
app.MapGet("/clientes", async (LocadoraContext context) =>
{
    var clientes = await context.Clientes.ToListAsync();
    return Results.Ok(clientes);
});


/// <summary>
/// Obtém o cliente com aquele ID.
/// </summary>
/// <param name="id">O ID do cliente.</param>
/// <returns>Os detalhes do cliente.</returns>
/// <response code="200">Retorna o cliente encontrado.</response>
/// <response code="404">Se nenhum cliente para aquele ID.</response>
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




/// <summary>
/// Cria cliente.
/// </summary>
/// <param name="cliente">dados do novo cliente.</param>
/// <returns> Detalhes do cliente.</returns>
/// <response code="201"> cliente criado.</response>
app.MapPost("/clientes", async (LocadoraContext context, Cliente cliente) =>
{
    context.Clientes.Add(cliente);
    await context.SaveChangesAsync();
    return Results.Created($"/clientes/{cliente.ClienteId}", cliente);
});







/// <summary>
/// Atualiza um cliente .
/// </summary>
/// <param name="id"> ID do cliente a ser atualizado.</param>
/// <param name="clienteInput"> dados atualizados do cliente </param>
/// <response code="204"> cliente foi atualizado com sucesso </response>
/// <response code="404"> nenhum cliente  encontrado para esse ID.</response>
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



/// <summary>
/// Excluir cliente .
/// </summary>
/// <param name="id"> ID do cliente a ser excluído </param>
/// <response code="204"> Cliente excluído com sucesso </response>
/// <response code="404"> nenhum cliente corresponde esse ID</response>
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



/// <summary>
/// Obtém reservas armazenadas.
/// </summary>
/// <returns>Uma lista de reservas.</returns>
/// <response code="200">Retorna a lista de reservas.</response>
app.MapGet("/reservas", async (LocadoraContext context) =>
{
    var reservas = await context.Reservas.ToListAsync();
    return Results.Ok(reservas);
});




/// <summary>
/// Obtém a reserva com aquele ID.
/// </summary>
/// <param name="id">O ID daquela reserva.</param>
/// <returns>Os detalhes da reserva.</returns>
/// <response code="200">Retorna a reserva encontrada.</response>
/// <response code="404">Se nenhuma reserva armazenada corresponde com aquele ID.</response>
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




/// <summary>
/// Cria reserva.
/// </summary>
/// <param name="reserva"> dados da reserva </param>
/// <returns> detalhes da reserva </returns>
/// <response code="201"> Retorna reserva criada </response>
app.MapPost("/reservas", async (LocadoraContext context, Reserva reserva) =>
{
    context.Reservas.Add(reserva);
    await context.SaveChangesAsync();
    return Results.Created($"/reservas/{reserva.ReservaId}", reserva);
});



/// <summary>
/// Atualiza reserva .
/// </summary>
/// <param name="id"> ID da reserva a ser atualizada </param>
/// <param name="reservaInput"> dados atualizados da reserva </param>
/// <response code="204"> Reserva foi atualizada com sucesso </response>
/// <response code="404"> nenhuma reserva encontrada com esse ID </response>
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



/// <summary>
/// Excluir reserva .
/// </summary>
/// <param name="id"> ID  da reserva para exluir </param>
/// <response code="204"> reserva excluída com sucesso.</response>
/// <response code="404"> nenhuma reserva corresponde esse ID</response>
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
