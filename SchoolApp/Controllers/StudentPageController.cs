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
        /// <summary>
        /// Displays the form to create a new student, with optional error message.
        /// </summary>
        /// <param name="error">Optional error message to display when form submission fails.</param>
        /// <example>
        /// GET: /StudentPage/NewStudent  
        /// Displays the form to create a new student.
        /// </example>
        /// <returns>
        /// A view displaying the form for creating a new student.
        /// </returns>
        [HttpGet]
        public IActionResult NewStudent(string? error)
        {
            ViewData["Error"] = error;
            return View();
        }
        /// <summary>
        /// Creates a new student and redirects to the student details page.
        /// </summary>
        /// <param name="NewStudent">The student data to be added.</param>
        /// <example>
        /// POST: /StudentPage/CreateStudent  
        /// Submits the student creation form and redirects to the student details.
        /// </example>
        /// <returns>
        /// A redirect to the details page of the newly created student.
        /// </returns>
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
        /// <summary>
        /// Displays a confirmation page for deleting a student.
        /// </summary>
        /// <param name="id">The ID of the student to confirm deletion.</param>
        /// <example>
        /// GET: /StudentPage/ConfirmDeleteStudent/1  
        /// Displays the confirmation page for deleting the student with ID 1.
        /// </example>
        /// <returns>
        /// A view for confirming the deletion of the specified student.
        /// </returns>
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
        /// <summary>
        /// Deletes a student and redirects to the list of all students.
        /// </summary>
        /// <param name="id">The ID of the student to delete.</param>
        /// <example>
        /// POST: /StudentPage/DeleteStudent/1  
        /// Deletes the student with ID 1 and redirects to the student list.
        /// </example>
        /// <returns>
        /// A redirect to the list of students after the deletion.
        /// </returns>
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

