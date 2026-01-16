using PayPalIntegration.Application.Interfaces;
using PayPalIntegration.Application.Requests;
using PayPalIntegration.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayPalIntegration.Application.Services
{
    public class OrderService : IOrderService
    {
        public Task<Order> CreateOrder(CreateOrderRequest request)
        {
            throw new NotImplementedException();
        }

        //public async Task<Order> CreateOrderAsync(string orderNumber, decimal totalAmount)
        //{
        //    var order = new Order { OrderNumber = orderNumber, TotalAmount = totalAmount };
        //    await _orderRepository.AddAsync(order);
        //    await _orderRepository.SaveChangesAsync();
        //    return order;
        //}

    }
}
