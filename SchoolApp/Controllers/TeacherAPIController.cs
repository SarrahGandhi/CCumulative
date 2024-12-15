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
        /// <summary>
        /// Adds a new teacher to the database.
        /// </summary>
        /// <param name="teacherData">The teacher object containing the data to be added.</param>
        /// <example>
        /// Example usage:
        /// POST: api/TeacherAPI/AddTeacher  
        /// Request body: 
        /// {
        ///   "TeacherFName": "John",
        ///   "TeacherLName": "Doe",
        ///   "EmployeeNumber": "T001",
        ///   "TeacherHireDate": "2024-01-01T00:00:00",
        ///   "TeacherSalary": 50000
        /// }
        /// Response: 
        /// "Teacher added successfully with ID: 1"
        /// </example>
        /// <returns>A string indicating the result of the add operation.</returns>

        [HttpPost]
        [Route(template: "AddTeacher")]
        public ActionResult<string> AddTeacher([FromBody] Teacher teacherData)
        {
            string pattern = @"^T\d{3}$"; // ^ and $ ensure the entire string matches
            Regex regex = new Regex(pattern);
            // error handling for name
            if (teacherData.TeacherFName == null || teacherData.TeacherLName == null || teacherData.TeacherFName == "" || teacherData.TeacherLName == "")
            {
                return BadRequest("Please provide a valid name for the teacher.");
            }
            // error handling for date
            if (teacherData.TeacherHireDate > DateTime.Today.AddDays(1).AddTicks(-1))
            {
                return BadRequest("Future Date not acceptable");
            }
            if (teacherData.EmployeeNumber == null || teacherData.EmployeeNumber == "")
            {
                return BadRequest("Please provide an Employee Number");
            }
            if (!regex.IsMatch(teacherData.EmployeeNumber))
            {
                return BadRequest("Invalid Employee Number");
            }
            if (teacherData.TeacherSalary <= 0)
            {
                return BadRequest("Salary must be greater than 0");

            }

            using (MySqlConnection Connection = _schoolcontext.AccessDatabase())
            {
                Connection.Open();
                //Check if teacher number is already in use
                MySqlCommand getTeacherCommand = Connection.CreateCommand();
                getTeacherCommand.CommandText = "select * from teachers where employeenumber=@employeenumber";
                getTeacherCommand.Parameters.AddWithValue("@employeenumber", teacherData.EmployeeNumber);
                var teacherCount = 0;
                using (MySqlDataReader ResultSet = getTeacherCommand.ExecuteReader())
                {
                    while (ResultSet.Read())
                    {
                        teacherCount++;
                    }
                }

                if (teacherCount > 0)
                {
                    return BadRequest("Employee Number already in use");
                }

                // Insert
                MySqlCommand insertCommand = Connection.CreateCommand();
                insertCommand.CommandText = "insert into teachers(teacherfname, teacherlname, employeenumber, hiredate, salary) values(@teacherfname, @teacherlname, @employeenumber, @hiredate, @salary)";
                insertCommand.Parameters.AddWithValue("@teacherfname", teacherData.TeacherFName);
                insertCommand.Parameters.AddWithValue("@teacherlname", teacherData.TeacherLName);
                insertCommand.Parameters.AddWithValue("@employeenumber", teacherData.EmployeeNumber);
                insertCommand.Parameters.AddWithValue("@hiredate", teacherData.TeacherHireDate);
                insertCommand.Parameters.AddWithValue("@salary", teacherData.TeacherSalary);
                insertCommand.ExecuteNonQuery();
                return Ok($"{insertCommand.LastInsertedId}");

            }

        }
        /// <summary>
        /// Deletes a teacher from the database by their ID.
        /// </summary>
        /// <param name="id">The ID of the teacher to be deleted.</param>
        /// <example>
        /// Example usage:
        /// DELETE: api/TeacherAPI/DeleteTeacher/1
        /// Response: "Teacher with ID 1 deleted."
        /// </example>
        /// <returns>A string indicating the result of the delete operation.</returns>

        [HttpDelete]
        [Route(template: "DeleteTeacher/{id}")]
        public ActionResult<string> DeleteTeacher(int id)
        {
            using (MySqlConnection Connection = _schoolcontext.AccessDatabase())
            {
                Connection.Open();
                MySqlCommand Command = Connection.CreateCommand();
                Command.CommandText = "delete from teachers where teacherid=@id";
                Command.Parameters.AddWithValue("@id", id);
                var rowsAffected = Command.ExecuteNonQuery();
                if (rowsAffected == 0)
                {
                    return NotFound($"Teacher with ID {id} not found.");
                }
                return Ok($"Teacher with ID {id} deleted.");
            }

        }
        /// <summary>
        /// Updates an existing teacher's information in the database.
        /// </summary>
        /// <param name="id">The unique identifier of the teacher to be updated.</param>
        /// <param name="teacherData">A Teacher object containing updated information such as:
        ///     - First Name
        ///     - Last Name
        ///     - Employee Number (must match the T### format)
        ///     - Hire Date (cannot be a future date)
        ///     - Salary (must be greater than 0)
        /// </param>
        /// <returns>
        /// Returns an ActionResult containing:
        ///     - 200 OK if the update is successful.
        ///     - 400 BadRequest for invalid inputs (e.g., invalid name, salary, date, or employee number).
        ///     - 404 NotFound if no teacher with the specified ID exists.
        /// </returns>
        [HttpPut(template: "UpdateTeacher/{id}")]
        public ActionResult<string> UpdateTeacher(int id, [FromBody] Teacher teacherData)
        {
            string pattern = @"^T\d{3}$"; // ^ and $ ensure the entire string matches
            Regex regex = new Regex(pattern);
            // error handling for name
            if (teacherData.TeacherFName == null || teacherData.TeacherLName == null || teacherData.TeacherFName == "" || teacherData.TeacherLName == "")
            {
                return BadRequest("Please provide a valid name for the teacher.");
            }
            // error handling for date
            if (teacherData.TeacherHireDate > DateTime.Today.AddDays(1).AddTicks(-1))
            {
                return BadRequest("Future Date not acceptable");
            }
            if (teacherData.EmployeeNumber == null || teacherData.EmployeeNumber == "")
            {
                return BadRequest("Please provide an Employee Number");
            }
            if (!regex.IsMatch(teacherData.EmployeeNumber))
            {
                return BadRequest("Invalid Employee Number");
            }
            if (teacherData.TeacherSalary <= 0)
            {
                return BadRequest("Salary must be greater than 0");

            }
            using (MySqlConnection Connection = _schoolcontext.AccessDatabase())
            {
                Connection.Open();
                MySqlCommand checkEmployeeNumberCommand = Connection.CreateCommand();
                checkEmployeeNumberCommand.CommandText = "SELECT COUNT(*) FROM teachers WHERE employeenumber = @employeenumber AND teacherid != @id";
                checkEmployeeNumberCommand.Parameters.AddWithValue("@employeenumber", teacherData.EmployeeNumber);
                checkEmployeeNumberCommand.Parameters.AddWithValue("@id", id);

                var existingCount = Convert.ToInt32(checkEmployeeNumberCommand.ExecuteScalar());
                if (existingCount > 0)
                {
                    return BadRequest($"Employee Number '{teacherData.EmployeeNumber}' is already assigned to another teacher.");
                }

                MySqlCommand checkCommand = Connection.CreateCommand();
                checkCommand.CommandText = "select * from teachers where teacherid=@id";
                checkCommand.Parameters.AddWithValue("@id", id);

                MySqlCommand Command = Connection.CreateCommand();
                Command.CommandText = "UPDATE teachers set teacherfname=@teacherfname, teacherlname=@teacherlname, employeenumber=@employeenumber, hiredate=@hiredate, salary=@salary where teacherid=@id";
                Command.Parameters.AddWithValue("@teacherfname", teacherData.TeacherFName);
                Command.Parameters.AddWithValue("@teacherlname", teacherData.TeacherLName);
                Command.Parameters.AddWithValue("@employeenumber", teacherData.EmployeeNumber);
                Command.Parameters.AddWithValue("@hiredate", teacherData.TeacherHireDate);
                Command.Parameters.AddWithValue("@salary", teacherData.TeacherSalary);
                Command.Parameters.AddWithValue("@id", id);
                var rowsAffected = Command.ExecuteNonQuery();
                if (rowsAffected == 0)
                {
                    return NotFound($"Teacher with ID {id} not found.");
                }
                return Ok($"Teacher with ID {id} updated.");
            }
        }




    }
}