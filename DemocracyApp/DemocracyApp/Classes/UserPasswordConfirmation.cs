using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkulApp.Classes
{
    public class UserPasswordConfirmation : User
    {
        public string Password { get; set; }

        public string ConfirmPassword { get; set; }
    }

}
