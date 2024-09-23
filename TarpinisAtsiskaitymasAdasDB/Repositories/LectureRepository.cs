using TarpinisAtsiskaitymasAdasDB.Data;
using TarpinisAtsiskaitymasAdasDB.Entities;
using TarpinisAtsiskaitymasAdasDB.Repositories.Interfaces;

namespace TarpinisAtsiskaitymasAdasDB.Repositories;

public class LectureRepository : ILectureRepository
{
    private readonly ApplicationContext _context;
    public LectureRepository(ApplicationContext context)
    {
        _context = context;
    }
    public IEnumerable<Lecture> GetLectures()
    {
        return _context.Lectures.ToList();
    }
    public IEnumerable<Lecture> GetLecturesByDepartment(string departmentCode)
    {
        return _context.Lectures
            .Where(l => l.Departments.Any(d => d.DepartmentCode == departmentCode))
            .ToList();
    }
    public IEnumerable<Lecture> GetAllLecturesByStudent(int studentNumber)
    {
        return _context.Students
            .Where(s => s.StudentNumber == studentNumber)
            .SelectMany(s => s.Lectures)
            .ToList();
    }
    public void Add(Lecture lecture)
    {
        _context.Lectures.Add(lecture);
    }

    public void SaveChanges()
    {
        _context.SaveChanges();
    }
    public Lecture? GetLectureByName(string lectureName)
    {
        return _context.Lectures.FirstOrDefault(l => l.LectureName == lectureName);
    }
}
