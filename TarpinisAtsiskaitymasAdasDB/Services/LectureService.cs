using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using TarpinisAtsiskaitymasAdasDB.Entities;
using TarpinisAtsiskaitymasAdasDB.Repositories;
using TarpinisAtsiskaitymasAdasDB.Repositories.Interfaces;
using TarpinisAtsiskaitymasAdasDB.Services.Interfaces;

namespace TarpinisAtsiskaitymasAdasDB.Services;

public class LectureService : ILectureService
{
    private readonly ILectureRepository _lectureRepository;
    public LectureService(ILectureRepository lectureRepository)
    {
        _lectureRepository = lectureRepository;
    }
    public IEnumerable<Lecture> GetLectures()
    {
        return _lectureRepository.GetLectures();
    }
    public Result<IEnumerable<Lecture>> GetLecturesByDepartment(string departmentCode)
    {
        var lectures = _lectureRepository.GetLecturesByDepartment(departmentCode);
        if (lectures == null || !lectures.Any())
        {
            return Result<IEnumerable<Lecture>>.Fail("No lectures found for this department.");
        }
        return Result<IEnumerable<Lecture>>.Ok(lectures);
    }

    public Result<IEnumerable<Lecture>> GetAllLecturesByStudent(int studentNumber)
    {
        var lectures = _lectureRepository.GetAllLecturesByStudent(studentNumber);
        if (lectures == null || !lectures.Any())
        {
            return Result<IEnumerable<Lecture>>.Fail("No lectures found for this student.");
        }
        return Result<IEnumerable<Lecture>>.Ok(lectures);
    }
    public Result ValidateLecture(Lecture lecture, string departmentCode)
    {
        if (string.IsNullOrWhiteSpace(lecture.LectureName))
        {
            return Result.Fail("Lecture name is required.");
        }
        if (lecture.LectureName.Length < 5)
        {
            return Result.Fail("Lecture name must be at least 5 characters long.");
        }

        var times = lecture.LectureTime.Split('-');
        if (times.Length != 2 || !TimeSpan.TryParse(times[0], out var startTime) || !TimeSpan.TryParse(times[1], out var endTime))
        {
            return Result.Fail("Invalid lecture time format. Expected format is 'HH:mm-HH:mm'.");
        }

        if (startTime < TimeSpan.Zero || startTime >= TimeSpan.FromHours(24) || endTime <= startTime || endTime > TimeSpan.FromHours(24))
        {
            return Result.Fail("Lecture times must be valid and end time cannot be earlier than start time.");
        }

        if (lecture.LectureDay.HasValue && (lecture.LectureDay.Value < DayOfWeek.Monday || lecture.LectureDay.Value > DayOfWeek.Friday))
        {
            return Result.Fail("Invalid day of the week. Allowed values are Monday to Friday.");
        }

        var departmentLectures = _lectureRepository.GetLecturesByDepartment(departmentCode);
        foreach (var deptLecture in departmentLectures)
        {
            var deptTimes = deptLecture.LectureTime.Split('-');
            var deptStartTime = TimeSpan.Parse(deptTimes[0]);
            var deptEndTime = TimeSpan.Parse(deptTimes[1]);

            if ((startTime < deptEndTime && startTime >= deptStartTime) || (endTime > deptStartTime && endTime <= deptEndTime))
            {
                return Result.Fail("Lecture times overlap with another lecture in the same department.");
            }
        }

        return Result.Ok();
    }

    public Result CreateLecture(Lecture lecture, Department department)
    {
        try
        {
            lecture.Departments.Add(department);

            _lectureRepository.Add(lecture);
            _lectureRepository.SaveChanges();

            return Result.Ok();
        }
        catch (Exception ex)
        {
            return Result.Fail($"Error creating lecture: {ex.Message}");
        }
    }
    public Result<Lecture> GetLectureByName(string lectureName)
    {
        if (string.IsNullOrWhiteSpace(lectureName))
        {
            return Result<Lecture>.Fail("Lecture name cannot be empty or whitespace.");
        }

        var lecture = _lectureRepository.GetLectureByName(lectureName);
        if (lecture == null)
        {
            return Result<Lecture>.Fail($"No lecture found with the name: {lectureName}");
        }

        return Result<Lecture>.Ok(lecture);
    }



}
