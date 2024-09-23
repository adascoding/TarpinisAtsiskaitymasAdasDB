using TarpinisAtsiskaitymasAdasDB.Entities;

namespace TarpinisAtsiskaitymasAdasDB.Services.Interfaces;

public interface IStudentService
{
    IEnumerable<Student> GetStudents();
    Result<IEnumerable<Student>> GetStudentsByDepartmentCode(string departmentCode);
    Result ValidateStudentNumber(string studentNumber);
    bool StudentExists(int studentNumber);
    Result<Student> GetStudentByStudentNumber(string studentNumber);
    Result ValidateStudent(Student student);
    Result CreateStudent(Student student);
    Result AssignLectureToStudent(Student student, Lecture lecture);
}
