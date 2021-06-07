using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Panda.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Panda.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index(string id="panda")
        {

            PublicViewOutput result = new PublicViewOutput(id);
            result.PublicObject = ZC.QRGenerator.GenerateBase64Image(result.URL + "/r/" + id);
            return View(result);

            //return RedirectToAction("Index", "QR", new { id });
            //return RedirectToAction("Index","Admin", new { id = 3712240 });
            //return RedirectToAction("Index", "Super", new { id = "klickpanda64" });
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
