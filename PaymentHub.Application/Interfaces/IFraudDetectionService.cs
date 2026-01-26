using PaymentHub.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentHub.Application.Interfaces
{
    public interface IFraudDetectionService
    {
        Task<FraudPredictionResponse> CheckTransaction(FraudPredictionRequest request);
    }
}
