using Microsoft.EntityFrameworkCore;
using TarpinisAtsiskaitymasAdasDB.Data;
using TarpinisAtsiskaitymasAdasDB.Entities;
using TarpinisAtsiskaitymasAdasDB.Repositories.Interfaces;

namespace TarpinisAtsiskaitymasAdasDB.Repositories;

public class StudentRepository : IStudentRepository
{
    private readonly ApplicationContext _context;
    public StudentRepository(ApplicationContext context)
    {
        _context = context;
    }
    public IEnumerable<Student> GetStudents()
    {
        return _context.Students.ToList();
    }
    public IEnumerable<Student> GetStudentsByDepartment(string departmentCode)
    {
        return _context.Students
            .Where(s => s.DepartmentCode == departmentCode)
            .ToList();
    }
    public bool Exists(int studentNumber)
    {
        return _context.Students.Any(s => s.StudentNumber == studentNumber);
    }
    public Student? GetStudentByNumber(int studentNumber)
    {
        return _context.Students.FirstOrDefault(s => s.StudentNumber == studentNumber);
    }
    public bool ExistsByEmail(string email)
    {
        return _context.Students.Any(s=>s.Email == email);
    }
    public void Add(Student student)
    {
        _context.Students.Add(student);
    }

    public void SaveChanges()
    {
        _context.SaveChanges();
    }

}
