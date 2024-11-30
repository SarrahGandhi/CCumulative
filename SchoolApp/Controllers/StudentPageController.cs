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
        [HttpGet]
        public IActionResult NewStudent(string? error)
        {
            ViewData["Error"] = error;
            return View();
        }
        [HttpPost]
        public IActionResult CreateStudent(Student NewStudent)
        {
            ActionResult<Student> student = _api.AddStudent(NewStudent);
            ObjectResult result = student.Result as ObjectResult;
            if (result is OkObjectResult)
            {
                var StudentId = Convert.ToInt32(result.Value);
                return RedirectToAction("ShowStudent", new { id = StudentId });
            }
            return RedirectToAction("NewStudent", new { error = result.Value });

        }
        [HttpGet]
        public IActionResult ConfirmDeleteStudent(int id)
        {
            ActionResult<Student> SelectedStudent = _api.FindStudent(id);
            if (SelectedStudent.Result is OkObjectResult objectResult && objectResult.Value is Student student)
            {
                ViewData["Student"] = student;
                return View();

            }
            return RedirectToAction("ListStudent");
        }
        [HttpPost]
        public IActionResult DeleteStudent(int id)
        {
            var student = _api.FindStudent(id);
            if (student == null)
            {
                return NotFound();

            }
            ActionResult<Student> StudentId = _api.DeleteStudent(id);
            return RedirectToAction("ListStudent");
        }
    }
}

