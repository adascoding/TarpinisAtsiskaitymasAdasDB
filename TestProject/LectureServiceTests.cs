using Microsoft.EntityFrameworkCore;
using TarpinisAtsiskaitymasAdasDB.Data;
using TarpinisAtsiskaitymasAdasDB.Entities;
using TarpinisAtsiskaitymasAdasDB.Repositories;
using TarpinisAtsiskaitymasAdasDB.Services;

namespace TestProject;

public class LectureServiceTests
{
    private ApplicationContext _dbContext;
    private LectureRepository _lectureRepository;
    private LectureService _lectureService;

    [SetUp]
    public void Setup()
    {
        _dbContext = new ApplicationContext();
        _lectureRepository = new LectureRepository(_dbContext);
        _lectureService = new LectureService(_lectureRepository);
    }

    [TearDown]
    public void TearDown()
    {
        _dbContext.Dispose();
    }

    [Test]
    public void ValidateLecture_EmptyOrNullName_ReturnsError()
    {
        // Jei paduodami paskaitos pavadinimas tuščias "" arba null, tai gaunama klaida, nes pavadinimas yra privalomas laukas.

        // Arrange
        var newLectureWithEmptyName = new Lecture
        {
            LectureName = "",
            LectureTime = "10:00-11:00"
        };

        var newLectureWithNullName = new Lecture
        {
            LectureName = null,
            LectureTime = "10:00-11:00"
        };

        // Act
        var resultWithEmptyName = _lectureService.ValidateLecture(newLectureWithEmptyName, "CS1234");
        var resultWithNullName = _lectureService.ValidateLecture(newLectureWithNullName, "CS1234");

        // Assert
        Assert.IsFalse(resultWithEmptyName.Success);
        Assert.AreEqual("Lecture name is required.", resultWithEmptyName.ErrorMessage);

        Assert.IsFalse(resultWithNullName.Success);
        Assert.AreEqual("Lecture name is required.", resultWithNullName.ErrorMessage);
    }
    [Test]
    public void ValidateLecture_ShortName_ReturnsError()
    {
        // Jei paduodami paskaitos pavadinimas "Math" (4 simboliai), tai gaunama klaida,
        // nes pavadinimas turi būti ne trumpesnis kaip 5 simboliai.

        // Arrange
        var newLectureWithShortName = new Lecture
        {
            LectureName = "Math", // 4 characters
            LectureTime = "10:00-11:00"
        };

        // Act
        var result = _lectureService.ValidateLecture(newLectureWithShortName, "CS1234");

        // Assert
        Assert.IsFalse(result.Success);
        Assert.AreEqual("Lecture name must be at least 5 characters long.", result.ErrorMessage);
    }
    [Test]
    public void ValidateLecture_InvalidTimeRange_ReturnsError()
    {
        // Jei paduodami paskaitos laikas "25:00-26:30", tai gaunama klaida,
        // nes pradžios ir pabaigos laikas turi būti tarp 00:00 ir 24:00.

        // Arrange
        var newLectureWithInvalidTime = new Lecture
        {
            LectureName = "Algorithms",
            LectureTime = "25:00-26:30" // Invalid time
        };

        // Act
        var result = _lectureService.ValidateLecture(newLectureWithInvalidTime, "CS1234");

        // Assert
        Assert.IsFalse(result.Success);
        Assert.AreEqual("Invalid lecture time format. Expected format is 'HH:mm-HH:mm'.", result.ErrorMessage);
    }
    [Test]
    public void ValidateLecture_EndTimeEarlierThanStartTime_ReturnsError()
    {
        // Jei paduodami paskaitos laikas "14:00-13:00", tai gaunama klaida,
        // nes pabaigos laikas negali būti ankstesnis už pradžios laiką.

        // Arrange
        var newLectureWithInvalidTime = new Lecture
        {
            LectureName = "Algorithms",
            LectureTime = "14:00-13:00" // End time is earlier than start time
        };

        // Act
        var result = _lectureService.ValidateLecture(newLectureWithInvalidTime, "CS1234");

        // Assert
        Assert.IsFalse(result.Success);
        Assert.AreEqual("Lecture times must be valid and end time cannot be earlier than start time.", result.ErrorMessage);
    }

}
