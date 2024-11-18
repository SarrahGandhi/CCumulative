# SchoolApp
Description
SchoolApp is a web-based application designed to manage and display courses within a school system. It provides a simple interface for users to view course details such as course name, code, start date, finish date, and the teacher assigned to each course. The application leverages ASP.NET Core MVC and MySQL for data storage, enabling CRUD operations for courses and teachers.

## Features
Course Management: Displays a list of all courses with details like name, course code, and duration.
Teacher Information: Each course has associated teacher details (name, hire date, etc.).
Course Filtering: Users can filter courses based on start and finish dates.
Responsive Design: Mobile-friendly interface to ensure accessibility across all devices.
CRUD Operations: The backend supports basic CRUD operations for managing courses and teachers.
Technologies Used
ASP.NET Core MVC: For building the web application using the MVC architecture.
MySQL: For storing course and teacher data.
HTML/CSS: For the frontend to display course information.
Bootstrap: For responsive design and improved UI elements.
MySQL Data Provider: To establish database connections and perform operations in the application.

## Requirements
.NET 6.0 or higher
MySQL Database
Visual Studio or any .NET-compatible IDE
Installation
1. Clone the Repository
Clone the project to your local machine using Git:


## Git Repo
git clone https://github.com/SarrahGandhi/SchoolApp.git


### Set Up MySQL Database
Create a MySQL database named school (or use your preferred name).

## Update the connection string in SchoolDbContext.cs with your database credentials:

private static string User { get { return "root"; } } <br>
private static string Password { get { return ""; } } <br>
private static string Database { get { return "school"; } } <br>
private static string Server { get { return "localhost"; } } <br>
private static string Port { get { return "3306"; } } <br>


## Application Structure
Controllers: Contains the logic for handling requests related to courses and teachers (e.g., TeacherPageController and CourseAPIController).<br>
Models: Contains the class definitions for Course, Teacher, and Student, which represent the data objects used in the application. <br>
Views: Displays the UI for listing courses, showing individual course details, and more. <br>
Database Context: SchoolDbContext.cs is responsible for handling MySQL database connections and executing queries. <br>
