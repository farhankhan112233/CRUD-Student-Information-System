using CRUD.DAL.Models;

namespace CRUD.DAL.Repositories
{
    public interface IClassRepository
    {
        Task<ClassTable?> GetByName(string className);
        Task<ClassTable> AddCandidate(ClassTable classEntity);
        Task<ClassTable?> GetById(int id);
    }
}
