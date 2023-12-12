using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using StudentControl.Domain.Model;

namespace StudentControl.DTO
{

    public class StudentDTO
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; } = string.Empty;
        public string Surname { get; set; } = string.Empty;
        public string? Middle_name { get; set; }
        public Status Status { get; set; }
        public short Graduate { get; set; }
        public string? Snils { get; set; }


        
    }
}
