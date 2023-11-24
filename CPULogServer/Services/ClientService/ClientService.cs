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
        private readonly ILogger<ClientModel> _logger;
        private readonly DataContext _context;
        public ClientService(ILogger<ClientModel> logger, DataContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task Store(ClientModel client)
        {
            await _context.Clients.AddAsync(client);
            await _context.SaveChangesAsync();
        }

        public async Task<ClientModel> Get(int id)
        {
            ClientModel result = await _context.Clients.SingleOrDefaultAsync<ClientModel>(c => c.Id == id);
            return result;
        }
        public async Task<List<ClientModel>> Get()
        {
            List<ClientModel> result = await _context.Clients.ToListAsync<ClientModel>();
            return result;
        }

        public async Task Update(int id, ClientModel client)
        {
            ClientModel result = await _context.Clients.SingleOrDefaultAsync<ClientModel>(c => c.Id == id);
            _context.Entry(result).CurrentValues.SetValues(client);
        }
        public async Task Delete(int id)
        {
            ClientModel result = await _context.Clients.SingleOrDefaultAsync<ClientModel>(c => c.Id == id);
            _context.Entry(result).State = EntityState.Detached;
        }
    }
}
