using System.ComponentModel.DataAnnotations;

namespace TarpinisAtsiskaitymasAdasDB.Entities;

public class Student
{
    public string FirstName { get; set; }
    public string LastName { get; set; }

    public int StudentNumber { get; set; }

    public string Email { get; set; }

    public string? DepartmentCode { get; set; }
    public Department? Department { get; set; }
    public ICollection<Lecture> Lectures { get; set; } = new List<Lecture>();
}
