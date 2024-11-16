namespace Csharp_Cumulative_Assignment_1.Models
{

    // Created Teacher Class which is a definition to define the properties of the Teacher
    // This definition is used to create objects
    // The properties of that object is then accessed which are sent to view to display on the webpage
    // Here, Teacher Class is created and the definition mentions that it has 6 properties (TeacherId, TeacherFName,
    // TeacherLName, TeacherHireDate, EmployeeNumber, TeacherSalary which are accessed in Controller and then return
    // to View to display that properties information on the web page


    public class Student
    {
        // Unique identifier for each teacher. It is used as the primary key in a database.
        public int studentId { get; set; }

        // First name of the teacher. It stores the teacher's first name as a string.
        public string? studentFName { get; set; }

        // Last name of the teacher. It stores the teacher's last name as a string.
        public string? studentLName { get; set; }

        // The date when the teacher was hired. It is used to track employment start date.
        public DateTime enrolDate { get; set; }

        // It is a unique employee number assigned to each teacher. 
        public string? studentNumber { get; set; }

        internal static void Add(Student eachStudent)
        {
            throw new NotImplementedException();
        }

        // It is the salary of the teacher. It is stored as a decimal to accommodate monetary values.
    }
}