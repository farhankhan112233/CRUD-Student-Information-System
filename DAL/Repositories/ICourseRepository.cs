using CRUD.DAL.Models;

namespace CRUD.DAL.Repositories
{
    public interface ICourseRepository
    {
        Task AddCourse(IEnumerable<CourseTable> courses);
        Task RemoveCourse(IEnumerable<CourseTable> courses);
        Task<List<CourseTable>> GetByCandidateId(int candidateId);
    }
}
