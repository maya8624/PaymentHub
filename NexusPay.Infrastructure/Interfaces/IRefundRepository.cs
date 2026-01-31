using NexusPay.Domain.Entities;
using NexusPay.Domain.Entities;
using NexusPay.Infrastructure.Interfaces;
using System;

namespace NexusPay.Infrastructure.Interfaces
{
    public interface IRefundRepository : IRepositoryBase<Refund>
    {
    }
}
