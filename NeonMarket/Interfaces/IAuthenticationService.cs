using Microsoft.AspNetCore.Identity;
using NeonMarket.Models;
using NeonMarket.ViewModels.AuthenticationRelated;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace NeonMarket.Interfaces
{
    public interface IAuthenticationService
    {

        public Task<IdentityResult> RegisteUserAsync(RegisterCustomerVM model);
        ClaimsIdentity GetClaimsIdentity(User model);
        JwtSecurityToken GetJWTToken(IEnumerable<Claim> claims);
        string GetAccessToken(JwtSecurityToken jwtToken);
        public Task<bool> IsInRole(User user, string role);


    }
}
