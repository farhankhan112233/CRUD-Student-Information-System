using CRUD.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace CRUD.DAL.Repositories
{
    public class CourseRepository : ICourseRepository
    {
        private readonly StudentInfoContext _context;
        public CourseRepository(StudentInfoContext context) => _context = context;

        public async Task AddCourse(IEnumerable<CourseTable> courses)
        {
            await _context.CourseTables.AddRangeAsync(courses);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveCourse(IEnumerable<CourseTable> courses)
        {
          _context.CourseTables.RemoveRange(courses);
          await _context.SaveChangesAsync();
        }

        public async Task<List<CourseTable>> GetByCandidateId(int candidateId)
        {
            return await _context.CourseTables.Where(c => c.CandidateId == candidateId).ToListAsync();
        }
    }
}
