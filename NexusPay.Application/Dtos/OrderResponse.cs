using NexusPay.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NexusPay.Application.Dtos
{
    public class OrderResponse
    {
        public int OrderId { get; set; }

        public OrderStatus Status { get; set; } 

        public decimal TotalAmount { get; set; }
        
        public string IdempotencyKey { get; set; }

        public List<OrderItemResponse> Items { get; set; } = [];

        public DateTimeOffset CreatedAt { get; set; }
    }
}
