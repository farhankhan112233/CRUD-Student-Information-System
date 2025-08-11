using CRUD.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CRUD.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private readonly StudentInfoContext dbContext;

        public StudentsController(StudentInfoContext dbContext)
        {
            this.dbContext = dbContext;
        }
        [HttpPost]
        public IActionResult AddCandidates([FromBody] CandidateDto dto)
        {
            var classEntity = dbContext.ClassTables.FirstOrDefault(c => c.ClassName == dto.className);
            if (classEntity == null)
            {
                classEntity = new ClassTable { ClassName = dto.className };
                dbContext.ClassTables.Add(classEntity);
                dbContext.SaveChanges();
            }
            var candidate = new CandidateTable
            {
                Name = dto.name,
                ClassId = classEntity.ClassId
            };
            dbContext.CandidateTables.Add(candidate);
            dbContext.SaveChanges();
            foreach (var courseName in dto.courses.Split(',', StringSplitOptions.RemoveEmptyEntries))
            {
                var course = new CourseTable
                {
                    Name = courseName.Trim(),
                    CandidateId = candidate.CandidateId
                };
                dbContext.CourseTables.Add(course);
            }
            dbContext.SaveChanges();
            var result = dbContext.ClassTables
                .Include(x => x.CandidateTables)
                .ThenInclude(x => x.CourseTables)
                .FirstOrDefault(x => x.ClassId == candidate.ClassId);
            var response = result.CandidateTables
                .Select(x => new
                {
                    id = x.CandidateId,
                    name = x.Name,
                    className = result.ClassName,
                    courses = String.Join(",", x.CourseTables.Select(c => c.Name))
                }).FirstOrDefault();

            return Ok(response);
        }
        [HttpPut("{id}")]
        public IActionResult UpdateCandidate(int id, [FromBody] CandidateDto dto)
        {
            using var transaction = dbContext.Database.BeginTransaction();
            try {
                var candidate = dbContext.CandidateTables
                    .Include(x => x.CourseTables)
                    .Include(x => x.Class)
                    .FirstOrDefault(x => x.CandidateId == dto.id);

                if (candidate == null)
                {
                    return NotFound("Candidate not found");
                }

                candidate.Name = dto.name;
                if (candidate.Class != null)
                {
                    candidate.Class.ClassName = dto.className;
                }
                dbContext.CourseTables.RemoveRange(candidate.CourseTables);
                foreach (var courseName in dto.courses.Split(',', StringSplitOptions.RemoveEmptyEntries))
                {
                    dbContext.CourseTables.Add(new CourseTable
                    {
                        Name = courseName.Trim(),
                        CandidateId = candidate.CandidateId
                    });
                }
                dbContext.SaveChanges();
                transaction.Commit();
                var response = new
                {
                    id = candidate.CandidateId,
                    name = candidate.Name,
                    className = candidate.Class?.ClassName,
                    courses = string.Join(",", dbContext.CourseTables
               .Where(c => c.CandidateId == candidate.CandidateId)
               .Select(c => c.Name))
                };
                return Ok(response);
            }catch(Exception ex){
                transaction.Rollback();
                return StatusCode(500, new { message = ex.Message });
            }
        }
        [HttpGet]
        public  IActionResult GetAllCandidates()
        {
            var candidate = dbContext.CandidateTables
                   .Include(x => x.CourseTables)
                   .Include(x => x.Class)
                   .Select(x => new
                   {
                       id = x.CandidateId,
                       className = x.Class.ClassName,
                       name = x.Name,
                       courses = string.Join(",", x.CourseTables.Select(c => c.Name))
                   }).ToList();

            return Ok(candidate);
            
        }
        [HttpDelete("{id}")]
        public IActionResult DeleteCandidate(int id)
        {
            var candidate = dbContext.CandidateTables.FirstOrDefault(x => x.CandidateId == id);
            if(candidate == null)
            {
                return NotFound("Candidate id Missing");
            }
            dbContext.Remove(candidate);
            dbContext.SaveChanges();


            return Ok();
        }
    }   }
           


