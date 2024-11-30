using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SchoolApp.Models;
using System;
using MySql.Data.MySqlClient;
using System.Text.RegularExpressions;



namespace SchoolApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseAPIController : ControllerBase
    {

        // This is dependancy injection
        private readonly SchoolDbContext _schoolcontext;
        public CourseAPIController(SchoolDbContext schoolcontext)
        {
            _schoolcontext = schoolcontext;
        }



        /// <summary>
        /// Retrieves a list of all courses available in the database.
        /// </summary>
        /// <example>
        /// GET api/CourseAPI/ListCourses
        /// </example>
        /// <returns>
        /// A list of courses, each with properties like CourseId, CourseCode, TeacherId, StartDate, FinishDate, and CourseName.
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
                        Course EachCourse = new Course()
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

        /// <summary>
        /// Finds a specific course by its unique CourseId.
        /// </summary>
        /// <param name="id">The unique ID of the course to find.</param>
        /// <example>
        /// GET api/CourseAPI/FindCourse/1
        /// </example>
        /// <returns>
        /// The details of the course with the specified ID, or a 404 status if not found.
        /// </returns>
        /// 
        [HttpGet]
        [Route(template: "FindCourse/{id}")]
        public ActionResult<Course> FindCourse(int id)
        {
            // Created an object SelectedCourse using Course definition defined as Class in Models
            // Initialize as null
            // 'using' keyword is used that will close the connection by itself after executing the code inside
            using (MySqlConnection Connection = _schoolcontext.AccessDatabase())
            {
                // Opening the Connection
                Connection.Open();
                // Establishing a new query for our database
                MySqlCommand Command = Connection.CreateCommand();
                // @id is replaced with a 'sanitized' (masked) id so that id can be referenced
                // without revealing the actual @id
                Command.CommandText = "select * from courses where courseid=@id";
                Command.Parameters.AddWithValue("@id", id);
                // Storing the Result Set query in a variable
                using (MySqlDataReader ResultSet = Command.ExecuteReader())
                {
                    // While loop is used to loop through each row in the ResultSet
                    while (ResultSet.Read())
                    {
                        // Accessing the information of Course using the Column name as an index
                        int Id = Convert.ToInt32(ResultSet["courseId"]);
                        string CourseCode = ResultSet["coursecode"].ToString();
                        int TeacherId = Convert.ToInt32(ResultSet["teacherId"]);
                        DateTime StartDate = Convert.ToDateTime(ResultSet["startdate"]);
                        DateTime FinishDate = Convert.ToDateTime(ResultSet["finishdate"]);
                        string CourseName = ResultSet["courseName"].ToString();
                        // Accessing the information of the properties of Course and then assigning it to the short names
                        // created above for all properties of the Course
                        Course SelectedCourse = new Course()
                        {
                            CourseId = Id,
                            CourseCode = CourseCode,
                            TeacherId = TeacherId,
                            StartDate = StartDate,
                            FinishDate = FinishDate,
                            CourseName = CourseName
                        };
                        return Ok(SelectedCourse);
                    }
                }
            }
            return NotFound($"Course with ID {id} not found.");
        }

        /// <summary>
        /// Finds a specific course by the TeacherId associated with it.
        /// </summary>
        /// <param name="id">The unique ID of the teacher.</param>
        /// <example>
        /// GET api/CourseAPI/FindCourseByTeacherId/3
        /// </example>
        /// <returns>
        /// The details of the course taught by the specified teacher, or a 404 status if not found.
        /// </returns>

        [HttpGet]
        [Route(template: "FindCourseByTeacherId/{id}")]
        public ActionResult<Course> FindCourseByTeacherId(int id)
        {
            // Created an object SelectedCourse using Course definition defined as Class in Models
            // Initialize as null
            // 'using' keyword is used that will close the connection by itself after executing the code inside
            using (MySqlConnection Connection = _schoolcontext.AccessDatabase())
            {
                // Opening the Connection
                Connection.Open();
                // Establishing a new query for our database
                MySqlCommand Command = Connection.CreateCommand();
                // @id is replaced with a 'sanitized' (masked) id so that id can be referenced
                // without revealing the actual @id
                Command.CommandText = "select * from courses where teacherId=@id";
                Command.Parameters.AddWithValue("@id", id);
                // Storing the Result Set query in a variable
                using (MySqlDataReader ResultSet = Command.ExecuteReader())
                {
                    // While loop is used to loop through each row in the ResultSet
                    while (ResultSet.Read())
                    {
                        // Accessing the information of Course using the Column name as an index
                        int Id = Convert.ToInt32(ResultSet["courseId"]);
                        string CourseCode = ResultSet["coursecode"].ToString();
                        int TeacherId = Convert.ToInt32(ResultSet["teacherId"]);
                        DateTime StartDate = Convert.ToDateTime(ResultSet["startdate"]);
                        DateTime FinishDate = Convert.ToDateTime(ResultSet["finishdate"]);
                        string CourseName = ResultSet["courseName"].ToString();
                        // Accessing the information of the properties of Course and then assigning it to the short names
                        // created above for all properties of the Course
                        Course SelectedCourse = new Course()
                        {
                            CourseId = Id,
                            CourseCode = CourseCode,
                            TeacherId = TeacherId,
                            StartDate = StartDate,
                            FinishDate = FinishDate,
                            CourseName = CourseName
                        };
                        return Ok(SelectedCourse);
                    }
                }
            }
            return NotFound($"Teacher with ID {id} not found.");
        }
        [HttpPost]
        [Route(template: "AddCourse")]
        public ActionResult<string> AddCourse([FromBody] Course courseData)
        {
            string pattern = @"^[A-Za-z]{4}\d{4}$";
            Regex regex = new Regex(pattern);
            if (courseData.CourseCode == null || courseData.CourseCode == "")
            {
                return BadRequest("Course Code is required");
            }
            if (!regex.IsMatch(courseData.CourseCode))
            {
                return BadRequest("Course Code must be in the format of 4letter+4digit");

            }
            if (courseData.CourseName == null || courseData.CourseName == "")
            {
                return BadRequest("Course Name is required");
            }
            using (MySqlConnection Connection = _schoolcontext.AccessDatabase())
            {
                Connection.Open();
                MySqlCommand getCourseCommand = Connection.CreateCommand();
                getCourseCommand.CommandText = "select * from courses where coursecode=@courseCode";
                getCourseCommand.Parameters.AddWithValue("@courseCode", courseData.CourseCode);
                var coursecount = 0;
                using (MySqlDataReader ResultSet = getCourseCommand.ExecuteReader())
                {
                    while (ResultSet.Read())
                    {
                        coursecount++;

                    }
                }
                if (coursecount > 0)
                {
                    return BadRequest("Course with this code already exists");
                }
                MySqlCommand getCourseId = Connection.CreateCommand();
                getCourseId.CommandText = "select * from courses where courseid=@courseId";
                getCourseId.Parameters.AddWithValue("@courseId", courseData.CourseId);
                var courseIdCount = 0;
                using (MySqlDataReader ResultSet = getCourseId.ExecuteReader())
                {
                    while (ResultSet.Read())
                    {
                        courseIdCount++;
                    }

                }
                if (courseIdCount > 0)
                {
                    return BadRequest("Course with this id already exists");

                }


                MySqlCommand getTeacherId = Connection.CreateCommand();
                getTeacherId.CommandText = "select * from courses where teacherid=@teacherId";
                getTeacherId.Parameters.AddWithValue("@teacherId", courseData.TeacherId);
                var teacherIdCount = 0;
                using (MySqlDataReader ResultSet = getTeacherId.ExecuteReader())
                {
                    while (ResultSet.Read())
                    {
                        teacherIdCount++;

                    }
                }
                if (teacherIdCount == 0)
                {
                    return BadRequest("Teacher with this id does not exist");

                }



                MySqlCommand addCourseCommand = Connection.CreateCommand();
                addCourseCommand.CommandText = "insert into courses(courseid,coursecode,teacherId,startdate,finishdate,coursename) values(@courseId,@courseCode,@teacherId,@startDate,@finishDate,@courseName)";
                addCourseCommand.Parameters.AddWithValue("@courseId", courseData.CourseId);
                addCourseCommand.Parameters.AddWithValue("@courseCode", courseData.CourseCode);
                addCourseCommand.Parameters.AddWithValue("@teacherId", courseData.TeacherId);
                addCourseCommand.Parameters.AddWithValue("@startDate", courseData.StartDate);
                addCourseCommand.Parameters.AddWithValue("@finishDate", courseData.FinishDate);
                addCourseCommand.Parameters.AddWithValue("@courseName", courseData.CourseName);
                addCourseCommand.ExecuteNonQuery();
                return Ok($"{courseData.CourseId}");

            }
        }
        [HttpDelete]
        [Route(template: "DeleteCourse/{id}")]
        public ActionResult<string> DeleteCourse(int id)
        {
            using (MySqlConnection Connection = _schoolcontext.AccessDatabase())
            {
                Connection.Open();
                MySqlCommand deleteCourseCommand = Connection.CreateCommand();
                deleteCourseCommand.CommandText = "delete from courses where courseId=@courseId";
                deleteCourseCommand.Parameters.AddWithValue("@courseId", id);
                var rowsAffected = deleteCourseCommand.ExecuteNonQuery();
                if (rowsAffected == 0)
                {
                    return NotFound($"Course with ID {id} not found.");
                }
                return Ok($"Course with ID {id} deleted.");
            }
        }



    }
}






