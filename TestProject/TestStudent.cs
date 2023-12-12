using Microsoft.EntityFrameworkCore;
using StudentControl.Domain.Model;
using StudentControl.Infrastructure;
using StudentControl.Infrastructure.Repository;

namespace TestProject
{
    public class TestStudent
    {

        [Fact]
        public void Create_DB()
        {
            var StudentTest = new TestHelper();
            var Repository = StudentTest.StudentRepository;

            Assert.Equal(1, 1);
        }

        [Fact]
        public void Add_in_DB()
        {
            var StudentTest = new TestHelper();
            var Repository = StudentTest.StudentRepository;

            var student = new Student
            {
                Name = "TempName",
                Surname = "TempSurname",
                Status = Status.Studying,
                Graduate = 1,
            };

            Repository.AddAsync(student).Wait();
            Repository.ChangeTrackerClear();

            Assert.True(Repository.GetAllAsync().Result.Count == 5);
            Assert.Equal("Name1", Repository.GetByNameAsync("Name1").Result.Name);
            Assert.Equal("Name2", Repository.GetByNameAsync("Name2").Result.Name);
            Assert.Equal("Name3", Repository.GetByNameAsync("Name3").Result.Name);
            Assert.Equal("Name4", Repository.GetByNameAsync("Name4").Result.Name);
            Assert.Equal("TempName", Repository.GetByNameAsync("TempName").Result.Name);
            Assert.Equal(2, Repository.GetByNameAsync("Name1").Result.Orders.Count);

        }

        [Fact]
        public void Update_in_DB()
        {
            var StudentTest = new TestHelper();
            var Repository = StudentTest.StudentRepository;
            var ExistingStudent = Repository.GetByNameAsync("Name1").Result;
            Repository.ChangeTrackerClear();

            ExistingStudent.Name = "ChangedName1";
            ExistingStudent.Orders.Add(new Order { Number = "3", Name = Name.Transfer, Date = new DateTime() });
            Repository.UpdateAsync(ExistingStudent).Wait();
            


            Assert.Equal("ChangedName1", Repository.GetByNameAsync("ChangedName1").Result.Name);
            Assert.Equal(3, Repository.GetByNameAsync("ChangedName1").Result.Orders.Count);
           
        }

        [Fact]
        public void DeleteUpdate_in_DB()
        {
            var StudentTest = new TestHelper();
            var Repository = StudentTest.StudentRepository;
            var Student = Repository.GetByNameAsync("Name1").Result;

            Repository.ChangeTrackerClear();

            Student.Orders.RemoveAt(0);
            Repository.UpdateAsync(Student).Wait();

            Assert.Equal(1, Repository.GetByNameAsync("Name1").Result.Orders.Count);
        }

    }
}