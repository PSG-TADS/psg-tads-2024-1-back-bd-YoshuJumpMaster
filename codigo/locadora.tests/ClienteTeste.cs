using LocadoraDeVeiculos.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using System.Net.Http;
using Xunit;

namespace LocadoraDeVeiculos.Tests
{
    public class ClienteControllerTestes
    {
        private readonly HttpClient _client;
        private readonly LocadoraContext _dbContext;

        public ClienteControllerTestes()
        {
            var serviceProvider = new ServiceCollection()
                .AddDbContext<LocadoraContext>(options => options.UseInMemoryDatabase("TestDatabase"), ServiceLifetime.Singleton)
                .BuildServiceProvider();

            _client = new HttpClient
            {
                BaseAddress = new Uri("http://localhost:5236") 
            };

            _dbContext = serviceProvider.GetRequiredService<LocadoraContext>();
        }

        [Fact]
        public async Task GetClientePorId_IdExistente_RetornaOkResult()
        {
            var cliente = new Cliente { Nome = "Jose", CPF = "123456789" };
            _dbContext.Clientes.Add(cliente);
            await _dbContext.SaveChangesAsync();
            var response = await _client.GetAsync($"/clientes/{cliente.ClienteId}");
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}