using PaymentHub.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentHub.Application.Interfaces
{
    public interface IRefundService
    {
        Task<Refund> Refund(int paymentId, decimal amount, CancellationToken ct);
    }
}
