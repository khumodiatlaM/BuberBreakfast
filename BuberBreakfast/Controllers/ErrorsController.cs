using Microsoft.AspNetCore.Mvc;

namespace BuberBreakfast.Controllers
{
    public class ErrorsController : ControllerBase
    {
        [Route("/error")]
        public IActionResult Index()
        {
            return Problem();
        }
    }
}
