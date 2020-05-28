using NeonMarket.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NeonMarket.ViewModels.AuthenticationRelated
{
    public class RegisterCustomerVM
    {
        public User User { get; set; }
        public string Password { get; set; }
    }
}
