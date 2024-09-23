using TarpinisAtsiskaitymasAdasDB.Entities;

namespace TarpinisAtsiskaitymasAdasDB.Services.Interfaces;

public interface IDepartmentService
{
    IEnumerable<Department> GetDepartments();
    Result ValidateDepartmentCode(string departmentCode);
    Result ValidateDepartmentName(string departmentName);
    bool DepartmentExists(string departmentCode);
    Department? GetDepartmentByCode(string departmentCode);
    Result CreateDepartment(Department department);
    Result AssignStudentToDepartment(Student student, Department department);
    Result AssignLectureToDepartment(Lecture lecture, Department department);
    Result<Department> GetDepartmentByStudent(Student student);
    Result TransferStudentToDepartment(Student student, Department newDepartment);
}
