namespace SchoolApp.Models
{
    /// <summary>
    /// Created Student Class which is a definition to define the properties of the Student
    /// This definition is used to create objects
    /// The properties of that object is then accessed which are sent to view to display on the webpage
    /// Here, Student Class is created and the definition mentions that it has 6 properties (StudentId, StudentFName,
    /// StudentLName, StudentEnrollment Date, EmployeeNumber, StudentSalary which are accessed in Controller and then return
    /// to View to display that properties information on the web page
    /// </summary>

    public class Student
    {
        // Unique identifier for each Student. It is used as the primary key in a database.
        public int StudentId { get; set; }

        // First name of the Student. It stores the Student's first name as a string.
        public string? StudentFName { get; set; }

        // Last name of the Student. It stores the Student's last name as a string.
        public string? StudentLName { get; set; }

        // The date when the Student was enrolled. It is used to track student start date.
        public DateTime EnrolDate { get; set; }

        // It is a unique student number assigned to each Student. 
        public string? StudentNumber { get; set; }



    }
}
