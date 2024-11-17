using Microsoft.AspNetCore.Mvc;
using SchoolApp.Models;

namespace SchoolApp.Controllers
{
    public class StudentPageController : Controller
    {

        // API is responsible for gathering the information from the Database and MVC is responsible for giving an HTTP response
        // as a web page that shows the information written in the View

        // In practice, both the StudentAPI and StudentPage controllers
        // should rely on a unified "Service", with an explicit interface

        private readonly StudentAPIController _api;

        public StudentPageController(StudentAPIController api)
        {
            _api = api;
        }

        public IActionResult ListStudent()
        {
            List<Student> Students = _api.ListStudents();



            return View(Students);


        }
        /// <summary>
        /// When we click on the Students button in Navugation Bar, it returns the web page displaying all the teachers in the Database student
        /// </summary>
        /// <returns>
        /// List of all Students in the Database student
        /// </returns>
        /// <example>
        /// GET : api/StudentPage/List  ->  Gives the list of all Students in the Database student
        /// </example>

        public IActionResult ShowStudent(int id)
        {
            ActionResult<Student> Studentresult = _api.FindStudent(id);
            if (Studentresult.Result is ObjectResult objectResult && objectResult.Value is Student student)
            {
                return View(student); // Pass the teacher object to the view
            }
            return View(null);  // Pass the teacher object to the view
                                // If no teacher was found, return NotFound response

            // If the teacher is found, return the Teacher object


        }
    }
}
