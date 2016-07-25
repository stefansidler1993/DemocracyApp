using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemocracyApp.Classes
{
    public class User
    {
        public int UserId { get; set; }

        public string UserName { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string FullName { get { return string.Format("{0} {1}", this.FirstName, this.LastName); } }

        public string Phone { get; set; }

        public string Adress { get; set; }

        public string Grade { get; set; }

        public string Group { get; set; }

    }

}
