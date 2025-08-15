using CRUD.BLL.Services;
using CRUD.DAL.Dto;
using Microsoft.AspNetCore.Mvc;

namespace CRUD.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private readonly ICandidateService _candidateService;

        public StudentsController(ICandidateService candidateService)
        {
            _candidateService = candidateService;
        }

        [HttpPost]
        public async Task<IActionResult> AddCandidates([FromBody] CandidateResponseDto dto)
        {
            var result = await _candidateService.AddCandidate(dto);
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCandidate(int id, [FromBody] CandidateResponseDto dto)
        {
            var updated = await _candidateService.UpdateCandidate(id, dto);
            if (updated == null) return NotFound("Candidate not found");
            return Ok(updated);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCandidates()
        {
            var list = await _candidateService.GetAllCandidates();
            return Ok(list);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCandidate(int id)
        {
            
                await _candidateService.DeleteCandidate(id);
                return NoContent();
           
        }
        
    }
}
