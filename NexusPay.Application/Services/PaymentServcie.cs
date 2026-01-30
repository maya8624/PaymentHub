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
    }
}
