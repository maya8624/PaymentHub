using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentHub.Application.Dtos
{
    public class PayPalOrderResponse
    {
        public string Id { get; set; } = null!;
        public string? Status { get; set; }
        public List<PayPalLink>? Links { get; set; }
    }
}
