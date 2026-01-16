using PayPalIntegration.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayPalIntegration.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        public Task<int> SaveChanges()
        {
            throw new NotImplementedException();
        }
    }
}
