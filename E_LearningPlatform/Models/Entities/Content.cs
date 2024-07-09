﻿namespace E_LearningPlatorm.Models.Entities
{
    public class Content
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? VideoUrl { get; set; }
        public int? CourseId { get; set; }
        public Course?Course { get; set; }
    }
}
