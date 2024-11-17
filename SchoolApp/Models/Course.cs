namespace SchoolApp.Models
{

    /// <summary>
    /// Represents a course with its associated properties.
    /// This class serves as a blueprint to create course objects, which can then be used to manage course-related data.
    /// </summary>

    public class Course
    {
        /// <summary>
        /// Unique identifier for each course. Used as the primary key in the database.
        /// </summary>        
        public int CourseId { get; set; }

        /// <summary>
        /// The unique code for the course (e.g., "CS101"). Helps identify the course uniquely.
        /// </summary>        
        public string? CourseCode { get; set; }

        /// <summary>
        /// The ID of the teacher assigned to the course. Links the course to a specific teacher.
        /// </summary>        
        public int TeacherId { get; set; }

        // <summary>
        /// The date when the course starts.
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// The date when the course ends.
        /// </summary>
        public DateTime FinishDate { get; set; }

        /// <summary>
        /// The full name of the course (e.g., "Introduction to Programming").
        /// </summary>
        public string? CourseName { get; set; }

        // It is the salary of the teacher. It is stored as a decimal to accommodate monetary values.

    }
}
