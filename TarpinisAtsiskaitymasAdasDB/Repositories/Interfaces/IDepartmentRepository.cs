using TarpinisAtsiskaitymasAdasDB.Entities;

namespace TarpinisAtsiskaitymasAdasDB.Repositories.Interfaces;

public interface IDepartmentRepository
{
    IEnumerable<Department> GetDepartments();
    bool Exists(string departmentCode);
    Department? GetDepartmentByCode(string departmentCode);
    void Add(Department department);
    void SaveChanges();
    void AssignStudentToDepartment(Student student, Department department);
    void AssignLectureToDepartment(Lecture lecture, Department department);
    Department GetDepartmentByStudent(Student student);
    void TransferStudentToDepartment(Student student, Department newDepartment);
}
