using CRUD.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace CRUD.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private readonly StudentInformationContext dbContext;

        public StudentsController(StudentInformationContext dbContext)
        {
            this.dbContext = dbContext;
        }
        [HttpPost]
        public async Task<IActionResult> SaveCandidate([FromBody] CandidateDto dto)
        {
            if (dto == null)
            {
                return BadRequest("payload null");
            }
            var CourseEntity = new Course
            {
                CourseId = dto.id,
                Name = dto.courses
            };
            var ClassEntity = new CandidateClass
            {
                ClassId = dto.id,
                Name = dto.className
            };
            var CandidateEntity = new Candidate
            {
                CandidateId = dto.id,
                Name = dto.name,
                Class = ClassEntity,
                Courses = new List<Course> { CourseEntity }
            };

            dbContext.Add(CandidateEntity);
            dbContext.SaveChanges();

            var result = await dbContext.Candidates
        .Where(c => c.CandidateId == dto.id)
        .Select(c => new
        {
            id = c.CandidateId,
            name = c.Name,

            className = c.Class.Name,
            courses = c.Courses.Select(course => course.Name
            )
        })
        .SingleOrDefaultAsync();

            if (result == null)
                return NotFound();

            return Ok(result);



        }
        [HttpGet]
        public async Task<IActionResult> GetAllCandidates()
        {
            var candidates = await dbContext.Candidates
         .Select(c => new
         {
             id = c.CandidateId,
             name = c.Name,
             className = c.Class.Name,
             courses = c.Courses.Select(course => course.Name).ToList()
         })
         .ToListAsync();
            return Ok(candidates);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCandidate(int id, [FromBody] CandidateDto dto)
        {
            if (dto == null || id != dto.id)
                return BadRequest("Invalid data");

            var candidate = await dbContext.Candidates
     .Include(c => c.Class)
     .Include(c => c.Courses)
     .FirstOrDefaultAsync(c => c.CandidateId == id);

            if (candidate == null) return NotFound();

            candidate.Name = dto.name;
            candidate.Class.Name = dto.className;

            candidate.Courses.Clear();

            var existingCourse = await dbContext.Courses.FindAsync(dto.id);
            if (existingCourse != null)
            {
                candidate.Courses.Add(existingCourse);
            }
            else
            {
                candidate.Courses.Add(new Course { CourseId = dto.id, Name = dto.courses });
            }

            await dbContext.SaveChangesAsync();
          

            return Ok(candidate);
            


        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCandidate(int id)
        {
            var existing = await dbContext.Candidates.Include(x => x.Class).Include(x => x.Courses).FirstOrDefaultAsync(x => x.CandidateId == id);
            if(existing == null)
            {
               return BadRequest("null");
            }
            dbContext.Candidates.Remove(existing);
            await dbContext.SaveChangesAsync();

            return Ok();

        }
    }
}            



