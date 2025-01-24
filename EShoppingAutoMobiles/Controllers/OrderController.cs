using Microsoft.AspNetCore.Mvc;

namespace EShoppingAPI.Controllers
{
    public class OrderController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
