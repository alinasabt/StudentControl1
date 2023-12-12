using Microsoft.EntityFrameworkCore;
using StudentControl.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentControl.Infrastructure.Repository
{
    public class OrderRepository
    {
        private readonly Context context;

        public Context UnitOfWork { get { return context; } }

        public OrderRepository(Context context)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));

        }

        public void ChangeTrackerClear()
        {
            context.ChangeTracker.Clear();
        }

        public async Task AddAsync(Order order)
        {
            context.Orders.Add(order);
            await context.SaveChangesAsync();
        }

        public async Task<List<Order>> GetAllAsync()
        {
            return await context.Orders.OrderBy(p => p.Id).ToListAsync();
        }

        public async Task<Order?> GetByIdAsync(Guid id)
        {
            return await context.Orders.Where(p => p.Id == id)
                .Include(p => p.Students)
                .FirstOrDefaultAsync();
        }


        public async Task<Order?> GetByNumberAsync(string number)
        {
            return await context.Orders
                .Where(p => p.Number == number)
                .Include(p => p.Students)
                .FirstOrDefaultAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            Order? group = await context.Orders.FindAsync(id);

            if (group != null)
            {
                context.Remove(group);
                await context.SaveChangesAsync();
            }
        }

        public async Task UpdateAsync(Order ChangedOrder)
        {
            var existOrder = GetByIdAsync(ChangedOrder.Id).Result;
            if (existOrder != null)
            {
                context.Entry(existOrder).CurrentValues.SetValues(ChangedOrder);
                //Удаление студентов что не входят в новый список
                foreach (var existStudent in existOrder.Students.ToList())
                {
                    if (!ChangedOrder.Students.Any(ord => ord.Id == existStudent.Id))
                    {
                        existStudent.RemoveGroup();
                        existOrder.Students.Remove(existStudent);
                    }
                }

                //Добавление новых или изменение существующих студентов
                foreach (var NewStudent in ChangedOrder.Students)
                {
                    var existStudent = existOrder.Students.FirstOrDefault(ord => ord.Id == NewStudent.Id);

                    if (existStudent != null)
                    {
                        context.Entry(existStudent).CurrentValues.SetValues(NewStudent);

                    }
                    else
                    {
                        existOrder.Students.Add(NewStudent);
                    }

                    await context.SaveChangesAsync();

                }
            }
        }
    }
}
