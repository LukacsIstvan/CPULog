﻿using CPULogServer.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Net.Sockets;
using System.Net;
using System.Threading.Tasks;
using CPULogServer.Models;
using System.Linq;
using System.Collections.Generic;

namespace CPULogServer.Services.CPUDataService
{
    public class CPUDataService : ICPUDataService
    {
        private readonly ILogger<CPUDataService> _logger;
        private readonly DataContext _context;
        public CPUDataService(ILogger<CPUDataService> logger, DataContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task Store(CPUData cpuData)
        {
            await _context.CPUData.AddAsync(cpuData);
            await _context.SaveChangesAsync();
        }
        public async Task<List<CPUData>> GetByClient(int id)
        {
            List<CPUData> result = await _context.CPUData.Where(c => c.ClientModel.Id==id).ToListAsync<CPUData>();
            return result;
        }

        public async Task<CPUData> Get(int id)
        {
            CPUData result = await _context.CPUData.SingleOrDefaultAsync<CPUData>(c => c.Id == id);
            return result;
        }
        public async Task<List<CPUData>> Get()
        {
            List<CPUData> result = await _context.CPUData.ToListAsync<CPUData>();
            return result;
        }

        public async Task Update(int id, CPUData cpuData)
        {
            CPUData result = await _context.CPUData.SingleOrDefaultAsync<CPUData>(c => c.Id == id);
            _context.Entry(result).CurrentValues.SetValues(cpuData);
        }
        public async Task Delete(int id)
        {
            CPUData result = await _context.CPUData.SingleOrDefaultAsync<CPUData>(c => c.Id == id);
            _context.Entry(result).State = EntityState.Detached;
        }
    }
}
