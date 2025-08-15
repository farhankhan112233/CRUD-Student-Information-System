using CRUD.DAL.Dto;

namespace CRUD.BLL.Services
{
    public interface ICandidateService
    {
        Task<CandidateResponseDto> AddCandidate(CandidateResponseDto dto);
        Task<CandidateResponseDto?> UpdateCandidate(int id, CandidateResponseDto dto);
        Task<List<CandidateResponseDto>> GetAllCandidates();
        Task DeleteCandidate(int id);
    }
}
