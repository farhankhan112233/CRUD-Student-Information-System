using CRUD.DAL.Models;

namespace CRUD.DAL.Repositories
{
    public interface ICandidateRepository
    {
        Task<CandidateTable> Add(CandidateTable candidate);
        Task<CandidateTable?> GetWithRelatedById(int id);
        Task<CandidateTable?> GetById(int id);
        Task<List<CandidateTable>> GetAllWithRelated();
        Task Update(CandidateTable candidate);
        Task Delete(CandidateTable candidate);
    }
}
