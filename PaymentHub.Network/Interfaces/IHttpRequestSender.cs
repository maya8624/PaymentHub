using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentHub.Network.Interfaces
{
    public interface IHttpRequestSender
    {
        Task<T> ExecuteRequest<T>(HttpRequestMessage request);//, CancellationToken ct);
        Task ExecuteRequest(HttpRequestMessage request, CancellationToken ct);
    }
}
