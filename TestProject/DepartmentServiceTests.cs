using Microsoft.EntityFrameworkCore;
using TarpinisAtsiskaitymasAdasDB.Data;
using TarpinisAtsiskaitymasAdasDB.Entities;
using TarpinisAtsiskaitymasAdasDB.Repositories;
using TarpinisAtsiskaitymasAdasDB.Services;
using TarpinisAtsiskaitymasAdasDB.Services.Interfaces;

namespace TestProject;

public class DepartmentServiceTests
{
    private ApplicationContext _dbContext;
    private DepartmentRepository _departmentRepository;
    private DepartmentService _departmentService;

    [SetUp]
    public void Setup()
    {
        _dbContext = new ApplicationContext();
        _departmentRepository = new DepartmentRepository(_dbContext);
        _departmentService = new DepartmentService(_departmentRepository);
    }

    [TearDown]
    public void TearDown()
    {
        _dbContext.Dispose();
    }
    [Test]
    public void ValidateDepartmentName_TooShort_ReturnsError()
    {
        // Jei paduodamas departamento pavadinimas "CS" (2 simboliai), tai gaunama klaida, nes pavadinimas turi būti ne trumpesnis kaip 3 simboliai.

        // Arrange
        var departmentName = "CS"; // 2 characters

        // Act
        var result = _departmentService.ValidateDepartmentName(departmentName);

        // Assert
        Assert.IsFalse(result.Success);
        Assert.AreEqual("Department name must be at least 3 characters long.", result.ErrorMessage);
    }
    [Test]
    public void ValidateDepartmentName_WithSpecialCharacters_ReturnsError()
    {
        // Jei paduodami departamento pavadinimas "Computer Science & Engineering" (su specialiaisiais simboliais), tai gaunama klaida, nes pavadinime gali būti tik raidės ir skaičiai.

        // Arrange
        var departmentName = "Computer Science & Engineering"; // Contains special character '&'

        // Act
        var result = _departmentService.ValidateDepartmentName(departmentName);

        // Assert
        Assert.IsFalse(result.Success);
        Assert.AreEqual("Department name can only contain letters, numbers, and spaces.", result.ErrorMessage);
    }

    [Test]
    public void ValidateDepartmentCode_WithShortCode_ReturnsError()
    {
        // Jei paduodami departamento kodas "CS12" (4 simboliai), tai gaunama klaida, nes kodas turi būti tiksliai 6 simbolių ilgio.

        // Arrange
        var departmentCode = "CS12"; // 4 characters long

        // Act
        var result = _departmentService.ValidateDepartmentCode(departmentCode);

        // Assert
        Assert.IsFalse(result.Success);
        Assert.AreEqual("Department code must be exactly 6 characters long.", result.ErrorMessage);
    }

    [Test]
    public void ValidateDepartmentCode_WithSpecialCharacter_ReturnsError()
    {
        // Jei paduodami departamento kodas "CS123@" tai gaunama klaida, nes kode gali būti tik raidės ir skaičiai.

        // Arrange
        var departmentCode = "CS123@"; // Contains special character '@'

        // Act
        var result = _departmentService.ValidateDepartmentCode(departmentCode);

        // Assert
        Assert.IsFalse(result.Success);
        Assert.AreEqual("Department code can only contain letters and numbers.", result.ErrorMessage);
    }

    [Test]
    public void ValidateDepartmentCode_WhenCodeExists_ReturnsError()
    {
        // Jei paduodami departamento kodas "CS1234" kuris jau egzistuoja duomenų bazėje, tai gaunama klaida dėl kodo unikalumo pažeidimo.

        // Arrange
        var departmentCode = "CS1234"; // Assume this code already exists

        // Act
        var existingDepartment = _departmentService.GetDepartmentByCode(departmentCode);

        // Assert
        Assert.IsNotNull(existingDepartment);
    }

}
