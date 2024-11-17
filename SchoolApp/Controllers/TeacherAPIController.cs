using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SchoolApp.Models;
using System;
using MySql.Data.MySqlClient;



namespace SchoolApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeacherAPIController : ControllerBase
    {

        // This is dependancy injection
        private readonly SchoolDbContext _schoolcontext;
        /// <summary>
        /// Initializes a new instance of the TeacherAPIController with the provided database context.
        /// </summary>
        /// <param name="schoolcontext">The database context for accessing school data.</param>

        public TeacherAPIController(SchoolDbContext schoolcontext)
        {
            _schoolcontext = schoolcontext;
        }


        /// <summary>
        /// Retrieves a list of all teachers in the database.
        /// </summary>
        /// <example>
        /// Example usage:
        /// GET: api/TeacherAPI/ListTeachers  
        /// Response: 
        /// [
        ///   {"TeacherFName":"John", "TeacherLName":"Doe"},
        ///   {"TeacherFName":"Jane", "TeacherLName":"Doe"}
        /// ]
        /// </example>
        /// <returns>A list of all teachers in the database.</returns>


        [HttpGet]
        [Route(template: "ListTeachers")]
        public List<Teacher> ListTeachers()
        {
            // Create an empty list of Teachers
            List<Teacher> Teachers = new List<Teacher>();

            // 'using' keyword is used that will close the connection by itself after executing the code given inside
            using (MySqlConnection Connection = _schoolcontext.AccessDatabase())
            {

                // Opening the connection
                Connection.Open();


                // Establishing a new query for our database 
                MySqlCommand Command = Connection.CreateCommand();


                // Writing the SQL Query we want to give to database to access information
                Command.CommandText = "select * from teachers";


                // Storing the Result Set query in a variable
                using (MySqlDataReader ResultSet = Command.ExecuteReader())
                {

                    // While loop is used to loop through each row in the ResultSet 
                    while (ResultSet.Read())
                    {

                        // Accessing the information of Teacher using the Column name as an index
                        int Id = Convert.ToInt32(ResultSet["teacherid"]);
                        string FirstName = ResultSet["teacherfname"].ToString();
                        string LastName = ResultSet["teacherlname"].ToString();
                        string EmployeeNumber = ResultSet["employeenumber"].ToString();
                        DateTime TeacherHireDate = Convert.ToDateTime(ResultSet["hiredate"]);
                        decimal Salary = Convert.ToDecimal(ResultSet["salary"]);


                        // Assigning short names for properties of the Teacher
                        Teacher EachTeacher = new Teacher()
                        {
                            TeacherId = Id,
                            TeacherFName = FirstName,
                            TeacherLName = LastName,
                            TeacherHireDate = TeacherHireDate,
                            EmployeeNumber = EmployeeNumber,
                            TeacherSalary = Salary
                        };


                        // Adding all the values of properties of EachTeacher in Teachers List
                        Teachers.Add(EachTeacher);

                    }
                }
            }


            //Return the final list of Teachers 
            return Teachers;
        }


        /// <summary>
        /// Retrieves details of a specific teacher by their ID.
        /// </summary>
        /// <param name="id">The unique identifier of the teacher.</param>
        /// <example>
        /// Example usage:
        /// GET: api/TeacherAPI/FindTeacher/3  
        /// Response: 
        /// {
        ///   "TeacherId": 3,
        ///   "TeacherFName": "Sam",
        ///   "TeacherLName": "Cooper",
        ///   "EmployeeNumber": "EMP123",
        ///   "TeacherHireDate": "2023-09-01T00:00:00",
        ///   "TeacherSalary": 60000.00
        /// }
        /// </example>
        /// <returns>
        /// An ActionResult containing the teacher's details if found; otherwise, a NotFound response.
        /// </returns>

        [HttpGet]
        [Route(template: "FindTeacher/{id}")]
        public ActionResult<Teacher> FindTeacher(int id)
        {
            // Created an object SelectedTeacher using Teacher definition defined as Class in Models
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
                Command.CommandText = "select * from teachers where teacherid=@id";
                Command.Parameters.AddWithValue("@id", id);

                // Storing the Result Set query in a variable
                using (MySqlDataReader ResultSet = Command.ExecuteReader())
                {
                    // While loop is used to loop through each row in the ResultSet 
                    while (ResultSet.Read())
                    {
                        // Accessing the information of Teacher using the Column name as an index
                        int Id = Convert.ToInt32(ResultSet["teacherid"]);
                        string FirstName = ResultSet["teacherfname"].ToString();
                        string LastName = ResultSet["teacherlname"].ToString();
                        string EmployeeNumber = ResultSet["employeenumber"].ToString();
                        DateTime TeacherHireDate = Convert.ToDateTime(ResultSet["hiredate"]);
                        decimal Salary = Convert.ToDecimal(ResultSet["salary"]);

                        // Accessing the information of the properties of Teacher and then assigning it to the short names 
                        // created above for all properties of the Teacher
                        Teacher SelectedTeacher = new Teacher()
                        {
                            TeacherId = Id,
                            TeacherFName = FirstName,
                            TeacherLName = LastName,
                            TeacherHireDate = TeacherHireDate,
                            EmployeeNumber = EmployeeNumber,
                            TeacherSalary = Salary
                        };
                        return Ok(SelectedTeacher);
                    }
                }
            }

            // If no teacher was found, return NotFound response

            return NotFound($"Teacher with ID {id} not found.");

            // If the teacher is found, return the Teacher object

        }
    }
}