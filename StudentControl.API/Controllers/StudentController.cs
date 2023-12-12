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
    public class StudentController: ControllerBase
    {
        private readonly Context context;
        private readonly StudentRepository studentRepository;
        private readonly IMapper mapper;

        public StudentController(Context context, IMapper map)
        {
            this.context = context;
            studentRepository = new StudentRepository(context);
            mapper = map;
;
        }
        //AutoMapper
        //DTO C#
        // GET: api/<StudentController>
        [HttpGet("/AllStudents")]
        public async Task<StudentDTO[]> GetStudents()
        {
            return mapper.Map<List<Student>, List<StudentDTO>>(await studentRepository.GetAllAsync()).ToArray();
        }

        // GET api/<StudentController>/5
        [HttpGet("{id}/Get")]
        public async Task<ActionResult<StudentDTO>> Get(Guid id)
        {
            try
            {
                Student student = await studentRepository.GetByIdAsync(id);
                if (student == null)
                {
                    return NotFound();
                }

                return Ok(mapper.Map<StudentDTO>(student));
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        [HttpGet("{id}/OrdersList")]
        public async Task<ActionResult<IEnumerable<OrderDTO>>> GetOrders(Guid id)
        {
            try
            {
                var student = await studentRepository.GetByIdAsync(id);
                if (student == null)
                {
                    return NotFound();
                }

                //return Ok(student.Orders.ToList());
                return Ok(mapper.Map<List<Order>, List<OrderDTO>>(student.Orders.ToList()));
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        // POST api/<StudentController>
        [HttpPost]
        public async Task<ActionResult<StudentDTO>> Post(StudentDTO student)
        {
            
            if (context.Students.Any(stud => stud.Id == student.Id)) return BadRequest();
            await studentRepository.AddAsync(mapper.Map<Student>(student));
            return CreatedAtAction(nameof(Post), new { id = student.Id }, student);
        }

        // PUT api/<StudentController>/5
        [HttpPut("{id}/Put")]
        public async Task<ActionResult<StudentDTO>> Put(Guid id, StudentDTO student)
        {

            if (id != student.Id && !context.Students.Any(stud => stud.Id == student.Id)) return BadRequest();
            await studentRepository.UpdateAsync(mapper.Map<Student>(student));
            return Ok( mapper.Map<StudentDTO>( await studentRepository.GetByIdAsync(id)));
        }

        [HttpPut("{id}/RedactOrders")]
        public async Task<ActionResult> Put(Guid id, List<Tuple<Guid,bool>> OrdersID)
        {
            var student = await studentRepository.GetByIdAsync(id);
            if (student == null) return BadRequest();
            foreach (var OrderID in OrdersID) {

                var Order = await context.Orders.FirstOrDefaultAsync(ord => ord.Id == OrderID.Item1);
                if (Order == null) return BadRequest();
                if (OrderID.Item2) student.AddOrder(Order);
                else student.RemoveOrder(Order);
            }
            await studentRepository.UpdateAsync(student);
            return Ok();
        }


        [HttpPut("{StudentId}/SetGroup")]
        public async Task<ActionResult<StudentDTO>> Put(Guid StudentId, Guid GroupId)
        {
            var student = await studentRepository.GetByIdAsync(StudentId);
            var group = await context.Groups.FirstOrDefaultAsync(gr => gr.Id == GroupId);
            if (student == null || group == null) return BadRequest();

            student.SetGroup(group);

            await studentRepository.UpdateAsync(student);
            return Ok(mapper.Map<StudentDTO>( await studentRepository.GetByIdAsync(StudentId)));
        }

        // DELETE api/<StudentController>/5
        [HttpDelete("{id}/Delete")]
        public async Task<ActionResult> Delete(Guid id)
        {
            if(context.Students.Any(stud => stud.Id == id))
            {
                await studentRepository.DeleteAsync(id);
                return Ok();
            }
            return BadRequest(Delete);
        }
    }
}
