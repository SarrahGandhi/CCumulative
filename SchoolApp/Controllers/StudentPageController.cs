using Microsoft.AspNetCore.Mvc;
using SchoolApp.Models;

namespace SchoolApp.Controllers
{
    public class StudentPageController : Controller
    {
        private readonly StudentAPIController _api;
        /// <summary>
        /// Initializes a new instance of the StudentPageController with the provided StudentAPIController.
        /// </summary>
        /// <param name="api">The API controller used for retrieving student data from the database.</param>
        public StudentPageController(StudentAPIController api)
        {
            _api = api;
        }
        /// <summary>
        /// Displays a web page listing all students retrieved from the database.
        /// </summary>
        /// <example>
        /// GET: /StudentPage/ListStudent  
        /// Displays a list of all students in the database.
        /// </example>
        /// <returns>
        /// A view containing the list of all students in the database.
        /// </returns>
        public IActionResult ListStudent()
        {
            List<Student> Students = _api.ListStudents();
            return View(Students);
        }
        /// <summary>
        /// Displays details of a specific student based on the provided student ID.
        /// </summary>
        /// <param name="id">The unique identifier of the student. Example: 1</param>
        /// <example>
        /// GET: /StudentPage/ShowStudent/1  
        /// Displays the details of the student with ID 1.
        /// </example>
        /// <returns>
        /// A view containing the details of the requested student, or a null view if the student is not found.
        /// </returns>
        public IActionResult ShowStudent(int id)
        {
            ActionResult<Student> Studentresult = _api.FindStudent(id);
            if (Studentresult.Result is ObjectResult objectResult && objectResult.Value is Student student)
            {
                return View(student);
            }
            // If no student is found, return a null view
            return View(null);
        }
    }
}
