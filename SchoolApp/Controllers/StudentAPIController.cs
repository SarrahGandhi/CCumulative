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

    public class StudentAPIController : ControllerBase
    {

        // This is dependancy injection
        private readonly SchoolDbContext _schoolcontext;
        // <summary>
        /// Initializes a new instance of the StudentAPIController with the provided database context.
        /// </summary>
        /// <param name="schoolcontext">The database context for accessing the school database.</param>
        public StudentAPIController(SchoolDbContext schoolcontext)
        {
            _schoolcontext = schoolcontext;
        }

        /// <summary>
        /// Retrieves a list of all students in the database.
        /// </summary>
        /// <example>
        /// Request: GET api/StudentAPI/ListStudents  
        /// 
        /// Response:
        /// [
        /// ]
        /// </example>
        /// <returns>
        /// A list of all students, including their ID, first name, last name, enrollment date, and student number.
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
        /// <summary>
        /// Finds and retrieves the details of a specific student by their ID.
        /// </summary>
        /// <param name="id">The unique ID of the student to retrieve. Example: 1</param>
        /// <example>
        /// Request: GET api/StudentAPI/FindStudent/1  
        /// 

        /// If the ID does not exist:
        /// Response: 404 Not Found - "Student with ID 1 not found."
        /// </example>
        /// <returns>
        /// A single student's details if found
        /// </returns>

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
        [HttpPost]
        [Route(template: "AddStudent")]
        public ActionResult<Student> AddStudent([FromBody] Student studentData)
        {
            string pattern = @"^N\d{4}$"; // ^ and $ ensure the entire string matches
            Regex regex = new Regex(pattern);
            if (studentData.StudentFName == null || studentData.StudentLName == null || studentData.StudentFName == "" || studentData.StudentLName == "")
            {
                return BadRequest("Please enter a valid name for the student");

            }
            if (studentData.EnrolDate > DateTime.Today.AddDays(1).AddTicks(-1))
            {
                return BadRequest("Please enter a valid enrolment date");

            }
            if (studentData.StudentNumber == null || studentData.StudentNumber == "")
            {
                return BadRequest("Please enter a valid student number");
            }
            if (!regex.IsMatch(studentData.StudentNumber))
            {
                return BadRequest("Invalid Student Number");
            }
            using (MySqlConnection Connection = _schoolcontext.AccessDatabase())
            {
                Connection.Open();
                MySqlCommand getStudentCommand = Connection.CreateCommand();
                getStudentCommand.CommandText = "select * from students where studentnumber=@number";
                getStudentCommand.Parameters.AddWithValue("@number", studentData.StudentNumber);
                var studentCount = 0;
                using (MySqlDataReader reader = getStudentCommand.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        studentCount++;
                    }
                }
                if (studentCount > 0)
                {
                    return BadRequest("Student already exists");
                }
                MySqlCommand addStudentCommand = Connection.CreateCommand();
                addStudentCommand.CommandText = "insert into students (studentfname,studentlname,studentnumber,enroldate) values (@fname,@lname,@number,@date)";
                addStudentCommand.Parameters.AddWithValue("@fname", studentData.StudentFName);
                addStudentCommand.Parameters.AddWithValue("@lname", studentData.StudentLName);
                addStudentCommand.Parameters.AddWithValue("@number", studentData.StudentNumber);
                addStudentCommand.Parameters.AddWithValue("@date", studentData.EnrolDate);
                addStudentCommand.ExecuteNonQuery();
                return Ok($"{addStudentCommand.LastInsertedId}");
            }
        }
        [HttpDelete]
        [Route(template: "DeleteStudent/{id}")]
        public ActionResult<Student> DeleteStudent(int id)
        {
            using (MySqlConnection Connection = _schoolcontext.AccessDatabase())
            {
                Connection.Open();
                MySqlCommand deleteStudentCommand = Connection.CreateCommand();
                deleteStudentCommand.CommandText = "delete from students where studentid=@id";
                deleteStudentCommand.Parameters.AddWithValue("@id", id);
                var studentCount = deleteStudentCommand.ExecuteNonQuery();
                if (studentCount == 0)
                {
                    return NotFound($"Student with ID {id} not found.");
                }
                return Ok($"Student with ID {id} deleted.");
            }
        }


    }
}




