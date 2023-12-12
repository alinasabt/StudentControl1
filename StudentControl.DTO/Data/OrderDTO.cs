using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using StudentControl.Domain.Model;

namespace StudentControl.DTO
{

    public class OrderDTO
    {
        public Guid Id { get; set; }
        public string Number { get; set; } = string.Empty;
        public Name Name { get; set; }
        public DateTime Date { get; set; }
        public string Comment { get; set; } = string.Empty;

     
    }
}
