using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using StudentControl.Domain.Model;

namespace StudentControl.DTO
{
    public class GroupDTO
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = null!;
        public int NumberOfPeople { get; set; } = 0;
        public Formofstudy Formofstudy { get; set; }
        public string StudyProfile { get; set; } = string.Empty;

    }
}
