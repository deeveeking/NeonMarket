
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NeonMarket.Interfaces;
using NeonMarket.ViewModels.AuthenticationRelated;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace NeonMarket.Controllers
{
    public class AuthenticationController : Controller
    {

        private IAuthenticationService authenticationService;
        private IHttpContextAccessor httpContextAccessor;

        public AuthenticationController(IAuthenticationService authenticationService, IHttpContextAccessor httpContextAccessor)
        {
            this.authenticationService = authenticationService;
            this.httpContextAccessor = httpContextAccessor;
        }


        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginCustomerVM model)
        {
            return View();
        }


        public IActionResult RegisterCustomer() 
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> RegisterCustomer(RegisterCustomerVM model)
        {

            ClaimsIdentity identity = authenticationService.GetClaimsIdentity(model.User);

            if (identity == null)
            {
                ModelState.AddModelError("", "Неправильный логин и (или) пароль");
                return View(model);
            }

            JwtSecurityToken jwtToken     = authenticationService.GetJWTToken(identity.Claims);
            string           access_token = authenticationService.GetAccessToken(jwtToken);

            IdentityResult userIdentity = await authenticationService.RegisteUserAsync(model);

            if (userIdentity == null)
            {

                ModelState.AddModelError("", "Внутренняя ошибка сервера");
                return View(model);
            }

            if (!userIdentity.Succeeded)
            {
                ModelState.AddModelError("", "Пользователь уже существует");
                return View(model);
            }


            // save into cookies
            httpContextAccessor.HttpContext.Response.Cookies.Append(".AspNetCore.Application.Id", access_token,
                new CookieOptions
                {
                 //   MaxAge = TimeSpan.FromMinutes(60)
                }
            );


            return RedirectToAction("Index", "Home");
        }

        [Authorize(Roles = Constants.ROLE_ADMIN)]
        public IActionResult RegisterSupportModerator()
        {
            return View();
        }



        public IActionResult LoginREST()
        {
            return Ok();
        }

        public IActionResult RegisterCustomerREST(RegisterCustomerVM model)
        {
            return Ok();
        }
    }
}
