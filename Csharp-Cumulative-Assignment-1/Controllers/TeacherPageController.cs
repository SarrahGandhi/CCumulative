using Microsoft.AspNetCore.Mvc;
using Csharp_Cumulative_Assignment_1.Models;

namespace Csharp_Cumulative_Assignment_1.Controllers
{
    public class TeacherPageController : Controller
    {

        // API is responsible for gathering the information from the Database and MVC is responsible for giving an HTTP response
        // as a web page that shows the information written in the View

        // In practice, both the TeacherAPI and TeacherPage controllers
        // should rely on a unified "Service", with an explicit interface

        private readonly TeacherAPIController _api;

        public TeacherPageController(TeacherAPIController api)
        {
            _api = api;
        }


        /// <summary>
        /// When we click on the Teachers button in Navugation Bar, it returns the web page displaying all the teachers in the Database school
        /// </summary>
        /// <returns>
        /// List of all Teachers in the Database school
        /// </returns>
        /// <example>
        /// GET : api/TeacherPage/List  ->  Gives the list of all Teachers in the Database school
        /// </example>

        public IActionResult List(DateTime? startDate, DateTime? endDate)
        {
            List<Teacher> Teachers = _api.ListTeachers();

            if (startDate.HasValue && endDate.HasValue)
            {
                Teachers = Teachers.Where(t => t.TeacherHireDate >= startDate.Value && t.TeacherHireDate <= endDate.Value).ToList();
            }
            // else if (startDate.HasValue)
            // {
            //     Teachers = Teachers.Where(t => t.TeacherHireDate >= startDate.Value).ToList();
            // }
            // else if (endDate.HasValue)
            // {
            //     Teachers = Teachers.Where(t => t.TeacherHireDate <= endDate.Value).ToList();
            // }

            return View(Teachers);


        }



        /// <summary>
        /// When we Select one Teacher from the list, it returns the web page displaying the information of the SelectedTeacher from the database school
        /// </summary>
        /// <returns>
        /// Information of the SelectedTeacher from the database school
        /// </returns>
        /// <example>
        /// GET :api/TeacherPage/Show/{id}  ->  Gives the information of the SelectedTeacher
        /// </example>
        /// 
        public IActionResult Show(int id)
        {
            ActionResult<Teacher> result = _api.FindTeacher(id);
            if (result.Result is ObjectResult objectResult && objectResult.Value is Teacher teacher)
            {
                return View(teacher); // Pass the teacher object to the view
            }

            // Handle the NotFound case by passing null to the view
            return View(null);
        }


    }
}
