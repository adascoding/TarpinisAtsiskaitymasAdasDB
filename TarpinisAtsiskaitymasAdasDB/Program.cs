using TarpinisAtsiskaitymasAdasDB.Data;
using TarpinisAtsiskaitymasAdasDB.Repositories;
using TarpinisAtsiskaitymasAdasDB.Repositories.Interfaces;
using TarpinisAtsiskaitymasAdasDB.Services;
using TarpinisAtsiskaitymasAdasDB.Services.Interfaces;
using TarpinisAtsiskaitymasAdasDB.UI;

namespace TarpinisAtsiskaitymasAdasDB;

public class Program
{
    static void Main(string[] args)
    {
        ApplicationContext context = new ApplicationContext();

        IStudentRepository studentRepository = new StudentRepository(context);
        IDepartmentRepository departmentRepository = new DepartmentRepository(context);
        ILectureRepository lectureRepository = new LectureRepository(context);

        IStudentService studentService = new StudentService(studentRepository);
        IDepartmentService departmentService = new DepartmentService(departmentRepository);
        ILectureService lectureService = new LectureService(lectureRepository);

        App app = new App(studentService, departmentService, lectureService);
        app.Run();
    }
}
