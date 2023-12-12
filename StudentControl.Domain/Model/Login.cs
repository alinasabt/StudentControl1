using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentControl.Domain.Model
{
    public class Login
    {
        public Guid Id { get; set; } 
        public string Email { get; set; }
        public string Password { get; set; }

        public Guid StudentID { get; set; }
        public Student Student { get; set; } = null!;
    }
}
