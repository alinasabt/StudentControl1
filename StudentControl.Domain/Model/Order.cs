using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace StudentControl.Domain.Model
{
    public enum Name
    {
        Priem,
        Group_formation,
        Electives,
        Distribution_by_bases_of_practice,
        Transfer,
        Grants
    }
    public class Order
    {
        public Guid Id { get; set; }
        public string Number { get; set; } = string.Empty;
        public Name Name { get; set; }
        public DateTime Date { get; set; }
        public string Comment { get; set; } = string.Empty;


        public List<Student> Students { get; set; } = new List<Student>();


        public void AddStudent(Student student)
        {
            if(!Students.Any(stud => stud.Id == student.Id))
            { 
                Students.Add(student);
                student.AddOrder(this);
            }
            
        }

        public void RemoveStudent(Student student)
        {
            var CurStudent = Students.FirstOrDefault(stud => stud.Id == student.Id);
            if(CurStudent != null)
            {
                Students.Remove(CurStudent);
                CurStudent.RemoveOrder(this);
            }

        }
    }
}
