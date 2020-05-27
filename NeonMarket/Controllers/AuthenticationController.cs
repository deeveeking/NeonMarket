using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NeonMarket.Controllers
{
    public class AuthenticationController : Controller
    {

        public IActionResult Login()
        {
            return View();
        }

        public IActionResult RegisterCustomer()
        {
            return View();
        }

        [Authorize(Roles = Constants.ROLE_ADMIN)]
        public IActionResult RegisterSupportModerator()
        {
            return View();
        }
    }
}
