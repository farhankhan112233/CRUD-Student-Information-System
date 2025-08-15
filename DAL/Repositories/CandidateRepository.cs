using CRUD.DAL.Models;
using Microsoft.EntityFrameworkCore;


namespace CRUD.DAL.Repositories
{
    public class CandidateRepository : ICandidateRepository
    {
        private readonly StudentInfoContext dbContext;
        public CandidateRepository(StudentInfoContext context)
        {
            dbContext = context;
        }
        public async Task<CandidateTable> Add(CandidateTable candidate)
        {
            await dbContext.CandidateTables.AddAsync(candidate);
            await dbContext.SaveChangesAsync();
            return candidate;
        }

        public async Task<CandidateTable?> GetWithRelatedById(int id)
        {
            return await dbContext.CandidateTables
                .Include(x => x.CourseTables)
                .Include(x => x.Class)
                .FirstOrDefaultAsync(x => x.CandidateId == id);
        }

        public async Task<CandidateTable?> GetById(int id)
        {
            return await dbContext.CandidateTables.FirstOrDefaultAsync(x => x.CandidateId == id);
        }

        public async Task<List<CandidateTable>> GetAllWithRelated()
        {
            return await dbContext.CandidateTables
                .Include(x => x.CourseTables)
                .Include(x => x.Class)
                .ToListAsync();
        }

        public async Task Update(CandidateTable candidate)
        {
            dbContext.CandidateTables.Update(candidate);
            await dbContext.SaveChangesAsync();
        }

        public async Task Delete(CandidateTable candidate)
        {
            dbContext.CandidateTables.Remove(candidate);
            await dbContext.SaveChangesAsync();
        }
    }
}
