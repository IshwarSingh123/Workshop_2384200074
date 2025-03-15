using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLayer.Model
{
    public class AddressBookModel
    {
        
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string Country { get; set; }

        public string  ToString()
        {
            return $"{FirstName}\n{LastName}\n{Email}" +
                    $"\n{PhoneNumber}\n{Address}\n{Country}";
        }
    }
}
