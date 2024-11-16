using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Csharp_Cumulative_Assignment_1.Models;
using System;
using MySql.Data.MySqlClient;



namespace Csharp_Cumulative_Assignment_1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CoursesAPIController : ControllerBase
    {

        // This is dependancy injection
        private readonly SchoolDbContext _schoolcontext;
        public CoursesAPIController(SchoolDbContext schoolcontext)
        {
            _schoolcontext = schoolcontext;
        }



        /// <summary>
        /// When we click on Teachers in Navigation bar on Home page, We are directed to a webpage that lists all teachers in the database school
        /// </summary>
        /// <example>
        /// GET api/Teacher/ListTeachers -> [{"TeacherFname":"Manik", "TeacherLName":"Bansal"},{"TeacherFname":"Asha", "TeacherLName":"Bansal"},.............]
        /// GET api/Teacher/ListTeachers -> [{"TeacherFname":"Apurva", "TeacherLName":"Gupta"},{"TeacherFname":"Himani", "TeacherLName":"Garg"},.............]
        /// </example>
        /// <returns>
        /// A list all the teachers in the database school
        /// </returns>


        [HttpGet]
        [Route(template: "ListCourses")]
        public List<Course> ListCourses()
        {
            // Create an empty list of Teachers
            List<Course> Courses = new List<Course>();
            // 'using' keyword is used that will close the connection by itself after executing the code given inside
            using (MySqlConnection Connection = _schoolcontext.AccessDatabase())
            {

                // Opening the connection
                Connection.Open();


                // Establishing a new query for our database 
                MySqlCommand Command = Connection.CreateCommand();


                // Writing the SQL Query we want to give to database to access information
                Command.CommandText = "select * from courses";


                // Storing the Result Set query in a variable
                using (MySqlDataReader ResultSet = Command.ExecuteReader())
                {

                    // While loop is used to loop through each row in the ResultSet 
                    while (ResultSet.Read())
                    {

                        // Accessing the information of Teacher using the Column name as an index
                        int Id = Convert.ToInt32(ResultSet["courseId"]);
                        string CourseCode = ResultSet["coursecode"].ToString();
                        int TeacherId = Convert.ToInt32(ResultSet["teacherId"]);
                        DateTime StartDate = Convert.ToDateTime(ResultSet["startdate"]);
                        DateTime FinishDate = Convert.ToDateTime(ResultSet["finishdate"]);
                        string CourseName = ResultSet["courseName"].ToString();


                        // Assigning short names for properties of the Teacher
                        Course EachCourse = new()
                        {
                            CourseId = Id,
                            CourseCode = CourseCode,
                            TeacherId = TeacherId,
                            StartDate = StartDate,
                            FinishDate = FinishDate,
                            CourseName = CourseName
                        };


                        // Assigning short names for properties of the Teache
                        Courses.Add(EachCourse);

                    }
                }
            }


            //Return the final list of Teachers 
            return Courses;
        }
    }

    public class Course
    {
        public int CourseId { get; internal set; }
        public string? CourseName { get; internal set; }
        public DateTime FinishDate { get; internal set; }
        public DateTime StartDate { get; internal set; }
        public int TeacherId { get; internal set; }
        public string? CourseCode { get; internal set; }
    }
}

