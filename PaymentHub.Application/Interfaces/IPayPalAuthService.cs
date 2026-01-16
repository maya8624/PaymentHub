using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentHub.Application.Interfaces
{
    public interface IPayPalAuthService
    {
        Task<string> GetAccessToken();
    }
}
