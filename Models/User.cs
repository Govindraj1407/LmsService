using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Models
{
    public class User
    {
        public string UserId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Role { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        public string Pin { get; set; }

        [Required]
        public string Phone { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        public int PointOfContact { get; set; }

    }
}
