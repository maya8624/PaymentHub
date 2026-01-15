using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using PayPalIntegration.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayPalIntegration.Infrastructure.Persistence
{
    public class PayPalDbContext : DbContext
    {
        public PayPalDbContext(DbContextOptions<PayPalDbContext> options) : base(options)
        {
        }
        

        public DbSet<Order> Business { get; set; }
        public DbSet<Payment> Customers { get; set; }
    }
}
