using GolestanProject.Models;
using Microsoft.AspNetCore.Mvc;

namespace GolestanProject.Controllers
{
    public class InstuctorController : Controller
    {

        private readonly MyAppContext _context;
        public InstuctorController(MyAppContext context)
        {
            _context = context;
        }

        public IActionResult AdminDashboard ()
        {
            var roleSelected = HttpContext.Session.GetString("SelectedRole");
            if (roleSelected != "Instructor")
            {
                return RedirectToAction("SelectRole", "Account");
            }
            return View();
        }
    }
}
