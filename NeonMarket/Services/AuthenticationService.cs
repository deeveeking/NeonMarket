using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using NeonMarket.Interfaces;
using NeonMarket.Models;
using NeonMarket.ViewModels.AuthenticationRelated;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace NeonMarket.Services
{
    public class AuthenticationService : IAuthenticationService
    {

        private readonly DatabaseContext dataBaseContext;
        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> signInManager;

        public AuthenticationService(DatabaseContext dataBaseContext, UserManager<User> userManager, SignInManager<User> signInManager)
        {
            this.dataBaseContext = dataBaseContext;
            this.userManager = userManager;
            this.signInManager = signInManager;

        }


        public async Task<IdentityResult> RegisteUserAsync(RegisterCustomerVM model)
        {

            User user = new User
            {
                Name = model.User.Name,
                Surname = model.User.Surname,
                UserName = model.User.Email,
                PhoneNumber = model.User.PhoneNumber
            };


            return await this.userManager.CreateAsync(user, model.Password);
        }

       

        public async Task<bool> IsInRole(User user, string role)
        {
            return await userManager.IsInRoleAsync(user, role);
        }

        public string GetAccessToken(JwtSecurityToken jwtToken)
        {
            return new JwtSecurityTokenHandler().WriteToken(jwtToken);
        }

        public JwtSecurityToken GetJWTToken(IEnumerable<Claim> claims)
        {

            DateTime now = DateTime.UtcNow;

            JwtSecurityToken jwt = new JwtSecurityToken(
                    issuer: Constants.ISSUER,
                    audience: Constants.AUDIENCE,
                    notBefore: now,
                    claims: claims,
                    //   expires: now.Add(TimeSpan.FromDays(AuthOptions.LIFETIME)),
                    signingCredentials: new SigningCredentials(Constants.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));

            return jwt;
        }


        public ClaimsIdentity GetClaimsIdentity(User model)
        {
            return getClaimsIdentity(model.Name, Constants.ROLE_CUSTOMER);
        }

        private ClaimsIdentity getClaimsIdentity(string phoneNumber, string role)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, phoneNumber),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, role)
            };

            ClaimsIdentity claimsIdentity =
            new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType,
                ClaimsIdentity.DefaultRoleClaimType);

            return claimsIdentity;

        }
    }
}
