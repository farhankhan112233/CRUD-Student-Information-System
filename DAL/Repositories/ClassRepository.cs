using CRUD.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace CRUD.DAL.Repositories
{
    public class ClassRepository : IClassRepository
    {
        private readonly StudentInfoContext _context;
        public ClassRepository(StudentInfoContext context)
        {
            _context = context;
        }
        public async Task<ClassTable?> GetByName(string className)
        {
            return await _context.ClassTables.FirstOrDefaultAsync(c => c.ClassName == className);
        }

        public async Task<ClassTable> AddCandidate(ClassTable classEntity)
        {
            await _context.ClassTables.AddAsync(classEntity);
            await _context.SaveChangesAsync();
            return classEntity;
        }

        public async Task<ClassTable?> GetById(int id)
        {
            return await _context.ClassTables.FirstOrDefaultAsync(c => c.ClassId == id);
        }
    }
}
