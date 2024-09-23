using Microsoft.EntityFrameworkCore;
using TarpinisAtsiskaitymasAdasDB.Entities;
using TarpinisAtsiskaitymasAdasDB.Services;

public static class ModelSeeder
{
    public static void SeedData(ModelBuilder modelBuilder)
    {
        SeedDepartments(modelBuilder);
        SeedLectures(modelBuilder);
        SeedStudents(modelBuilder);
        SeedDepartmentLectures(modelBuilder);
        SeedStudentLectures(modelBuilder);
    }

    private static void SeedDepartments(ModelBuilder modelBuilder)
    {
        var departments = CsvDataLoader.GetDepartments();
        modelBuilder.Entity<Department>().HasData(departments);
    }

    private static void SeedLectures(ModelBuilder modelBuilder)
    {
        var lectures = CsvDataLoader.GetLectures();
        modelBuilder.Entity<Lecture>().HasData(lectures);
    }

    private static void SeedStudents(ModelBuilder modelBuilder)
    {
        var students = CsvDataLoader.GetStudents();
        modelBuilder.Entity<Student>().HasData(students);
    }

    private static void SeedDepartmentLectures(ModelBuilder modelBuilder)
    {
        var departmentLectures = CsvDataLoader.GetDepartmentLectures();
        foreach (var item in departmentLectures)
        {
            modelBuilder.Entity("DepartmentLectures").HasData(
                new { DepartmentCode = item.Item1, LectureName = item.Item2 }
            );
        }
    }

    private static void SeedStudentLectures(ModelBuilder modelBuilder)
    {
        var studentLectures = CsvDataLoader.GetStudentLectures();
        foreach (var item in studentLectures)
        {
            modelBuilder.Entity("StudentLectures").HasData(
                new { StudentNumber = item.Item1, LectureName = item.Item2 }
            );
        }
    }
}
