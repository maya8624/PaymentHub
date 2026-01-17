using PayPalIntegration.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentHub.Application.Dtos
{
    public class OrderResponse
    {
        public int OrderId { get; set; }

        public OrderStatus Status { get; set; }  // Pending, Paid, Failed

        public decimal TotalAmount { get; set; }

        /// <summary>
        /// Echoed back so frontend can safely reuse it after refresh
        /// </summary>
        public string FrontendIdempotencyKey { get; set; }

        public List<OrderItemResponse> Items { get; set; } = [];

        public DateTimeOffset CreatedAt { get; set; }
    }
}
