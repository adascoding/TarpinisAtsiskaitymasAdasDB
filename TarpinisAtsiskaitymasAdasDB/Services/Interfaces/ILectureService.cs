using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TarpinisAtsiskaitymasAdasDB.Entities;

namespace TarpinisAtsiskaitymasAdasDB.Services.Interfaces;

public interface ILectureService
{
    IEnumerable<Lecture> GetLectures();
    Result<IEnumerable<Lecture>> GetLecturesByDepartment(string departmentCode);
    Result<IEnumerable<Lecture>> GetAllLecturesByStudent(int studentNumber);
    Result ValidateLecture(Lecture lecture, string departmentCode);
    Result CreateLecture(Lecture lecture, Department department);
    Result<Lecture> GetLectureByName(string lectureName);
}
