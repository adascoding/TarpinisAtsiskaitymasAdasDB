using System.ComponentModel.DataAnnotations;

namespace TarpinisAtsiskaitymasAdasDB.Entities;

public class Lecture
{
    public string LectureName { get; set; }
    public string LectureTime { get; set; }
    public DayOfWeek? LectureDay { get; set; }
    public ICollection<Department> Departments { get; set; } = new List<Department>();
    public ICollection<Student> Students { get; set; } = new List<Student>();
}
