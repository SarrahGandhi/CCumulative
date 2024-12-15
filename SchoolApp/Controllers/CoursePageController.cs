using System.Text.RegularExpressions;
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
        /// <summary>
        /// Displays a page to create a new course with an optional error message.
        /// </summary>
        /// <param name="error">An optional error message to display if the course creation fails.</param>
        /// <returns>
        /// A view displaying the form to create a new course.
        /// </returns>
        [HttpGet]
        public IActionResult NewCourse(string? error)
        {
            ViewData["Error"] = error;
            return View();
        }
        /// <summary>
        /// Creates a new course in the database.
        /// </summary>
        /// <param name="NewCourse">The course object to be added to the database.</param>
        /// <returns>
        /// Redirects to the "ShowCourse" action if the course is created successfully, 
        /// or redirects to the "NewCourse" action with an error message if creation fails.
        /// </returns>
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
        /// <summary>
        /// Displays a page to confirm the deletion of a course.
        /// </summary>
        /// <param name="id">The unique identifier of the course to be deleted.</param>
        /// <returns>
        /// A view displaying the course details to confirm deletion if the course is found, 
        /// or redirects to the course list page if not found.
        /// </returns>
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
        /// <summary>
        /// Deletes a course from the database.
        /// </summary>
        /// <param name="id">The unique identifier of the course to be deleted.</param>
        /// <returns>
        /// Redirects to the "ListCourse" action after deleting the course.
        /// </returns>
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
        /// <summary>
        /// Displays the Edit Course view with the current course details.
        /// </summary>
        /// <param name="id">The unique identifier of the course to be edited.</param>
        /// <param name="error">
        /// An optional error message to display on the view if any issue occurs during the process.
        /// </param>
        /// <returns>
        /// Returns the EditCourse view with the course details populated in ViewData.
        /// If an error occurs, the error message is also passed to the view.
        /// </returns>
        [HttpGet]
        public IActionResult EditCourse(int id, string? error)
        {
            ViewData["Error"] = error;
            ActionResult<Course> course = _api.FindCourse(id);
            if (course.Result is ObjectResult objectResult && objectResult.Value is Course courseResult)
            {
                ViewData["Course"] = courseResult;
            }
            return View();
        }
        /// <summary>
        /// Updates the details of an existing course and redirects to the ShowCourse view if successful.
        /// </summary>
        /// <param name="id">The unique identifier of the course being updated.</param>
        /// <param name="NewCourse">
        /// A Course object containing the updated course details, such as:
        ///     - Course Name
        ///     - Course Code
        ///     - Credits, etc.
        /// </param>
        /// <returns>
        /// Redirects to:
        /// - ShowCourse action if the update is successful.
        /// - EditCourse action with an error message if the update fails.
        /// </returns>
        [HttpPost]
        public IActionResult UpdateCourse(int id, Course NewCourse)
        {
            ActionResult<string> course = _api.UpdateCourse(id, NewCourse);
            ObjectResult objectResult = course.Result as ObjectResult;
            if (objectResult is OkObjectResult)
            {
                var resultString = objectResult.Value?.ToString();
                var match = Regex.Match(resultString, @"\d+");
                if (match.Success)
                {
                    var CourseId = Convert.ToInt32(match.Value);
                    return RedirectToAction("ShowCourse", new { id });
                }
            }
            return RedirectToAction("EditCourse", new { id, error = objectResult.Value });
        }

    }
}

