using Microsoft.EntityFrameworkCore;
using TarpinisAtsiskaitymasAdasDB.Data;
using TarpinisAtsiskaitymasAdasDB.Entities;
using TarpinisAtsiskaitymasAdasDB.Repositories.Interfaces;

namespace TarpinisAtsiskaitymasAdasDB.Repositories;

public class DepartmentRepository : IDepartmentRepository
{
    private readonly ApplicationContext _context;
    public DepartmentRepository(ApplicationContext context)
    {
        _context = context;
    }

    public bool Exists(string departmentCode)
    {
        return _context.Departments
            .Any(d => d.DepartmentCode == departmentCode);
    }
    public IEnumerable<Department> GetDepartments()
    {
        return _context.Departments.ToList();
    }
    public Department? GetDepartmentByCode(string departmentCode)
    {
        return _context.Departments
            .FirstOrDefault(d => d.DepartmentCode == departmentCode);
    }
    public void Add(Department department)
    {
        _context.Departments.Add(department);
    }

    public void SaveChanges()
    {
        _context.SaveChanges();
    }
    public void AssignStudentToDepartment(Student student, Department department)
    {
        department.Students.Add(student);
    }

    public void AssignLectureToDepartment(Lecture lecture, Department department)
    {
        department.Lectures.Add(lecture);
    }
    public Department GetDepartmentByStudent(Student student)
    {
        return _context.Departments
            .Include(d => d.Students)
            .FirstOrDefault(d => d.Students.Any(s => s.StudentNumber == student.StudentNumber));
    }
    public void TransferStudentToDepartment(Student student, Department newDepartment)
    {
        var currentDepartment = GetDepartmentByStudent(student);

        if (currentDepartment != null)
        {
            currentDepartment.Students.Remove(student);
        }

        newDepartment.Students.Add(student);
        _context.SaveChanges();
    }


}
