using TarpinisAtsiskaitymasAdasDB.Entities;
using TarpinisAtsiskaitymasAdasDB.Repositories.Interfaces;
using TarpinisAtsiskaitymasAdasDB.Services.Interfaces;

namespace TarpinisAtsiskaitymasAdasDB.Services;

public class DepartmentService : IDepartmentService
{
    private readonly IDepartmentRepository _departmentRepository;
    public DepartmentService(IDepartmentRepository departmentRepository)
    {
        _departmentRepository = departmentRepository;
    }

    public Result ValidateDepartmentCode(string departmentCode)
    {
        if (string.IsNullOrWhiteSpace(departmentCode))
        {
            return Result.Fail("Department code cannot be empty.");
        }
        if (departmentCode.Length != 6)
        {
            return Result.Fail("Department code must be exactly 6 characters long.");
        }
        if (!departmentCode.All(char.IsLetterOrDigit))
        {
            return Result.Fail("Department code can only contain letters and numbers.");
        }
        return Result.Ok();

    }
    public Result ValidateDepartmentName(string departmentName)
    {
        if (string.IsNullOrWhiteSpace(departmentName))
        {
            return Result.Fail("Department name cannot be empty.");
        }
        if (departmentName.Length < 3)
        {
            return Result.Fail("Department name must be at least 3 characters long.");
        }
        if (!departmentName.All(c => char.IsLetterOrDigit(c)))
        {
            return Result.Fail("Department name can only contain letters, numbers, and spaces.");
        }
        return Result.Ok();
    }
    public Department? GetDepartmentByCode(string departmentCode)
    {
        return _departmentRepository.GetDepartmentByCode(departmentCode);
    }
    public Result CreateDepartment(Department department)
    {
        try
        {
            _departmentRepository.Add(department);
            _departmentRepository.SaveChanges();
            return Result.Ok();
        }
        catch (Exception ex)
        {
            return Result.Fail("Error creating department: " + ex.Message);
        }
    }



    public IEnumerable<Department> GetDepartments()
    {
        return _departmentRepository.GetDepartments();
    }


    public bool DepartmentExists(string departmentCode)
    {
        return _departmentRepository.Exists(departmentCode);
    }







    public Result AssignStudentToDepartment(Student student, Department department)
    {
        if (department.Students.Contains(student))
        {
            return Result.Fail("Student is already assigned to this department.");
        }

        _departmentRepository.AssignStudentToDepartment(student, department);

        _departmentRepository.SaveChanges();

        return Result.Ok();
    }

    public Result AssignLectureToDepartment(Lecture lecture, Department department)
    {
        if (department.Lectures.Contains(lecture))
        {
            return Result.Fail("Lecture is already assigned to this department.");
        }

        _departmentRepository.AssignLectureToDepartment(lecture, department);

        _departmentRepository.SaveChanges();

        return Result.Ok();
    }
    public Result<Department> GetDepartmentByStudent(Student student)
    {
        if (student == null)
        {
            return Result<Department>.Fail("Student cannot be null.");
        }

        var department = _departmentRepository.GetDepartmentByStudent(student);
        if (department == null)
        {
            return Result<Department>.Fail($"No department found for student: {student.StudentNumber}");
        }

        return Result<Department>.Ok(department);
    }
    public Result TransferStudentToDepartment(Student student, Department newDepartment)
    {
        if (student == null || newDepartment == null)
        {
            return Result.Fail("Student or Department cannot be null.");
        }

        var currentDepartment = _departmentRepository.GetDepartmentByStudent(student);
        if (currentDepartment == null)
        {
            return Result.Fail("The student is not currently assigned to any department.");
        }

        currentDepartment.Students.Remove(student);

        newDepartment.Students.Add(student);

        _departmentRepository.SaveChanges();

        return Result.Ok();
    }

}
