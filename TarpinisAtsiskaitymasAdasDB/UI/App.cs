using TarpinisAtsiskaitymasAdasDB.Entities;
using TarpinisAtsiskaitymasAdasDB.Services.Interfaces;

namespace TarpinisAtsiskaitymasAdasDB.UI;

public class App
{
    private readonly IStudentService _studentService;
    private readonly IDepartmentService _departmentService;
    private readonly ILectureService _lectureService;
    public App(IStudentService studentService, IDepartmentService departmentService, ILectureService lectureService)
    {
        _studentService = studentService;
        _departmentService = departmentService;
        _lectureService = lectureService;
    }


    #region RunMethod
    public void Run()
    {
        bool exit = false;

        while (!exit)
        {
            Console.Clear();
            Console.WriteLine("Cool University Administration System");
            Console.WriteLine("================================");
            Console.WriteLine("1. Manage Departments");
            Console.WriteLine("2. Manage Students");
            Console.WriteLine("3. Manage Lectures");
            Console.WriteLine("0. Exit");
            Console.WriteLine("================================");
            Console.Write("Select an option: ");

            string input = Console.ReadLine();
            switch (input)
            {
                case "1":
                    ManageDepartments();
                    break;
                case "2":
                    ManageStudents();
                    break;
                case "3":
                    ManageLectures();
                    break;
                case "0":
                    exit = true;
                    break;
                default:
                    Console.WriteLine("Invalid selection. Press any key to try again.");
                    Console.ReadKey();
                    break;
            }
        }
        Console.WriteLine("Exiting the system. Goodbye!");

    }
    #endregion

    #region ManageDepartments
    private void ManageDepartments()
    {
        bool back = false;

        while (!back)
        {
            Console.Clear();
            Console.WriteLine("Manage Departments");
            Console.WriteLine("====================");
            Console.WriteLine("1. Create Department");
            Console.WriteLine("2. Assign Student to Department");
            Console.WriteLine("3. Assign Lecture to Department");
            Console.WriteLine("4. View Department's Students");
            Console.WriteLine("5. View Department's Lectures");
            Console.WriteLine("0. Back");
            Console.WriteLine("====================");
            Console.Write("Select an option: ");

            string input = Console.ReadLine();
            switch (input)
            {
                case "1":
                    CreateDepartment();
                    break;
                case "2":
                    AssignStudentToDepartment();
                    break;
                case "3":
                    AssignLectureToDepartment();
                    break;
                case "4":
                    ViewDepartmentStudents();
                    break;
                case "5":
                    ViewDepartmentLectures();
                    break;
                case "0":
                    back = true;
                    break;
                default:
                    Console.WriteLine("Invalid selection. Press any key to try again.");
                    Console.ReadKey();
                    break;
            }
        }
    }

    public void CreateDepartment()
    {
        Console.Clear();

        var departmentsResult = _departmentService.GetDepartments();
        Console.WriteLine("All Departments");
        DisplayDepartments(departmentsResult);

        Console.WriteLine("Create New Department");

        while (true)
        {
            Console.Write("Enter department code (6 letters or digits, or type '0' to cancel): ");
            var departmentCode = Console.ReadLine();

            if (departmentCode == "0")
            {
                return;
            }
            var departmentCodeValidation = _departmentService.ValidateDepartmentCode(departmentCode);
            if (!departmentCodeValidation.Success)
            {
                Console.WriteLine($"Error: {departmentCodeValidation.ErrorMessage}");
                Console.WriteLine("Press any key to retry.");
                Console.ReadKey();
                continue;
            }

            var department = _departmentService.GetDepartmentByCode(departmentCode);
            if (department is not null)
            {
                Console.WriteLine("Error: A department with this code already exists.");
                Console.WriteLine("Press any key to retry.");
                Console.ReadKey();
                continue;
            }

            Console.Write("Enter department name (min 3 symbols): ");
            var departmentName = Console.ReadLine();

            var departmentNameValidation = _departmentService.ValidateDepartmentName(departmentName);
            if (!departmentNameValidation.Success)
            {
                Console.WriteLine($"Error: {departmentNameValidation.ErrorMessage}");
                Console.WriteLine("Press any key to retry.");
                Console.ReadKey();
                continue;
            }

            var newDepartment = new Department
            {
                DepartmentCode = departmentCode,
                DepartmentName = departmentName
            };

            var createResult = _departmentService.CreateDepartment(newDepartment);
            if (createResult.Success)
            {
                Console.WriteLine("Department successfully created.");
                Console.WriteLine("Press any key to continue.");
                Console.ReadKey();
                return;
            }
            else
            {
                Console.WriteLine($"Error: {createResult.ErrorMessage}");
                Console.WriteLine("Press any key to retry.");
                Console.ReadKey();
            }
        }
    }
    public void AssignStudentToDepartment()
    {
        Console.Clear();
        Console.WriteLine("Assign Students to Department");

        var departmentsResult = _departmentService.GetDepartments();
        Console.WriteLine("All Departments");
        DisplayDepartments(departmentsResult);

        while (true)
        {
            Console.Write("Enter department code (6 letters or digits, or type '0' to cancel): ");
            var departmentCode = Console.ReadLine();

            if (departmentCode == "0")
            {
                return;
            }
            var departmentCodeValidation = _departmentService.ValidateDepartmentCode(departmentCode);
            if (!departmentCodeValidation.Success)
            {
                Console.WriteLine($"Error: {departmentCodeValidation.ErrorMessage}");
                Console.WriteLine("Press any key to retry.");
                Console.ReadKey();
                continue;
            }

            var department = _departmentService.GetDepartmentByCode(departmentCode);
            if (department is null)
            {
                Console.WriteLine("Error: A department with this code does not exists.");
                Console.WriteLine("Press any key to retry.");
                Console.ReadKey();
                continue;
            }

            var studentsResult = _studentService.GetStudents();
            Console.WriteLine("All Students");
            DisplayStudents(studentsResult);

            while (true)
            {
                Console.WriteLine("Enter the student number to assign (or type '0' to return):");
                var studentNumber = Console.ReadLine();

                if (studentNumber == "0")
                {
                    return;
                }

                var studentResult = _studentService.GetStudentByStudentNumber(studentNumber);
                if (!studentResult.Success)
                {
                    Console.WriteLine($"Error: {studentResult.ErrorMessage}");
                    Console.WriteLine("Press any key to retry.");
                    Console.ReadKey();
                    continue;
                }

                var assignResult = _departmentService.AssignStudentToDepartment(studentResult.Value, department);
                if (assignResult.Success)
                {
                    Console.WriteLine("Student successfully assigned to the department.");
                    Console.WriteLine("Press any key to continue.");
                    Console.ReadKey();
                    return;
                }
                else
                {
                    Console.WriteLine($"Error: {assignResult.ErrorMessage}");
                    Console.WriteLine("Press any key to retry.");
                    Console.ReadKey();
                }
            }
        }
    }
    public void AssignLectureToDepartment()
    {
        Console.Clear();
        Console.WriteLine("Assign Lectures to Department");

        var departmentsResult = _departmentService.GetDepartments();
        Console.WriteLine("All Departments");
        DisplayDepartments(departmentsResult);

        while (true)
        {
            Console.Write("Enter department code (6 letters or digits, or type '0' to cancel): ");
            var departmentCode = Console.ReadLine();

            if (departmentCode == "0")
            {
                return;
            }

            var departmentCodeValidation = _departmentService.ValidateDepartmentCode(departmentCode);
            if (!departmentCodeValidation.Success)
            {
                Console.WriteLine($"Error: {departmentCodeValidation.ErrorMessage}");
                Console.WriteLine("Press any key to retry.");
                Console.ReadKey();
                continue;
            }

            var department = _departmentService.GetDepartmentByCode(departmentCode);
            if (department is null)
            {
                Console.WriteLine("Error: A department with this code does not exist.");
                Console.WriteLine("Press any key to retry.");
                Console.ReadKey();
                continue;
            }

            var lecturesResult = _lectureService.GetLectures();
            Console.WriteLine("All Lectures");
            DisplayLectures(lecturesResult);

            while (true)
            {
                Console.WriteLine("Enter the lecture name to assign (or type '0' to return): ");
                var lectureName = Console.ReadLine();

                if (lectureName == "0")
                {
                    return;
                }

                var lectureResult = _lectureService.GetLectureByName(lectureName);
                if (!lectureResult.Success)
                {
                    Console.WriteLine($"Error: {lectureResult.ErrorMessage}");
                    Console.WriteLine("Press any key to retry.");
                    Console.ReadKey();
                    continue;
                }

                var assignResult = _departmentService.AssignLectureToDepartment(lectureResult.Value, department);
                if (assignResult.Success)
                {
                    Console.WriteLine("Lecture successfully assigned to the department.");
                    Console.WriteLine("Press any key to continue.");
                    Console.ReadKey();
                    return;
                }
                else
                {
                    Console.WriteLine($"Error: {assignResult.ErrorMessage}");
                    Console.WriteLine("Press any key to retry.");
                    Console.ReadKey();
                }
            }
        }
    }
    private void ViewDepartmentStudents()
    {
        Console.Clear();

        var departmentsResult = _departmentService.GetDepartments();
        Console.WriteLine("All Departments");
        DisplayDepartments(departmentsResult);

        while (true)
        {
            Console.WriteLine("Enter the department code to show students (or type '0' to return):");
            var departmentCode = Console.ReadLine();

            if (departmentCode == "0")
            {
                return;
            }
            var departmentCodeValidation = _departmentService.ValidateDepartmentCode(departmentCode);
            if (!departmentCodeValidation.Success)
            {
                Console.WriteLine($"Error: {departmentCodeValidation.ErrorMessage}");
                Console.WriteLine("Press any key to retry.");
                Console.ReadKey();
                continue;
            }
            var department = _departmentService.GetDepartmentByCode(departmentCode);
            if (department is null)
            {
                Console.WriteLine("Error: A department with this code does not exist.");
                Console.WriteLine("Press any key to retry.");
                Console.ReadKey();
                continue;
            }
            var studentsResult = _studentService.GetStudentsByDepartmentCode(department.DepartmentCode);
            if (studentsResult.Success)
            {
                Console.WriteLine($"Students in department: {studentsResult.Value.First().Department?.DepartmentName}");
                DisplayStudents(studentsResult.Value);
                Console.WriteLine("Press any key to continue.");
                Console.ReadKey();
            }
            else
            {
                Console.WriteLine($"Error: {studentsResult.ErrorMessage}");
                Console.WriteLine("Press any key to continue.");
                Console.ReadKey();
            }
            if (!RetrySearch("Do you want to search again? (Y/N)"))
            {
                return;
            }
        }
    }
    private void ViewDepartmentLectures()
    {
        Console.Clear();

        var departmentsResult = _departmentService.GetDepartments();
        Console.WriteLine("All Departments");
        DisplayDepartments(departmentsResult);

        while (true)
        {
            Console.WriteLine("Enter the department code to show lectures (or type '0' to return):");
            var departmentCode = Console.ReadLine();

            if (departmentCode == "0")
            {
                return;
            }

            var departmentCodeValidation = _departmentService.ValidateDepartmentCode(departmentCode);
            if (!departmentCodeValidation.Success)
            {
                Console.WriteLine($"Error: {departmentCodeValidation.ErrorMessage}");
                Console.WriteLine("Press any key to retry.");
                Console.ReadKey();
                continue;
            }

            var department = _departmentService.GetDepartmentByCode(departmentCode);
            if (department is null)
            {
                Console.WriteLine("Error: A department with this code does not exist.");
                Console.WriteLine("Press any key to retry.");
                Console.ReadKey();
                continue;
            }

            var lecturesResults = _lectureService.GetLecturesByDepartment(department.DepartmentCode);
            if (lecturesResults.Success)
            {
                Console.WriteLine($"Lectures in department: {department.DepartmentName}");
                DisplayLectures(lecturesResults.Value);
                Console.WriteLine("Press any key to continue.");
                Console.ReadKey();
            }
            else
            {
                Console.WriteLine($"Error: {lecturesResults.ErrorMessage}");
                Console.WriteLine("Press any key to retry.");
                Console.ReadKey();
            }

            if (!RetrySearch("Do you want to search again? (Y/N)"))
            {
                return;
            }
        }
    }


    #endregion

    #region ManageStudents
    private void ManageStudents()
    {
        bool back = false;

        while (!back)
        {
            Console.Clear();
            Console.WriteLine("Manage Students");
            Console.WriteLine("====================");
            Console.WriteLine("1. Create Student");
            Console.WriteLine("2. Transfer Student to Another Department");
            Console.WriteLine("3. View All Students");
            Console.WriteLine("4. View Student's Lectures");
            Console.WriteLine("0. Back");
            Console.WriteLine("====================");
            Console.Write("Select an option: ");

            string input = Console.ReadLine();
            switch (input)
            {
                case "1":
                    CreateStudent();
                    break;
                case "2":
                    TransferStudent();
                    break;
                case "3":
                    ViewAllStudents();
                    break;
                case "4":
                    ViewStudentLectures();
                    break;
                case "0":
                    back = true;
                    break;
                default:
                    Console.WriteLine("Invalid selection. Press any key to try again.");
                    Console.ReadKey();
                    break;
            }
        }
    }
    private void CreateStudent()
    {
        Console.Clear();
        Console.WriteLine("Create Student");
        while (true)
        {
            Console.Write("Enter student's first name: ");
            var firstName = Console.ReadLine();

            Console.Write("Enter student's last name: ");
            var lastName = Console.ReadLine();

            Console.Write("Enter student number (8 digits): ");
            var studentNumberInput = Console.ReadLine();
            if (!int.TryParse(studentNumberInput, out int studentNumber) || studentNumberInput.Length != 8)
            {
                Console.WriteLine("Error: Student number must be an 8-digit number.");
                Console.WriteLine("Press any key to retry.");
                Console.ReadKey();
                continue;
            }

            Console.Write("Enter student email: ");
            var email = Console.ReadLine();

            var departments = _departmentService.GetDepartments();
            DisplayDepartments(departments);

            Console.Write("Enter department code (6 letters or digits, or type '0' to cancel): ");
            var departmentCode = Console.ReadLine();

            if (departmentCode == "0")
            {
                return;
            }

            var newStudent = new Student
            {
                FirstName = firstName,
                LastName = lastName,
                StudentNumber = studentNumber,
                Email = email,
                DepartmentCode = departmentCode
            };

            var studentValidation = _studentService.ValidateStudent(newStudent);
            if (!studentValidation.Success)
            {
                Console.WriteLine($"Error: {studentValidation.ErrorMessage}");
                Console.WriteLine("Press any key to retry.");
                Console.ReadKey();
                continue;
            }

            var createStudentResult = _studentService.CreateStudent(newStudent);
            if (!createStudentResult.Success)
            {
                Console.WriteLine($"Error: {createStudentResult.ErrorMessage}");
                Console.ReadKey();
                continue;
            }

            var lectures = _lectureService.GetLecturesByDepartment(departmentCode);
            if (lectures.Success)
            {
                foreach (var lecture in lectures.Value)
                {
                    _studentService.AssignLectureToStudent(newStudent, lecture);
                }
                Console.WriteLine("Lectures successfully assigned to the student.");
            }
            else
            {
                Console.WriteLine($"Error retrieving lectures: {lectures.ErrorMessage}");
            }

            Console.WriteLine("Student successfully created and assigned to the department.");
            Console.ReadKey();
            Console.WriteLine("Press any key to continue.");
            break;
        }
    }

    private void TransferStudent()
    {
        Console.Clear();
        Console.WriteLine("Transfer Student to Another Department");

        while (true)
        {
            var students = _studentService.GetStudents();
            DisplayStudents(students);

            Console.Write("Enter student number (or type '0' to return): ");
            var studentNumber = Console.ReadLine();

            if (studentNumber == "0")
            {
                return;
            }

            var studentResult = _studentService.GetStudentByStudentNumber(studentNumber);
            if (!studentResult.Success)
            {
                Console.WriteLine($"Error: {studentResult.ErrorMessage}");
                Console.WriteLine("Press any key to retry.");
                Console.ReadKey();
                continue;
            }

            var student = studentResult.Value;

            var currentDepartment = _departmentService.GetDepartmentByStudent(student);
            if (currentDepartment == null)
            {
                Console.WriteLine("Error: Could not find the current department for the student.");
                Console.WriteLine("Press any key to retry.");
                Console.ReadKey();
                continue;
            }
            Console.WriteLine($"Current Department: {currentDepartment.Value.DepartmentName} ({currentDepartment.Value.DepartmentCode})");

            var departments = _departmentService.GetDepartments();
            DisplayDepartments(departments);

            Console.Write("Enter the new department code to transfer the student: ");
            var newDepartmentCode = Console.ReadLine();

            var newDepartment = _departmentService.GetDepartmentByCode(newDepartmentCode);
            if (newDepartment == null)
            {
                Console.WriteLine("Error: Invalid new department code.");
                Console.WriteLine("Press any key to retry.");
                Console.ReadKey();
                continue;
            }

            var transferResult = _departmentService.TransferStudentToDepartment(student, newDepartment);
            if (transferResult.Success)
            {
                Console.WriteLine($"Student {student.FirstName} {student.LastName} has been successfully transferred to {newDepartment.DepartmentName}.");
                Console.WriteLine("Press any key to continue.");
                Console.ReadKey();
                return;
            }
            else
            {
                Console.WriteLine($"Error: {transferResult.ErrorMessage}");
            }

            if (!RetrySearch("Do you want to transfer another student? (Y/N)"))
            {
                return;
            }
        }
    }
    private void ViewStudentLectures()
    {
        Console.Clear();
        var studentsResults = _studentService.GetStudents();
        Console.WriteLine("All Students");
        DisplayStudents(studentsResults);

        while (true)
        {
            Console.WriteLine("Enter the student number to show lectures (or type '0' to return):");
            var input = Console.ReadLine();

            if (input == "0")
            {
                return;
            }

            var validationResult = _studentService.ValidateStudentNumber(input);
            if (!validationResult.Success)
            {
                Console.WriteLine($"Error: {validationResult.ErrorMessage}");
                continue;
            }
            if (!int.TryParse(input, out int studentNumber))
            {
                Console.WriteLine("Error: Invalid student number. Please enter a valid number.");
                continue;
            }
            var exists = _studentService.StudentExists(studentNumber);
            if (!exists)
            {
                Console.WriteLine($"Error: Student does not exists.");
                continue;
            }
            var lecturesResults = _lectureService.GetAllLecturesByStudent(studentNumber);
            if (lecturesResults.Success)
            {
                Console.WriteLine($"Lectures for student: {input}");
                DisplayLectures(lecturesResults.Value);
                Console.WriteLine("Press any key to continue.");
                Console.ReadKey();
            }
            else
            {
                Console.WriteLine($"Error: {lecturesResults.ErrorMessage}");
            }
            if (!RetrySearch("Do you want to search again? (Y/N)"))
            {
                return;
            }
        }
    }
    #endregion

    #region ManageLectures
    private void ManageLectures()
    {
        bool back = false;

        while (!back)
        {
            Console.Clear();
            Console.WriteLine("Manage Lectures");
            Console.WriteLine("====================");
            Console.WriteLine("1. Create Lecture");
            Console.WriteLine("2. View All Lectures");
            Console.WriteLine("0. Back");
            Console.WriteLine("====================");
            Console.Write("Select an option: ");

            string input = Console.ReadLine();
            switch (input)
            {
                case "1":
                    CreateLecture();
                    break;
                case "2":
                    ViewAllLectures();
                    break;
                case "0":
                    back = true;
                    break;
                default:
                    Console.WriteLine("Invalid selection. Press any key to try again.");
                    Console.ReadKey();
                    break;
            }
        }
    }
    private void CreateLecture()
    {
        Console.Clear();
        Console.WriteLine("Create New Lecture");

        while (true)
        {
            Console.Write("Enter lecture name: ");
            var lectureName = Console.ReadLine();

            Console.Write("Enter lecture time (HH:mm-HH:mm): ");
            var lectureTime = Console.ReadLine();

            Console.WriteLine("Select the department for the lecture:");
            var departments = _departmentService.GetDepartments();
            DisplayDepartments(departments);

            Console.Write("Enter department code: ");
            var departmentCode = Console.ReadLine();

            var departmentValidation = _departmentService.ValidateDepartmentCode(departmentCode);
            if (!departmentValidation.Success)
            {
                Console.WriteLine($"Error: {departmentValidation.ErrorMessage}");
                Console.WriteLine("Press any key to retry.");
                Console.ReadKey();
                continue;
            }

            var exists = _departmentService.DepartmentExists(departmentCode);
            if (!exists)
            {
                Console.WriteLine("Error: Department does not exist.");
                Console.WriteLine("Press any key to retry.");
                Console.ReadKey();
                continue;
            }

            var department = _departmentService.GetDepartmentByCode(departmentCode);
            if (department == null)
            {
                Console.WriteLine("Error: Invalid department code.");
                Console.WriteLine("Press any key to retry.");
                Console.ReadKey();
                continue;
            }

            Console.Write("Enter day of the week for the lecture (or leave blank for daily): ");
            var dayInput = Console.ReadLine();

            DayOfWeek? lectureDay = null;
            if (!string.IsNullOrWhiteSpace(dayInput))
            {
                if (Enum.TryParse(dayInput, true, out DayOfWeek day) &&
                    day >= DayOfWeek.Monday && day <= DayOfWeek.Friday)
                {
                    lectureDay = day;
                }
                else
                {
                    Console.WriteLine("Error: Invalid day of the week. Must be between Monday and Friday.");
                    Console.WriteLine("Press any key to retry.");
                    Console.ReadKey();
                    continue;
                }
            }

            var lecture = new Lecture
            {
                LectureName = lectureName,
                LectureTime = lectureTime,
                LectureDay = lectureDay
            };

            var validationResult = _lectureService.ValidateLecture(lecture, departmentCode);
            if (!validationResult.Success)
            {
                Console.WriteLine($"Error: {validationResult.ErrorMessage}");
                Console.WriteLine("Press any key to retry.");
                Console.ReadKey();
                continue;
            }

            var result = _lectureService.CreateLecture(lecture, department);
            if (result.Success)
            {
                Console.WriteLine("Lecture successfully added to department.");
                Console.WriteLine("Press any key to continue.");
                Console.ReadKey();
                return;
            }
            else
            {
                Console.WriteLine($"Error: {result.ErrorMessage}");
                Console.WriteLine("Press any key to retry.");
                Console.ReadKey();
            }
        }
    }

    #endregion

    private void ViewAllLectures()
    {
        Console.Clear();
        var lecuturesResult = _lectureService.GetLectures();
        Console.WriteLine("All Lectures");
        DisplayLectures(lecuturesResult);
        Console.ReadKey();
    }
    private void ViewAllStudents()
    {
        Console.Clear();
        var studentsResults = _studentService.GetStudents();
        Console.WriteLine("All Students");
        DisplayStudents(studentsResults);
        Console.ReadKey();
    }
    

    #region DisplayAllMethods
    private void DisplayDepartments(IEnumerable<Department> departments)
    {
        foreach (var department in departments)
        {
            Console.WriteLine($"- Code: {department.DepartmentCode}, Name: {department.DepartmentName}");
        }
        Console.WriteLine("====================");
    }
    private void DisplayStudents(IEnumerable<Student> students)
    {
        foreach (var student in students)
        {
            Console.WriteLine($"{student.FirstName} {student.LastName}, " +
                              $"Student Number: {student.StudentNumber}, " +
                              $"Email: {student.Email}");
        }
        Console.WriteLine("====================");
    }
    private void DisplayLectures(IEnumerable<Lecture> lectures)
    {
        foreach (var lecture in lectures)
        {
            Console.WriteLine($"- {lecture.LectureName} at {lecture.LectureTime}");
        }
        Console.WriteLine("====================");
    }
    private bool RetrySearch(string message)
    {
        Console.WriteLine(message);
        var retry = Console.ReadLine()?.Trim().ToLower();
        return retry == "y";
    }
    #endregion
}
