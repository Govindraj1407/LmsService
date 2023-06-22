using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ViewModels
{
    public class UserViewModel
    {
        public string UserId { get; set; }

        public string Name { get; set; }

        [Required]
        public string Role { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        public string Pin { get; set; }

        public string Phone { get; set; }

        public string Email { get; set; }

    }
}
