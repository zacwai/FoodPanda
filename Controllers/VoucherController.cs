using Microsoft.AspNetCore.Mvc;
using Panda.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Panda.Controllers
{
    public class VoucherController : Controller
    {
        public IActionResult Index(string id)
        {
            Services.PromoCode service = new Services.PromoCode();
            PublicViewOutput result = service.Verify(id);
            return View(result);
        }
    }
}
