using Microsoft.AspNetCore.Mvc;

namespace EShoppingAutoMobiles.Controllers
{
    public class OrderController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
