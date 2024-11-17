using Microsoft.AspNetCore.Mvc;
using SchoolApp.Models;

namespace SchoolApp.Controllers
{
    public class CoursePageController : Controller
    {

        // API is responsible for gathering the information from the Database and MVC is responsible for giving an HTTP response
        // as a web page that shows the information written in the View

        // In practice, both the StudentAPI and StudentPage controllers
        // should rely on a unified "Service", with an explicit interface

        private readonly CourseAPIController _api;

        public CoursePageController(CourseAPIController api)
        {
            _api = api;
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

        public IActionResult ListCourse()
        {
            List<Course> Courses = _api.ListCourses();
            return View(Courses);

        }
        public IActionResult ShowCourse(int id)
        {
            ActionResult<Course> Courseresult = _api.FindCourse(id);
            if (Courseresult.Result is ObjectResult objectResult && objectResult.Value is Course course)
            {
                return View(course); // Pass the teacher object to the view
                // If no teacher was found, return NotFound response
                // If the teacher is found, return the Teacher object
            }
            return View(null);  // Pass the teacher object to the view
                                // If no teacher was found, return NotFound response
        }

    }
}

