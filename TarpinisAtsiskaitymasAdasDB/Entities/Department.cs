using System.ComponentModel.DataAnnotations;

namespace TarpinisAtsiskaitymasAdasDB.Entities;

public class Department
{
    public string DepartmentCode { get; set; }
    public string DepartmentName { get; set; }

    public ICollection<Student> Students { get; set; } = new List<Student>();
    public ICollection<Lecture> Lectures { get; set; } = new List<Lecture>();
}
