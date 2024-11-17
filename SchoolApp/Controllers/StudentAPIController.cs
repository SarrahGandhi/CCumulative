using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SchoolApp.Models;
using System;
using MySql.Data.MySqlClient;



namespace SchoolApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentAPIController : ControllerBase
    {

        // This is dependancy injection
        private readonly SchoolDbContext _schoolcontext;
        public StudentAPIController(SchoolDbContext schoolcontext)
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
        [Route(template: "ListStudents")]
        public List<Student> ListStudents()
        {
            // Create an empty list of Teachers
            List<Student> Students = new List<Student>();

            // 'using' keyword is used that will close the connection by itself after executing the code given inside
            using (MySqlConnection Connection = _schoolcontext.AccessDatabase())
            {

                // Opening the connection
                Connection.Open();


                // Establishing a new query for our database 
                MySqlCommand Command = Connection.CreateCommand();


                // Writing the SQL Query we want to give to database to access information
                Command.CommandText = "select * from students";


                // Storing the Result Set query in a variable
                using (MySqlDataReader ResultSet = Command.ExecuteReader())
                {

                    // While loop is used to loop through each row in the ResultSet 
                    while (ResultSet.Read())
                    {

                        // Accessing the information of Teacher using the Column name as an index
                        int Id = Convert.ToInt32(ResultSet["studentId"]);
                        string FirstName = ResultSet["studentfname"].ToString();
                        string LastName = ResultSet["studentlname"].ToString();
                        string StudentNumber = ResultSet["studentNumber"].ToString();
                        DateTime EnrollmentDate = Convert.ToDateTime(ResultSet["enroldate"]);


                        // Assigning short names for properties of the Teacher
                        Student EachStudent = new Student()
                        {
                            StudentId = Id,
                            StudentFName = FirstName,
                            StudentLName = LastName,
                            EnrolDate = EnrollmentDate,
                            StudentNumber = StudentNumber
                        };


                        // Adding all the values of properties of EachTeacher in Teachers List
                        Students.Add(EachStudent);

                    }
                }
            }


            //Return the final list of Teachers 
            return Students;
        }

        [HttpGet]
        [Route(template: "FindStudent/{id}")]
        public ActionResult<Student> FindStudent(int id)
        {
            // Created an object SelectedStudent using Student definition defined as Class in Models
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
                Command.CommandText = "select * from students where studentid=@id";
                Command.Parameters.AddWithValue("@id", id);

                // Storing the Result Set query in a variable
                using (MySqlDataReader ResultSet = Command.ExecuteReader())
                {
                    // While loop is used to loop through each row in the ResultSet 
                    while (ResultSet.Read())
                    {
                        // Accessing the information of Student using the Column name as an index
                        int Id = Convert.ToInt32(ResultSet["studentId"]);
                        string FirstName = ResultSet["studentfname"].ToString();
                        string LastName = ResultSet["studentlname"].ToString();
                        string StudentNumber = ResultSet["studentNumber"].ToString();
                        DateTime EnrollmentDate = Convert.ToDateTime(ResultSet["enroldate"]);


                        // Accessing the information of the properties of Student and then assigning it to the short names 
                        // created above for all properties of the Student
                        Student SelectedStudent = new Student()
                        {
                            StudentId = Id,
                            StudentFName = FirstName,
                            StudentLName = LastName,
                            EnrolDate = EnrollmentDate,
                            StudentNumber = StudentNumber
                        };
                        return Ok(SelectedStudent);
                    }
                }
            }
            return NotFound($"Student with ID {id} not found.");
        }


    }
}

