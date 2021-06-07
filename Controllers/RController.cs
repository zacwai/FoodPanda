using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Panda.Controllers
{
    public class RController : Controller
    {
        public IActionResult Index(string id)
        {
            return RedirectToAction("Index", "register", new { id });
        }
    }
}
