using Microsoft.EntityFrameworkCore;
using StudentControl.Domain.Model;
using StudentControl.Infrastructure;
using StudentControl.Infrastructure.Repository;

namespace TestProject
{
    public class TestGroup
    {
       

        [Fact]
        public void Create_DB()
        {
            var StudentTest = new TestHelper();
            var Repository = StudentTest.GroupRepository;

            Assert.Equal(1, 1);
        }

        [Fact]
        public void Add_in_DB()
        {
            var GroupTest = new TestHelper();
            var Repository = GroupTest.GroupRepository;

            var Group = new Group()
            {
                Name = "IST-032",
                NumberOfPeople = 1,
                StudyProfile = "IDK3"
            };

            Group.Students.Add(new Student { Name = "NameTemp", Surname = "SurNameTemp" });

            Repository.AddAsync(Group).Wait();
            Repository.ChangeTrackerClear();

            Assert.True(Repository.GetAllAsync().Result.Count == 3);
            Assert.Equal("IST-021", Repository.GetByNameAsync("IST-021").Result.Name);
            Assert.Equal("IST-012", Repository.GetByNameAsync("IST-012").Result.Name);
            Assert.Equal("IST-032", Repository.GetByNameAsync("IST-032").Result.Name);
            Assert.Equal(1, Repository.GetByNameAsync("IST-032").Result.Students.Count);

        }

        [Fact]
        public void Update_in_DB()
        {
            var GroupTest = new TestHelper();
            var Repository = GroupTest.GroupRepository;
            var ExistingGroup = Repository.GetByNameAsync("IST-021").Result;
            Repository.ChangeTrackerClear();

            ExistingGroup.Name = "Changed";
            ExistingGroup.Students.Add(new Student { Name = "NewStudent", Surname = "Classified"});
            Repository.UpdateAsync(ExistingGroup).Wait();



            Assert.Equal("Changed", Repository.GetByNameAsync("Changed").Result.Name);
            Assert.Equal(3, Repository.GetByNameAsync("Changed").Result.Students.Count);

        }

        [Fact]
        public void DeleteUpdate_in_DB()
        {
            var GroupTest = new TestHelper();
            var Repository = GroupTest.GroupRepository;
            var Group = Repository.GetByNameAsync("IST-021").Result;

            Repository.ChangeTrackerClear();

            Group.Students.RemoveAt(0);
            Repository.UpdateAsync(Group).Wait();

            Assert.Equal(1, Repository.GetByNameAsync("IST-021").Result.Students.Count);
        }
    }
}
