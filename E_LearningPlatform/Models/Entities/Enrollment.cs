using E_LearningPlatorm.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Azure.Documents;

namespace E_LearningPlatform.Models.Entities
{
    public class Enrollment
    {
        public int? Id { get; set; }
        public int? UserId { get; set; }
        public int? CourseId { get; set; }
        public DateTime? EnrollmentDate { get; set; }


        public virtual User? User { get; set; }
        public virtual Course? Course { get; set; }
    }
}
