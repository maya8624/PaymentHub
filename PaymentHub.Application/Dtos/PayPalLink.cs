using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentHub.Application.Dtos
{
    public class PayPalLink
    {
        public string Href { get; set; } = null!;
        public string Rel { get; set; } = null!;
        public string Method { get; set; } = null!;
    }
}
