using Microsoft.AspNetCore.Mvc;

namespace cis237_inclass_6.Controllers
{
    public class FoobarController : Controller
    {
        public IActionResult FoobarAction()
        {
            return Content("This is the foobar route.");
        }
    }
}
