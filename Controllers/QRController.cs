using Microsoft.AspNetCore.Mvc;
using Panda.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Panda.Controllers
{
    public class QRController : Controller
    {
        public IActionResult Index(string id = "panda")
        {
            PublicViewOutput result = new PublicViewOutput(id);
            result.PublicObject = ZC.QRGenerator.GenerateBase64Image(result.URL + "/r/" + id);
            return View(result);
        }
    }
}
