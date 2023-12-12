using Microsoft.EntityFrameworkCore;
using StudentControl.Domain.Model;
using StudentControl.Infrastructure;
using StudentControl.Infrastructure.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestProject
{
    public class TestHelper
    {
        private readonly Context context;

        public GroupRepository GroupRepository { get { return new GroupRepository(context); } }
        public StudentRepository StudentRepository { get { return new StudentRepository(context); } }

        public TestHelper()
        {
            var ContextOptions = new DbContextOptionsBuilder<Context>()
                .UseMySql(@"server=localhost;user=App;password=1234567890!;database=TestDB", new MySqlServerVersion(new Version(8, 0, 30)))
                .Options;

            context = new Context(ContextOptions);
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            var Group1 = new Group { Name = "IST-021", Formofstudy = Formofstudy.Full_time, StudyProfile = "IDK1" };
            var Group2 = new Group { Name = "IST-012", Formofstudy = Formofstudy.Full_time, StudyProfile = "IDK2" };


            var student1 = new Student
            {
                Name = "Name1",
                Surname = "Surname1",
                Status = Status.Studying,
                Graduate = 1
            };
            var student2 = new Student
            {
                Name = "Name2",
                Surname = "Surname2",
                Status = Status.Studying,
                Graduate = 2
            };
            var student3 = new Student
            {
                Name = "Name3",
                Surname = "Surname3",
                Status = Status.Studying,
                Graduate = 1
            };
            var student4 = new Student
            {
                Name = "Name4",
                Surname = "Surname4",
                Status = Status.Studying,
                Graduate = 2
            };

            Group1.AddStudent(student1);
            Group1.AddStudent(student3);
            Group2.AddStudent(student2);
            Group2.AddStudent(student4);
            var Order = new Order { Number = "1", Name = Name.Priem, Date = new DateTime() };
            student1.AddOrder(Order);
            student2.AddOrder(Order);
            student1.Orders.Add(new Order { Number = "21", Name = Name.Group_formation, Date = new DateTime() });
            student2.Orders.Add(new Order { Number = "22", Name = Name.Group_formation, Date = new DateTime() });

            context.Students.AddRange(student1,student2,student3,student4);
            context.Groups.Add(Group1);
            context.Groups.Add(Group2);

            context.SaveChanges();
            context.ChangeTracker.Clear();
        }
    }
}
