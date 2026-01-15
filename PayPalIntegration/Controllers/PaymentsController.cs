using Microsoft.AspNetCore.Mvc;

namespace PayPalIntegration.Controllers
{
    public class PaymentsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
