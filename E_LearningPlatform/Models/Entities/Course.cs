using E_LearningPlatform.Models.Entities;

namespace E_LearningPlatorm.Models.Entities
{
    public class Course
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public List<Content>? Contents { get; set; }
        public List<Enrollment>?Enrollments { get; set; }
    }
}
