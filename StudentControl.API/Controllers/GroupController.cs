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
    public class GroupController : ControllerBase
    {
        private readonly Context context;
        private readonly GroupRepository GroupRepository;
        private readonly IMapper Mapper;

        public GroupController(Context context, IMapper mapper)
        {
            this.context = context;
            GroupRepository = new GroupRepository(context);
            Mapper = mapper;
        }
        //AutoMapper
        //DTO C#
        // GET: api/<StudentController>
        [HttpGet("/AllGroups")]
        public async Task<GroupDTO[]> GetStudents()
        {
            return Mapper.Map<List<Group>,List<GroupDTO>>( await GroupRepository.GetAllAsync()).ToArray();
        }

        // GET api/<StudentController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<GroupDTO>> Get(Guid id)
        {
            try
            {
                var group = await GroupRepository.GetByIdAsync(id);
                if (group == null)
                {
                    return NotFound();
                }

                return Ok( Mapper.Map<GroupDTO>(group));
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        [HttpGet("{id}/StudentList")]
        public async Task<ActionResult<IEnumerable<StudentDTO>>> GetStudents(Guid id)
        {
            try
            {
                var student = await GroupRepository.GetByIdAsync(id);
                if (student == null)
                {
                    return NotFound();
                }

                return Ok(Mapper.Map<List<Student>, List<StudentDTO>>( student.Students.ToList()));
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        // POST api/<StudentController>
        [HttpPost]
        public async Task<ActionResult<GroupDTO>> Post(GroupDTO group)
        {
            if (context.Students.Any(stud => stud.Id == group.Id)) return BadRequest();
            await GroupRepository.AddAsync(Mapper.Map<Group>(group));
            return CreatedAtAction(nameof(Post), new { id = group.Id }, group);
        }

        // PUT api/<StudentController>/5
        [HttpPut("{id}")]
        public async Task<ActionResult<GroupDTO>> Put(Guid id, GroupDTO group)
        {

            if (id != group.Id && !context.Students.Any(gr => gr.Id == group.Id)) return BadRequest();
            await GroupRepository.UpdateAsync(Mapper.Map<Group>(group));
            return Ok(Mapper.Map<GroupDTO>( await GroupRepository.GetByIdAsync(id)));
        }

        [HttpPut("{id}/RedactStudents")]
        public async Task<ActionResult> Put(Guid id, List<Tuple<Guid, bool>> StudentsID)
        {
            var group = await GroupRepository.GetByIdAsync(id);
            if (group == null) return BadRequest();
            foreach (var StudentID in StudentsID)
            {

                var Student = await context.Students.FirstOrDefaultAsync(ord => ord.Id == StudentID.Item1);
                if (Student == null) return BadRequest();
                if (StudentID.Item2) group.AddStudent(Student);
                else group.RemoveStudent(Student);
            }
            await GroupRepository.UpdateAsync(group);
            return Ok();
        }

        // DELETE api/<StudentController>/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            if (context.Groups.Any(stud => stud.Id == id))
            {
                await GroupRepository.DeleteAsync(id);
                return Ok();
            }
            return BadRequest(Delete);
        }
    }
}
