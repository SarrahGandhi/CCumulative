﻿using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using SchoolApp.Models;

namespace SchoolApp.Controllers
{
    public class TeacherPageController : Controller
    {

        private readonly TeacherAPIController _api;
        private readonly CourseAPIController _courses;
        /// <summary>
        /// Initializes a new instance of the TeacherPageController with the provided API controllers.
        /// </summary>
        /// <param name="api">API controller for teacher operations.</param>
        /// <param name="course">API controller for course operations.</param>

        public TeacherPageController(TeacherAPIController api, CourseAPIController course)
        {
            _api = api;
            _courses = course;
        }

        /// <summary>
        /// Displays a list of teachers, optionally filtered by their hire date range.
        /// </summary>
        /// <param name="startDate">Optional start date for filtering teachers by hire date.</param>
        /// <param name="endDate">Optional end date for filtering teachers by hire date.</param>
        /// <returns>A view displaying the filtered or complete list of teachers.</returns>

        public IActionResult List(DateTime? startDate, DateTime? endDate)
        {
            List<Teacher> Teachers = _api.ListTeachers();
            if (startDate.HasValue && endDate.HasValue)
            {
                Teachers = Teachers.Where(t => t.TeacherHireDate >= startDate.Value && t.TeacherHireDate <= endDate.Value).ToList();
            }
            return View(Teachers);
        }
        /// <summary>
        /// Displays details of a specific teacher along with their assigned course(s).
        /// </summary>
        /// <param name="id">The unique identifier of the teacher.</param>
        /// <returns>A view displaying the teacher's details and their assigned course(s).</returns>


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
        /// <summary>
        /// Displays the form to create a new teacher, with optional error message.
        /// </summary>
        /// <param name="error">Optional error message to display when form submission fails.</param>
        /// <returns>A view displaying the form for creating a new teacher.</returns>
        /// <example>
        /// GET: /TeacherPage/New  
        /// Displays the form to create a new teacher.
        /// </example>
        [HttpGet]
        public IActionResult New(string? error)
        {
            ViewData["Error"] = error;
            return View();
        }
        /// <summary>
        /// Creates a new teacher and redirects to the teacher details page.
        /// </summary>
        /// <param name="NewTeacher">The teacher data to be added.</param>
        /// <returns>A redirect to the details page of the newly created teacher.</returns>
        /// <example>
        /// POST: /TeacherPage/Create  
        /// Submits the teacher creation form and redirects to the teacher details.
        /// </example>
        [HttpPost]
        public IActionResult Create(Teacher NewTeacher)
        {
            ActionResult<string> teacher = _api.AddTeacher(NewTeacher);
            ObjectResult objectResult = teacher.Result as ObjectResult;
            if (objectResult is OkObjectResult)
            {
                var TeacherId = Convert.ToInt32(objectResult.Value);
                return RedirectToAction("Show", new { id = TeacherId });
            }

            return RedirectToAction("New", new { error = objectResult.Value });


        }
        /// <summary>
        /// Displays a confirmation page for deleting a teacher.
        /// </summary>
        /// <param name="id">The ID of the teacher to confirm deletion.</param>
        /// <returns>A view for confirming the deletion of the specified teacher.</returns>
        /// <example>
        /// GET: /TeacherPage/DeleteConfirm/1  
        /// Displays the confirmation page for deleting the teacher with ID 1.
        /// </example>
        [HttpGet]
        public IActionResult DeleteConfirm(int id)
        {
            ActionResult<Teacher> SelectedTeacher = _api.FindTeacher(id);
            if (SelectedTeacher.Result is OkObjectResult objectResult && objectResult.Value is Teacher teacher)
            {
                ViewData["Teacher"] = teacher;
                return View();
            }
            return RedirectToAction("List");



        }
        /// <summary>
        /// Deletes a teacher and redirects to the list of all teachers.
        /// </summary>
        /// <param name="id">The ID of the teacher to delete.</param>
        /// <returns>A redirect to the list of teachers after the deletion.</returns>
        /// <example>
        /// POST: /TeacherPage/Delete/1  
        /// Deletes the teacher with ID 1 and redirects to the teacher list.
        /// </example>
        [HttpPost]
        public IActionResult Delete(int id)
        {
            var teacher = _api.FindTeacher(id);
            if (teacher == null)
            {
                return NotFound(); // Handle the case where the teacher does not exist
            }
            ActionResult<string> TeacherId = _api.DeleteTeacher(id);
            return RedirectToAction("List");
        }
    }
}
