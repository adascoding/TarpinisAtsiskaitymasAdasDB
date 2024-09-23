using System.Text.RegularExpressions;
using TarpinisAtsiskaitymasAdasDB.Entities;
using TarpinisAtsiskaitymasAdasDB.Repositories;
using TarpinisAtsiskaitymasAdasDB.Repositories.Interfaces;
using TarpinisAtsiskaitymasAdasDB.Services.Interfaces;

namespace TarpinisAtsiskaitymasAdasDB.Services
{
    public class StudentService : IStudentService
    {
        private readonly IStudentRepository _studentRepository;

        public StudentService(IStudentRepository studentRepository)
        {
            _studentRepository = studentRepository;
        }

        public StudentService()
        {
        }

        public Result<Student> GetStudentByStudentNumber(string studentNumber)
        {
            if (string.IsNullOrWhiteSpace(studentNumber))
            {
                return Result<Student>.Fail("Student number cannot be empty or whitespace.");
            }

            if (!int.TryParse(studentNumber, out int parsedStudentNumber))
            {
                return Result<Student>.Fail("Invalid student number format. It must be numeric.");
            }

            var student = _studentRepository.GetStudentByNumber(parsedStudentNumber);
            if (student == null)
            {
                return Result<Student>.Fail($"No student found with the student number: {studentNumber}");
            }

            return Result<Student>.Ok(student);
        }

        public IEnumerable<Student> GetStudents()
        {
            return _studentRepository.GetStudents();
        }

        public Result<IEnumerable<Student>> GetStudentsByDepartmentCode(string departmentCode)
        {
            var students = _studentRepository.GetStudentsByDepartment(departmentCode);
            if (!students.Any())
            {
                return Result<IEnumerable<Student>>.Fail($"No students found for {departmentCode} department.");
            }
            return Result<IEnumerable<Student>>.Ok(students);
        }

        public Result ValidateStudent(Student student)
        {
            if (string.IsNullOrWhiteSpace(student.FirstName))
            {
                return Result.Fail("First name is required.");
            }
            if (student.FirstName.Length < 2)
            {
                return Result.Fail("First name must be at least 2 characters long.");
            }
            if (student.FirstName.Length > 50)
            {
                return Result.Fail("First name cannot exceed 50 characters.");
            }
            if (!Regex.IsMatch(student.FirstName, @"^[a-zA-Z]+$"))
            {
                return Result.Fail("First name must contain only letters.");
            }

            if (string.IsNullOrWhiteSpace(student.LastName))
            {
                return Result.Fail("Last name is required.");
            }
            if (!Regex.IsMatch(student.LastName, @"^[a-zA-Z]+$"))
            {
                return Result.Fail("Last name must contain only letters.");
            }

            var numberValidationResult = ValidateStudentNumber(student.StudentNumber.ToString());
            if (!numberValidationResult.Success)
            {
                return numberValidationResult;
            }

            if (StudentExists(student.StudentNumber))
            {
                return Result.Fail("Student already exists.");
            }

            if (string.IsNullOrWhiteSpace(student.Email))
            {
                return Result.Fail("Email is required.");
            }
            if (!Regex.IsMatch(student.Email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {
                return Result.Fail("Invalid email format.");
            }
            if (_studentRepository.ExistsByEmail(student.Email))
            {
                return Result.Fail("Email must be unique.");
            }

            if (string.IsNullOrWhiteSpace(student.DepartmentCode))
            {
                return Result.Fail("Department is required.");
            }

            return Result.Ok();
        }

        public Result ValidateStudentNumber(string studentNumber)
        {
            var errors = new List<string>();

            if (studentNumber.Length != 8)
            {
                errors.Add("Student number must be exactly 8 characters long.");
            }
            if (!studentNumber.All(char.IsDigit))
            {
                errors.Add("Student number must contain only digits.");
            }

            if (errors.Any())
            {
                return Result.Fail(string.Join(" ", errors));
            }

            return Result.Ok();
        }



        public bool StudentExists(int studentNumber)
        {
            return _studentRepository.Exists(studentNumber);
        }


        public Result CreateStudent(Student student)
        {
            try
            {
                _studentRepository.Add(student);
                _studentRepository.SaveChanges();

                return Result.Ok();
            }
            catch (Exception ex)
            {
                return Result.Fail($"Error creating student: {ex.Message}");
            }
        }

        public Result AssignLectureToStudent(Student student, Lecture lecture)
        {
            try
            {
                if (student.Lectures.Contains(lecture))
                {
                    return Result.Fail("Lecture is already assigned to this student.");
                }

                student.Lectures.Add(lecture);
                _studentRepository.SaveChanges();

                return Result.Ok();
            }
            catch (Exception ex)
            {
                return Result.Fail($"Error assigning lecture to student: {ex.Message}");
            }
        }


    }

}
