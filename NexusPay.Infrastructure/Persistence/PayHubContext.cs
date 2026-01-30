using Microsoft.EntityFrameworkCore;
using PaymentHub.Domain.Entities;
using PayPalIntegration.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayPalIntegration.Infrastructure.Persistence
{
    public class PayHubContext : DbContext
    {
        public PayHubContext(DbContextOptions<PayHubContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }

        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Payment> Payments { get; set; }
    }
}
