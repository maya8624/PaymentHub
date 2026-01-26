using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentHub.Application.Dtos
{
    public class FraudPredictionResponse
    {
        public bool IsFraud { get; set; }
        public double ConfidenceScore { get; set; }
        public string AgentDecision { get; set; }
    }
}
