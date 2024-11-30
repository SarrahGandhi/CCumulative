using Microsoft.AspNetCore.Mvc;
using SchoolApp.Models;

namespace SchoolApp.Controllers
{
    /// <summary>
    /// The CoursePageController is an MVC controller responsible for rendering web pages related to courses.
    /// It interacts with the CourseAPIController to fetch data from the database using API methods and 
    /// presents that data in the form of web pages through views.
    /// </summary>
    public class CoursePageController : Controller
    {

        /// <summary>
        /// Initializes a new instance of the CoursePageController with an injected CourseAPIController.
        /// </summary>
        /// <param name="api">An instance of CourseAPIController for interacting with the database.</param>


        private readonly CourseAPIController _api;

        public CoursePageController(CourseAPIController api)
        {
            _api = api;
        }

        /// <summary>
        /// Displays a webpage with a list of all courses in the database.
        /// </summary>
        /// <example>
        /// Input:
        /// User clicks on the "Courses" button in the navigation bar.
        /// 
        /// Result:
        /// A webpage is rendered displaying the course list:
        /// 
        /// Internally, the method:
        /// - Calls the `ListCourses` method from the CourseAPIController.
        /// - Passes the retrieved list of courses to the associated view.
        /// </example>
        /// <returns>
        /// A view displaying a list of all courses in the database.
        /// </returns>
        public IActionResult ListCourse()
        {
            List<Course> Courses = _api.ListCourses();
            return View(Courses);

        }
        /// <summary>
        /// Displays a webpage showing the details of a specific course based on its ID.
        /// </summary>
        /// <param name="id">The unique identifier of the course. Example: 1</param>
        /// <example>
        /// Input:
        /// User navigates to `api/CoursePage/ShowCourse/1`.
        /// 
        /// Internally, the method:
        /// - Calls the `FindCourse` method from the CourseAPIController with the specified ID.
        /// - If the course is found, its details are passed to the associated view.
        /// - If the course is not found, a webpage is rendered showing no details or an error message.
        /// 
        /// 
        /// If the ID is invalid or not found:
        /// - A webpage is displayed with an appropriate message, e.g., "Course not found."
        /// </example>
        /// <returns>
        /// A view displaying the details of the specified course if found, or an empty view otherwise.
        /// </returns>
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
        [HttpGet]
        public IActionResult NewCourse(string? error)
        {
            ViewData["Error"] = error;
            return View();
        }
        [HttpPost]
        public IActionResult CreateCourse(Course NewCourse)
        {
            ActionResult<string> course = _api.AddCourse(NewCourse);
            ObjectResult result = course.Result as ObjectResult;
            if (result is OkObjectResult)
            {
                var CourseId = Convert.ToInt32(result.Value);
                return RedirectToAction("ShowCourse", new { id = NewCourse.CourseId });
            }
            return RedirectToAction("NewCourse", new { error = result.Value });
        }
        [HttpGet]
        public IActionResult ConfirmDeleteCourse(int id)
        {
            ActionResult<Course> SelectedCourse = _api.FindCourse(id);
            if (SelectedCourse.Result is OkObjectResult objectResult && objectResult.Value is Course course)
            {
                ViewData["Course"] = course;
                return View();
            }
            return RedirectToAction("ListCourse");

        }
        [HttpPost]
        public IActionResult DeleteCourse(int id)
        {
            var course = _api.FindCourse(id);
            if (course == null)
            {
                return NotFound();

            }
            ActionResult<string> CourseId = _api.DeleteCourse(id);
            return RedirectToAction("ListCourse");
        }

    }
}

