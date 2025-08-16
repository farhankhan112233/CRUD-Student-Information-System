using CRUD.DAL.Dto;
using CRUD.DAL.Models;
using CRUD.DAL.Repositories;
using Microsoft.AspNetCore.Http.HttpResults;


namespace CRUD.BLL.Services
{
    public class CandidateService : ICandidateService
    {
        private readonly ICandidateRepository _candidateRepo;
        private readonly IClassRepository _classRepo;
        private readonly ICourseRepository _courseRepo;

        public CandidateService(
            ICandidateRepository candidateRepo,
            IClassRepository classRepo,
            ICourseRepository courseRepo)
        {
            _candidateRepo = candidateRepo;
            _classRepo = classRepo;
            _courseRepo = courseRepo;
        }

        public async Task<CandidateResponseDto> AddCandidate(CandidateResponseDto dto)
        {
            var classEntity = await _classRepo.GetByName(dto.className);
            if (classEntity == null)
            {
                classEntity = new ClassTable { ClassName = dto.className};
                await _classRepo.AddCandidate(classEntity);
            }

            var candidate = new CandidateTable
            {
                Name = dto.name,
                ClassId = classEntity.ClassId
            };

            await _candidateRepo.Add(candidate);

            var courses = dto.courses
                .Split(',')
                .Select(c => new CourseTable { Name = c.Trim(), CandidateId = candidate.CandidateId })
                .ToList();

            if (courses != null && courses.Count > 0)
                await _courseRepo.AddCourse(courses);

            var result = await _candidateRepo.GetWithRelatedById(candidate.CandidateId);
            return Response(result);
        }

        public async Task<CandidateResponseDto?> UpdateCandidate(int id, CandidateResponseDto dto)
        {
            var candidate = await _candidateRepo.GetWithRelatedById(id);
            if (candidate == null)
                return null;

            candidate.Name = dto.name;

            if (!string.Equals(candidate.Class?.ClassName, dto.className))
            {
                var classEntity = await _classRepo.GetByName(dto.className);
                if (classEntity == null)
                {
                    classEntity = new ClassTable { ClassName = dto.className};
                    await _classRepo.AddCandidate(classEntity);
                }
                candidate.ClassId = classEntity.ClassId;
                candidate.Class = classEntity;
            }

            if (candidate.CourseTables != null && candidate.CourseTables.Any())
            {
                await _courseRepo.RemoveCourse(candidate.CourseTables);
            }

            var newCourses = dto.courses?
                .Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Select(c => new CourseTable { Name = c.Trim(), CandidateId = candidate.CandidateId })
                .ToList();

            if (newCourses != null && newCourses.Count > 0)
                await _courseRepo.AddCourse(newCourses);

            await _candidateRepo.Update(candidate);

            var updated = await _candidateRepo.GetWithRelatedById(candidate.CandidateId);
            return Response(updated);
        }

        public async Task<List<CandidateResponseDto>> GetAllCandidates()
        {
            var list = await _candidateRepo.GetAllWithRelated();
            return list.Select(Response).ToList();
        }

        public async Task DeleteCandidate(int id)
        {
            var candidate = await _candidateRepo.GetById(id);
            if (candidate == null) throw new KeyNotFoundException("Candidate not found");

            var courses = await _courseRepo.GetByCandidateId(id);
            if (courses.Any())
                await _courseRepo.RemoveCourse(courses);

            await _candidateRepo.Delete(candidate);
        }

        private CandidateResponseDto Response(CandidateTable? entity)
        {
            if (entity == null) return new CandidateResponseDto();

            return new CandidateResponseDto
            {
                id = entity.CandidateId,
                name = entity.Name,
                className = entity.Class?.ClassName ?? string.Empty,
                courses = string.Join(",", entity.CourseTables.Select(c => c.Name))
            };

        }
        public async Task<CandidateResponseDto?> GetCandidateById(int id)
        {
            var candidate = await _candidateRepo.GetWithRelatedById(id);
            if (candidate == null) 
            {
                throw new KeyNotFoundException($"Candidate with {id} not found ");
            }
            return Response(candidate);
        }
    }
}
