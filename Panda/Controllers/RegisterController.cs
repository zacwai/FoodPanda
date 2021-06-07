using Microsoft.AspNetCore.Mvc;
using Panda.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Panda.Controllers
{
    public class RegisterController : Controller
    {
        public IActionResult Index(string id = "panda")
        {
            PublicViewOutput result = new PublicViewOutput(id);

            return View(result);
        }
        public List<ErrorViewModel> _Reg(RegistrationInput input)
        {
            List<ErrorViewModel> result = new List<ErrorViewModel>();
            Regex strPattern = new Regex("[0-9A-Za-z_@. ]*");

            if (string.IsNullOrEmpty(input.Contact) || !long.TryParse(input.Contact, out var _contact) || input.Contact.Length >= 12 || input.Contact.Length < 10)
            {
                result.Add(new ErrorViewModel() { RequestId = "2", ErrMsg = "Invalid contact number" });
            }
            else if (Cache.Member.ContactExsist(input.Contact))
            {
                result.Add(new ErrorViewModel() { RequestId = "2", ErrMsg = "The phone number is already used for this promo" });
            }
            if (string.IsNullOrEmpty(input.FullName) || !strPattern.IsMatch(input.FullName))
            {
                result.Add(new ErrorViewModel() { RequestId = "3", ErrMsg = "Invalid name" });
            }
            if (string.IsNullOrEmpty(input.Email) || !ZC.Util.IsValidEmail(input.Email))
            {
                result.Add(new ErrorViewModel() { RequestId = "1", ErrMsg = "Invalid email" });
            }
            if(result.Count()==0)
            {
                Services.Member service = new Services.Member();
                var errMsg = service.Register(input);
                if (!string.IsNullOrEmpty(errMsg))
                    result.Add(new ErrorViewModel() { RequestId = "4", ErrMsg = errMsg });
            }

            return result;
        }
    }
}
