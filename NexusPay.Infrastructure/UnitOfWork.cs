using Microsoft.EntityFrameworkCore;
using NexusPay.Infrastructure.Interfaces;
using NexusPay.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NexusPay.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly NexusPayContext _context;

        public UnitOfWork(NexusPayContext context)
        {
            _context = context;
        }

        public async Task<int> SaveChanges()
        {
            var result = await _context.SaveChangesAsync();
            return result;
        }

    }
}
