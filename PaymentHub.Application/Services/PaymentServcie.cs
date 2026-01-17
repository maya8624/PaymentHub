using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PaymentHub.Application.Exceptions;
using PaymentHub.Application.Interfaces;
using PaymentHub.Domain.Enums;
using PaymentHub.Infrastructure.Interfaces;
using PayPalIntegration.Domain.Entities;
using PayPalIntegration.Domain.Enums;
using PayPalIntegration.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentHub.Application.Services
{
    public class PaymentServcie : IPaymentService
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly ILogger<PaymentServcie> _logger;
        private readonly IUnitOfWork _uow;

        public PaymentServcie(IPaymentRepository paymentRepository, ILogger<PaymentServcie> logger, IUnitOfWork uow)
        {
            _paymentRepository = paymentRepository;
            _logger = logger;
            _uow = uow;
        }

        public async Task SavePayment(Payment payment)
        {
            try
            {
                await _paymentRepository.Create(payment);
                await _uow.SaveChanges();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to save Payment record for Order {OrderId}", payment.OrderId);
                throw new PaymentSaveException(payment.OrderId, "Could not save payment record. Please try again.");
            }
        }

        public async Task UpdatePayment(Payment payment)
        {
            try
            {
                _paymentRepository.Update(payment);
                await _uow.SaveChanges();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to update Payment record for Order {OrderId}", payment.OrderId);
                throw new PaymentSaveException(payment.OrderId, "Failed to process payment. Please try again.");
            }
        }
    }
}
