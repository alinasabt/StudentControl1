using StudentControl.Infrastructure;
using StudentControl.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace StudentControl.Infrastructure.Repository
{
    public class StudentRepository
    {
        private readonly Context context;

        public Context UnitOfWork { get { return context; } }

        public StudentRepository(Context context)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));

        }

        public void ChangeTrackerClear()
        {
            context.ChangeTracker.Clear();
        }

        public async Task AddAsync(Student student)
        {
            //await context.AddAsync(student);
            context.Students.Add(student);
            await context.SaveChangesAsync();
        }

        public async Task<List<Student>> GetAllAsync()
        {
            return await context.Students.OrderBy(p => p.Name).ToListAsync();
        }

        public async Task<Student?> GetByIdAsync(Guid id)
        {
            return await context.Students.Where(p => p.Id == id)
                .Include(p => p.Orders)
                .FirstOrDefaultAsync();
        }

        public async Task<Student?> OnlyGetByIdAsync(Guid id)
        {
            return await context.Students.Where(p => p.Id == id)
                .FirstOrDefaultAsync();
        }


        public async Task<Student?> GetByNameAsync(string name)
        {
            return await context.Students
                .Where(p => p.Name == name)
                .Include(p => p.Orders)
                .FirstOrDefaultAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            Student? student = await context.Students.FindAsync(id);

            if (student != null)
            {
                context.Remove(student);
                await context.SaveChangesAsync();
            }
        }

        public async Task UpdateAsync(Student ChangedStudent)
        {
            var existStudent = GetByIdAsync(ChangedStudent.Id).Result;
            if (existStudent != null)
            {
                context.Entry(existStudent).CurrentValues.SetValues(ChangedStudent);
                //Удаление приказов что не входят в новый список
                foreach (var existStudentOrder in existStudent.Orders.ToList())
                {
                    if (!ChangedStudent.Orders.Any(ord => ord.Id == existStudentOrder.Id))
                    {
                        existStudent.RemoveOrder(existStudentOrder);
                    }
                }
                //Добавление новых или изменение существующих приказов
                foreach (var NewOrder in ChangedStudent.Orders)
                {
                    if (!context.Orders.Any(ord => ord.Id == NewOrder.Id)) context.Add(NewOrder);
                    var existOrder = existStudent.Orders.FirstOrDefault(ord => ord.Id == NewOrder.Id);
                    if (existOrder != null)
                    {
                        context.Entry(existOrder).CurrentValues.SetValues(NewOrder);

                    }
                    else
                    {
                        existStudent.AddOrder(NewOrder);
                    }
                    

                }
                await context.SaveChangesAsync();
            }
        }

    }
}
