using Microsoft.AspNetCore.Mvc;
using StudentControl.Infrastructure;
using StudentControl.Domain;
using StudentControl.Infrastructure.Repository;
using StudentControl.Domain.Model;
using StudentControl.DTO;
using Microsoft.EntityFrameworkCore;
using AutoMapper;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace StudentControl.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly Context context;
        private readonly OrderRepository OrderRepository;
        private readonly IMapper Mapper;

        public OrderController(Context context, IMapper mapper)
        {
            this.context = context;
            OrderRepository = new OrderRepository(context);
            this.Mapper = mapper;
        }
        //AutoMapper
        //DTO C#
        // GET: api/<StudentController>
        [HttpGet("/AllOrders")]
        public async Task<OrderDTO[]> GetOrders()
        {
            return Mapper.Map<List<Order>, List<OrderDTO>>(await OrderRepository.GetAllAsync()).ToArray();
        }

        // GET api/<StudentController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<OrderDTO>> Get(Guid id)
        {
            try
            {
                var Order = await OrderRepository.GetByIdAsync(id);
                if (Order == null)
                {
                    return NotFound();
                }

                return Ok(Mapper.Map<OrderDTO>(Order));
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        [HttpGet("{id}/StudentsList")]
        public async Task<ActionResult<IEnumerable<StudentDTO>>> GetStudents(Guid id)
        {
            try
            {
                var order = await OrderRepository.GetByIdAsync(id);
                if (order == null)
                {
                    return NotFound();
                }

                return Ok(Mapper.Map<List<Student>, List<StudentDTO>>(order.Students.ToList()));
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        // POST api/<StudentController>
        [HttpPost]
        public async Task<ActionResult<OrderDTO>> Post(OrderDTO order)
        {
            if (context.Orders.Any(ord => ord.Id == order.Id)) return BadRequest();
            await OrderRepository.AddAsync(Mapper.Map<Order>(order));
            return CreatedAtAction(nameof(Post), new { id = order.Id }, order);
        }

        // PUT api/<OrderController>/5
        [HttpPut("{id}")]
        public async Task<ActionResult<OrderDTO>> Put(Guid id, OrderDTO order)
        {

            if (id != order.Id && !context.Students.Any(stud => stud.Id == order.Id)) return BadRequest();
            await OrderRepository.UpdateAsync(Mapper.Map<Order>(order));
            return Ok(Mapper.Map<OrderDTO>( await OrderRepository.GetByIdAsync(id)));
        }

        [HttpPut("{id}/RedactOrders")]
        public async Task<ActionResult> Put(Guid id, List<Tuple<Guid, bool>> StudentsID)
        {
            var order = await OrderRepository.GetByIdAsync(id);
            if (order == null) return BadRequest();
            foreach (var StudentID in StudentsID)
            {

                var Student = await context.Students.FirstOrDefaultAsync(std => std.Id == StudentID.Item1);
                if (Student == null) return BadRequest();
                if (StudentID.Item2) order.AddStudent(Student);
                else order.RemoveStudent(Student);
            }
            await OrderRepository.UpdateAsync(order);
            return Ok();
        }

        // DELETE api/<StudentController>/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            if (context.Orders.Any(stud => stud.Id == id))
            {
                await OrderRepository.DeleteAsync(id);
                return Ok();
            }
            return BadRequest(Delete);
        }
    }
}
