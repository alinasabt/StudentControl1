using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace StudentControl.Domain.Model
{
    public enum Status
    {
        Studying,
        Notstudying
    }
    public class Student
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; } = string.Empty;
        public string Surname { get; set; } = string.Empty;
        public string? Middle_name { get; set; }
        public Status Status { get; set; }
        public short Graduate { get; set; }
        public string? Snils { get; set; }

        public Guid? GroupID { get; set; }

        public Group? Group { get; set; }

        public List<Order> Orders { get; set; } = new List<Order>();

        public void RemoveGroup()
        {
            GroupID = null;
            Group = null; 
        }
        public void SetGroup(Group group)
        {
            GroupID = group.Id;
            Group = group;
        }

        public void AddOrder(Order order)
        {
            if(!Orders.Any(ord => ord.Id == order.Id))
            {
                Orders.Add(order);
                order.AddStudent(this);
            }
        }

        public void RemoveOrder(Order order)
        {   
            var CurOrder = Orders.FirstOrDefault(ord=> ord.Id == order.Id);
            if(CurOrder!=null)
            {
                Orders.Remove(CurOrder);
                CurOrder.RemoveStudent(this);
            }
        }

    }
}
