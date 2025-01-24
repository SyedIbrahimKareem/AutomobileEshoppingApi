using Microsoft.AspNetCore.Mvc;

namespace EShoppingAPI.Controllers
{
    public class CartController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
