namespace CRUD.DAL.Dto
{
    public class CandidateResponseDto
    {
        public int? id { get; set; }
        public string name { get; set; } = null!;
        public string className { get; set; } = null!;
        public string courses { get; set; } = null!;

    }
}
