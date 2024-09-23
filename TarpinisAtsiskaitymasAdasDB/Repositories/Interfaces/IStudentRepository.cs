using Microsoft.EntityFrameworkCore;
using TarpinisAtsiskaitymasAdasDB.Entities;

namespace TarpinisAtsiskaitymasAdasDB.Repositories.Interfaces;

public interface IStudentRepository
{
    IEnumerable<Student> GetStudents();
    IEnumerable<Student> GetStudentsByDepartment(string departmentCode);
    bool Exists(int studentNumber);
    Student? GetStudentByNumber(int studentNumber);
    bool ExistsByEmail(string email);
    public void Add(Student student);
    public void SaveChanges();
}
