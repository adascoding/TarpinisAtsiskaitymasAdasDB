using TarpinisAtsiskaitymasAdasDB.Entities;

namespace TarpinisAtsiskaitymasAdasDB.Services;

public static class CsvDataLoader
{
    public static List<Department> GetDepartments()
    {
        var departments = new List<Department>();

        try
        {
            var lines = File.ReadAllLines("InitialData\\departments.csv");

            foreach (var line in lines.Skip(1))
            {
                var values = line.Split(',');
                if (values.Length >= 2)
                {
                    var department = new Department
                    {
                        DepartmentCode = values[0],
                        DepartmentName = values[1]
                    };
                    departments.Add(department);
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Can't read file, exception message: {ex.Message}");
        }
        return departments;
    }

    public static List<Lecture> GetLectures()
    {
        var lectures = new List<Lecture>();

        try
        {
            var lines = File.ReadAllLines("InitialData\\lectures.csv");

            foreach (var line in lines.Skip(1))
            {
                var values = line.Split(',');
                if (values.Length >= 2)
                {
                    var lecture = new Lecture
                    {
                        LectureName = values[0],
                        LectureTime = values[1]
                    };
                    lectures.Add(lecture);
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Can't read file, exception message: {ex.Message}");
        }

        return lectures;
    }
    public static List<Student> GetStudents()
    {
        var students = new List<Student>();

        try
        {
            var lines = File.ReadAllLines("InitialData\\students.csv");

            foreach (var line in lines.Skip(1))
            {
                var values = line.Split(',');
                if (values.Length >= 5)
                {
                    var student = new Student
                    {
                        FirstName = values[0],
                        LastName = values[1],
                        StudentNumber = int.Parse(values[2]),
                        Email = values[3],
                        DepartmentCode = values[4]
                    };
                    students.Add(student);
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Can't read file, exception message: {ex.Message}");
        }

        return students;
    }


    public static List<Tuple<string, string>> GetDepartmentLectures()
    {
        var departmentLectures = new List<Tuple<string, string>>();

        try
        {
            var lines = File.ReadAllLines("InitialData\\department_lectures.csv");

            foreach (var line in lines.Skip(1))
            {
                var values = line.Split(',');
                if (values.Length >= 2)
                {
                    departmentLectures.Add(new Tuple<string, string>(values[0], values[1]));
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Can't read file, exception message: {ex.Message}");
        }

        return departmentLectures;
    }

    public static List<Tuple<int, string>> GetStudentLectures()
    {
        var studentLectures = new List<Tuple<int, string>>();
        try
        {
            var lines = File.ReadAllLines("InitialData\\student_lectures.csv");

            foreach (var line in lines.Skip(1))
            {
                var values = line.Split(',');

                if (values.Length >= 2)
                {
                    var studentNumber = int.Parse(values[0]);
                    var lectureName = values[1];

                    studentLectures.Add(new Tuple<int, string>(studentNumber, lectureName));
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Can't read file, exception message: {ex.Message}");
        }
        return studentLectures;
    }

}

