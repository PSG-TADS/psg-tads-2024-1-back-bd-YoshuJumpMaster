using LocadoraDeVeiculos.Data;
using LocadoraDeVeiculos.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Reflection;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<LocadoraContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "LocadoraDeVeiculos", Version = "v1" });
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFilename);
    c.IncludeXmlComments(xmlPath);
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        builder =>
        {
            builder.AllowAnyOrigin()
                   .AllowAnyMethod()
                   .AllowAnyHeader();
        });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
    
}
else
{
    // ignora redirecionamento https por enquanto
}

app.UseCors("AllowAllOrigins"); // Habilita CORS
app.UseAuthorization();

app.MapControllers(); // Adiciona os controllers





//registro
app.MapPost("/api/auth/register", async (LocadoraContext context, [FromBody] RegisterModel model) =>
{
    var cliente = new Cliente
    {
        Nome = model.Username,
        CPF = model.CPF
    };

    context.Clientes.Add(cliente);
    await context.SaveChangesAsync();

    return Results.Ok(new { Message = "Registration successful" });
});






// Login
app.MapPost("/api/auth/login", (LocadoraContext context, [FromBody] LoginModel model) =>
{
    var user = context.Clientes.SingleOrDefault(u => u.Nome == model.Username);

    if (user != null)
    {
        return Results.Ok(new { token = "Fake JWT Token", username = model.Username });
    }
    return Results.Unauthorized();
});






// Saldo
app.MapGet("/api/saldo", (LocadoraContext context, HttpRequest request) =>
{
    var username = request.Headers["Username"].ToString();
    var user = context.Clientes.SingleOrDefault(u => u.Nome == username);

    if (user != null)
    {
      
        var saldo = context.Pagamentos
            .Where(p => context.Reservas.Any(r => r.ReservaId == p.ReservaId && r.ClienteId == user.ClienteId))
            .Sum(p => p.Valor);
        return Results.Ok(new { saldo });
    }

    return Results.Unauthorized();
});









// Recarga
app.MapPost("/api/recarga", async (LocadoraContext context, [FromBody] RecargaModel model, HttpRequest request) =>
{
    var username = request.Headers["Username"].ToString();
    var user = context.Clientes.SingleOrDefault(u => u.Nome == username);

    if (user != null)
    {
       
        var reserva = new Reserva
        {
            ClienteId = user.ClienteId,
            DataInicio = DateTime.Now,
            DataFim = DateTime.Now.AddDays(1),
            VeiculoId = 1, // Veículo padrão
            ValorTotal = model.Valor
        };

        context.Reservas.Add(reserva);
        await context.SaveChangesAsync();

        var pagamento = new Pagamento
        {
            ReservaId = reserva.ReservaId,
            Valor = model.Valor,
            Data = DateTime.Now
        };

        context.Pagamentos.Add(pagamento);
        await context.SaveChangesAsync();

        return Results.Ok(new { Message = "Recarga realizada com sucesso" });
    }

    return Results.Unauthorized();
});











//  Reserva
app.MapGet("/api/reservas/cliente", (LocadoraContext context, HttpRequest request) =>
{
    var username = request.Headers["Username"].ToString();
    var user = context.Clientes.SingleOrDefault(u => u.Nome == username);

    if (user != null)
    {
        var reservas = context.Reservas
            .Where(r => r.ClienteId == user.ClienteId)
            .Select(r => new
            {
                r.ReservaId,
                VeiculoModelo = r.Veiculo.Modelo,
                r.ValorTotal
            })
            .ToList();

        return Results.Ok(reservas);
    }

    return Results.Unauthorized();
});





// Pagamento
app.MapPost("/api/pagamento", async (LocadoraContext context, HttpRequest request, [FromBody] PagamentoModel pagamentoModel) =>
{
    var username = request.Headers["Username"].ToString();
    var user = context.Clientes.SingleOrDefault(u => u.Nome == username);

    if (user != null)
    {
        var reserva = context.Reservas.SingleOrDefault(r => r.ReservaId == pagamentoModel.ReservaId && r.ClienteId == user.ClienteId);

        if (reserva != null && pagamentoModel.Valor == reserva.ValorTotal)
        {
            // Calcular o saldo do cliente
            var saldoAtual = context.Pagamentos
                .Where(p => context.Reservas.Any(r => r.ReservaId == p.ReservaId && r.ClienteId == user.ClienteId))
                .Sum(p => p.Valor);

            if (saldoAtual >= pagamentoModel.Valor)
            {
                var pagamento = new Pagamento
                {
                    ReservaId = reserva.ReservaId,
                    Valor = -pagamentoModel.Valor, // Deduzindo o valor
                    Data = DateTime.Now
                };

                context.Pagamentos.Add(pagamento);
                await context.SaveChangesAsync();
                return Results.Ok(new { Message = "Pagamento realizado com sucesso!" });
            }
            else
            {
                return Results.BadRequest(new { Message = "Saldo insuficiente." });
            }
        }

        return Results.BadRequest(new { Message = "Reserva não encontrada ou valor incorreto." });
    }

    return Results.Unauthorized();
});














// veiculo
app.MapGet("/api/veiculos", async (LocadoraContext context) =>
{
    var veiculos = await context.Veiculos.ToListAsync();
    return Results.Ok(veiculos);
});

app.MapGet("/api/veiculos/{id}", async (LocadoraContext context, int id) =>
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

app.MapPost("/api/veiculos", async (LocadoraContext context, Veiculo veiculo) =>
{
    context.Veiculos.Add(veiculo);
    await context.SaveChangesAsync();
    return Results.Created($"/api/veiculos/{veiculo.VeiculoId}", veiculo);
});

app.MapPut("/api/veiculos/{id}", async (LocadoraContext context, int id, Veiculo veiculoInput) =>
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

app.MapDelete("/api/veiculos/{id}", async (LocadoraContext context, int id) =>
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











// cliente
app.MapGet("/api/clientes", async (LocadoraContext context) =>
{
    var clientes = await context.Clientes.ToListAsync();
    return Results.Ok(clientes);
});

app.MapGet("/api/clientes/{id}", async (LocadoraContext context, int id) =>
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

app.MapPost("/api/clientes", async (LocadoraContext context, Cliente cliente) =>
{
    context.Clientes.Add(cliente);
    await context.SaveChangesAsync();
    return Results.Created($"/api/clientes/{cliente.ClienteId}", cliente);
});

app.MapPut("/api/clientes/{id}", async (LocadoraContext context, int id, Cliente clienteInput) =>
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

app.MapDelete("/api/clientes/{id}", async (LocadoraContext context, int id) =>
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









// reserva
app.MapGet("/api/reservas", async (LocadoraContext context) =>
{
    var reservas = await context.Reservas.ToListAsync();
    return Results.Ok(reservas);
});

app.MapGet("/api/reservas/{id}", async (LocadoraContext context, int id) =>
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

app.MapPost("/api/reservas", async (LocadoraContext context, Reserva reserva) =>
{
    context.Reservas.Add(reserva);
    await context.SaveChangesAsync();
    return Results.Created($"/api/reservas/{reserva.ReservaId}", reserva);
});

app.MapPut("/api/reservas/{id}", async (LocadoraContext context, int id, Reserva reservaInput) =>
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

app.MapDelete("/api/reservas/{id}", async (LocadoraContext context, int id) =>
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










// pagamento
app.MapGet("/api/pagamentos", async (LocadoraContext context) =>
{
    var pagamentos = await context.Pagamentos.ToListAsync();
    return Results.Ok(pagamentos);
});

app.MapGet("/api/pagamentos/{id}", async (LocadoraContext context, int id) =>
{
    var pagamento = await context.Pagamentos.FindAsync(id);
    return pagamento != null ? Results.Ok(pagamento) : Results.NotFound();
});

app.MapPost("/api/pagamentos", async (LocadoraContext context, Pagamento pagamento) =>
{
    context.Pagamentos.Add(pagamento);
    await context.SaveChangesAsync();
    return Results.Created($"/api/pagamentos/{pagamento.PagamentoId}", pagamento);
});

app.MapPut("/api/pagamentos/{id}", async (LocadoraContext context, int id, Pagamento pagamentoInput) =>
{
    var pagamento = await context.Pagamentos.FindAsync(id);
    if (pagamento == null)
    {
        return Results.NotFound();
    }
    pagamento.ReservaId = pagamentoInput.ReservaId;
    pagamento.Valor = pagamentoInput.Valor;
    pagamento.Data = pagamentoInput.Data;

    await context.SaveChangesAsync();
    return Results.NoContent();
});

app.MapDelete("/api/pagamentos/{id}", async (LocadoraContext context, int id) =>
{
    var pagamento = await context.Pagamentos.FindAsync(id);
    if (pagamento == null)
    {
        return Results.NotFound();
    }
    context.Pagamentos.Remove(pagamento);
    await context.SaveChangesAsync();
    return Results.NoContent();
});

app.Run();
