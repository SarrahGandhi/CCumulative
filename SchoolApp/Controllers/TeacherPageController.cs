using Microsoft.AspNetCore.Mvc;
using SchoolApp.Models;

namespace SchoolApp.Controllers
{
    public class TeacherPageController : Controller
    {

        private readonly TeacherAPIController _api;
        private readonly CourseAPIController _courses;

        public TeacherPageController(TeacherAPIController api, CourseAPIController course)
        {
            _api = api;
            _courses = course;
        }

        public IActionResult List(DateTime? startDate, DateTime? endDate)
        {
            List<Teacher> Teachers = _api.ListTeachers();

            if (startDate.HasValue && endDate.HasValue)
            {
                Teachers = Teachers.Where(t => t.TeacherHireDate >= startDate.Value && t.TeacherHireDate <= endDate.Value).ToList();
            }

            return View(Teachers);


        }

        public IActionResult Show(int id)
        {
            ActionResult<Teacher> result = _api.FindTeacher(id);

            if (result.Result is ObjectResult objectResult && objectResult.Value is Teacher teacher)
            {
                ViewData["Teacher"] = teacher;

                //i want to access the Findcourse method in the teacherAPIController
                ActionResult<Course> courseResult = _courses.FindCourseByTeacherId(id);
                if (courseResult.Result is ObjectResult objectCourseResult && objectCourseResult.Value is Course course)

                {

                    ViewData["Course"] = course;
                    // ViewData["Error"] = false;
                    return View();
                }
            }
            // ViewData["Error"] = true;
            return View();
        }
    }
}
