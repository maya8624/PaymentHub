using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayPalIntegration.Domain.Interfaces
{
    public interface IUnitOfWork
    {
        Task<int> SaveChanges();
    }
}
