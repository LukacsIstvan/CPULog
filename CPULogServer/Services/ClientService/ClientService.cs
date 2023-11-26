using CPULogServer.Data;
using CPULogServer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CPULogServer.Services.ClientService
{
    public class ClientService : IClientService
    {
        private readonly ILogger<Client> _logger;
        private readonly DataContext _context;
        public ClientService(ILogger<Client> logger, DataContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task Store(Client client)
        {
            await _context.Clients.AddAsync(client);
            await _context.SaveChangesAsync();
        }

        public async Task<Client> Get(int id)
        {
            Client result = await _context.Clients.SingleOrDefaultAsync<Client>(c => c.Id == id);
            return result;
        }
        public async Task<List<Client>> Get()
        {
            List<Client> result = await _context.Clients.ToListAsync<Client>();
            return result;
        }

        public async Task Update(int id, Client client)
        {
            Client result = await _context.Clients.SingleOrDefaultAsync<Client>(c => c.Id == id);
            _context.Entry(result).CurrentValues.SetValues(client);
        }
        public async Task Delete(int id)
        {
            Client result = await _context.Clients.SingleOrDefaultAsync<Client>(c => c.Id == id);
            _context.Entry(result).State = EntityState.Detached;
        }
        public async Task SetSensor(int id, double value)
        {
            Client client = await _context.Clients.FirstAsync(c => c.Id == id);
            client.SensorTimer = value;
            _context.Clients.Update(client);
            await _context.SaveChangesAsync();
        }
    }
}
