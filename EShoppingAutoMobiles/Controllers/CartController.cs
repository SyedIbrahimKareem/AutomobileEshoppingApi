using Microsoft.AspNetCore.Mvc;

namespace EShoppingAutoMobiles.Controllers
{
    public class CartController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
