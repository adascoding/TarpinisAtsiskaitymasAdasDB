using TarpinisAtsiskaitymasAdasDB.Entities;

namespace TarpinisAtsiskaitymasAdasDB.Repositories.Interfaces;

public interface ILectureRepository
{
    IEnumerable<Lecture> GetLectures();
    IEnumerable<Lecture> GetLecturesByDepartment(string departmentCode);
    IEnumerable<Lecture> GetAllLecturesByStudent(int studentNumber);
    void Add(Lecture lecture);
    void SaveChanges();
    Lecture? GetLectureByName(string lectureName);
}
