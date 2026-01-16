using Microsoft.EntityFrameworkCore;
using PayPalIntegration.Domain.Interfaces;
using PayPalIntegration.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayPalIntegration.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly PayHubContext _context;

        public UnitOfWork(PayHubContext context)
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
