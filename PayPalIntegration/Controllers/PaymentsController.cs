using Microsoft.AspNetCore.Mvc;

namespace PayPalIntegration.Controllers
{
    public class PaymentsController : ControllerBase
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
