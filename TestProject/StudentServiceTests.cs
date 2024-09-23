using Microsoft.EntityFrameworkCore;
using TarpinisAtsiskaitymasAdasDB.Data;
using TarpinisAtsiskaitymasAdasDB.Entities;
using TarpinisAtsiskaitymasAdasDB.Repositories;
using TarpinisAtsiskaitymasAdasDB.Services;

namespace TestProject;

public class StudentServiceTests
{
    private ApplicationContext _dbContext;
    private StudentRepository _studentRepository;
    private StudentService _studentService;

    [SetUp]
    public void Setup()
    {
        _dbContext = new ApplicationContext();
        _studentRepository = new StudentRepository(_dbContext);
        _studentService = new StudentService(_studentRepository);
    }

    [TearDown]
    public void TearDown()
    {
        _dbContext.Dispose();
    }
    [Test]
    public void ValidateStudent_NameContainsDigits_ReturnsError()
    {
        // Jei paduodami studento vardas "Jo1n" ir pavardė "Smith", tai gaunama klaida, nes vardas turi būti sudarytas tik iš raidžių.

        // Arrange
        var student = new Student
        {
            FirstName = "Jo1n",
            LastName = "Smith"
        };

        // Act
        var result = _studentService.ValidateStudent(student);

        // Assert
        Assert.IsFalse(result.Success);
        Assert.AreEqual("First name must contain only letters.", result.ErrorMessage);
    }

    [Test]
    public void ValidateStudent_FirstNameTooShort_ReturnsError()
    {
        // Jei paduodami studento vardas "J" ir pavardė "Smith", tai gaunama klaida, nes vardas turi būti ne trumpesnis kaip 2 simboliai.

        // Arrange
        var student = new Student
        {
            FirstName = "J",
            LastName = "Smith"
        };

        // Act
        var result = _studentService.ValidateStudent(student);

        // Assert
        Assert.IsFalse(result.Success);
        Assert.AreEqual("First name must be at least 2 characters long.", result.ErrorMessage);
    }

    [Test]
    public void ValidateStudent_FirstNameTooLong_ReturnsError()
    {
        // Jei paduodami studento vardas "JohnathonJohnathonJohnathonJohnathonJohnathon" (51 simbolis) ir pavardė "Smith", tai gaunama klaida, nes vardas turi būti ne ilgesnis kaip 50 simbolių.

        // Arrange
        var student = new Student
        {
            FirstName = "JohnathonJohnathonJohnathonJohnathonJohnathonJohnathon", // 54 characters
            LastName = "Smith"
        };

        // Act
        var result = _studentService.ValidateStudent(student);

        // Assert
        Assert.IsFalse(result.Success);
        Assert.AreEqual("First name cannot exceed 50 characters.", result.ErrorMessage);
    }

    [Test]
    public void ValidateStudent_LastNameContainsSpecialCharacters_ReturnsError()
    {
        // Jei paduodami studento vardas "John" ir pavardė "Sm!th", tai gaunama klaida, nes pavardė turi būti sudaryta tik iš raidžių.

        // Arrange
        var student = new Student
        {
            FirstName = "John",
            LastName = "Sm!th"
        };

        // Act
        var result = _studentService.ValidateStudent(student);

        // Assert
        Assert.IsFalse(result.Success);
        Assert.AreEqual("Last name must contain only letters.", result.ErrorMessage);
    }

    [Test]
    public void ValidateStudent_StudentNumberTooShort_ReturnsError()
    {
        // Jei paduodami studento numeris "1234567" (7 skaitmenys), tai gaunama klaida, nes numeris turi būti tiksliai 8 simbolių ilgio.

        // Arrange
        var studentNumber = "1234567"; // 7 digits

        // Act
        var result = _studentService.ValidateStudentNumber(studentNumber);

        // Assert
        Assert.IsFalse(result.Success);
        Assert.AreEqual("Student number must be exactly 8 characters long.", result.ErrorMessage);
    }
    [Test]
    public void ValidateStudent_StudentNumberTooLong_ReturnsError()
    {
        // Jei paduodami studento numeris "123456789" (9 skaitmenys), tai gaunama klaida, nes numeris turi būti tiksliai 8 simbolių ilgio.

        // Arrange
        var studentNumber = "123456789"; // 9 digits

        // Act
        var result = _studentService.ValidateStudentNumber(studentNumber);

        // Assert
        Assert.IsFalse(result.Success);
        Assert.AreEqual("Student number must be exactly 8 characters long.", result.ErrorMessage);
    }
    [Test]
    public void ValidateStudent_StudentNumberWithLetters_ReturnsError()
    {
        // Jei paduodami studento numeris "1234ABCD", tai gaunama klaida, nes numeris turi būti sudarytas tik iš skaičių.

        // Arrange
        var studentNumber = "1234ABCD"; // Contains letters

        // Act
        var result = _studentService.ValidateStudentNumber(studentNumber);

        // Assert
        Assert.IsFalse(result.Success);
        Assert.AreEqual("Student number must contain only digits.", result.ErrorMessage);
    }
    [Test]
    public void ValidateStudent_StudentNumberAlreadyExists_ReturnsError()
    {
        // Jei paduodami studento numeris "12345678", kuris jau egzistuoja duomenų bazėje, tai gaunama klaida dėl numerio unikalumo pažeidimo.

        // Arrange
        var studentNumber = 12345678; // Existing student number

        // Act
        var exists = _studentService.StudentExists(studentNumber);

        // Assert
        Assert.IsTrue(exists);
    }
    [Test]
    public void ValidateStudent_StudentNumberInvalid_ReturnsMultipleErrors()
    {
        // Arrange
        var studentNumber = "ABC";


        // Act
        var result = _studentService.ValidateStudentNumber(studentNumber);

        // Assert
        Assert.IsFalse(result.Success);
        Assert.IsTrue(result.ErrorMessage.Contains("Student number must be exactly 8 characters long."));
        Assert.IsTrue(result.ErrorMessage.Contains("Student number must contain only digits."));
    }
    [Test]
    public void ValidateStudent_InvalidEmailFormat_ReturnsError()
    {
        // Jei paduodami studento el. paštas "john.smithexample.com" (trūksta @), tai gaunama klaida, nes el. paštas turi būti teisingo formato.

        // Arrange
        var student = new Student
        {
            FirstName = "John",
            LastName = "Smith",
            StudentNumber = 12345671,
            Email = "john.smithexample.com", // Missing @ symbol
            DepartmentCode = "CS101"
        };

        // Act
        var result = _studentService.ValidateStudent(student);

        // Assert
        Assert.IsFalse(result.Success);
        Assert.AreEqual("Invalid email format.", result.ErrorMessage);
    }

    [Test]
    public void ValidateStudent_InvalidEmailFormatMissingDomain_ReturnsError()
    {
        // Jei paduodami studento el. paštas "john.smith@" (trūksta domeno), tai gaunama klaida dėl netinkamo formato.

        // Arrange
        var student = new Student
        {
            FirstName = "John",
            LastName = "Smith",
            StudentNumber = 12345671,
            Email = "john.smith@", // Missing domain part
            DepartmentCode = "CS101"
        };

        // Act
        var result = _studentService.ValidateStudent(student);

        // Assert
        Assert.IsFalse(result.Success);
        Assert.AreEqual("Invalid email format.", result.ErrorMessage);
    }

    [Test]
    public void ValidateStudent_InvalidEmailFormatMissingLocalPart_ReturnsError()
    {
        // Jei paduodami studento el. paštas "@example.com" (trūksta vietovardžio), tai gaunama klaida dėl netinkamo formato.

        // Arrange
        var student = new Student
        {
            FirstName = "John",
            LastName = "Smith",
            StudentNumber = 12345671,
            Email = "@example.com", // Missing local part before '@'
            DepartmentCode = "CS101"
        };

        // Act
        var result = _studentService.ValidateStudent(student);

        // Assert
        Assert.IsFalse(result.Success);
        Assert.AreEqual("Invalid email format.", result.ErrorMessage);
    }

    [Test]
    public void ValidateStudent_InvalidEmailFormatMissingDomainSuffix_ReturnsError()
    {
        // Jei paduodami studento el. paštas "john.smith@example" (trūksta domeno pabaigos), tai gaunama klaida dėl netinkamo formato.

        // Arrange
        var student = new Student
        {
            FirstName = "John",
            LastName = "Smith",
            StudentNumber = 12345671,
            Email = "john.smith@example", // Missing domain suffix (e.g., .com, .net)
            DepartmentCode = "CS101"
        };

        // Act
        var result = _studentService.ValidateStudent(student);

        // Assert
        Assert.IsFalse(result.Success);
        Assert.AreEqual("Invalid email format.", result.ErrorMessage);
    }
    [Test]
    public void ValidateStudent_InvalidEmailFormatTrailingDot_ReturnsError()
    {
        // Jei paduodami studento el. paštas "john.smith@example." (trūksta domeno pabaigos), tai gaunama klaida dėl netinkamo formato.

        // Arrange
        var student = new Student
        {
            FirstName = "John",
            LastName = "Smith",
            StudentNumber = 12345671,
            Email = "john.smith@example.", // Missing domain suffix (ends with a dot)
            DepartmentCode = "CS101"
        };

        // Act
        var result = _studentService.ValidateStudent(student);

        // Assert
        Assert.IsFalse(result.Success);
        Assert.AreEqual("Invalid email format.", result.ErrorMessage);
    }
    [Test]
    public void ValidateStudent_MissingEmail_ReturnsError()
    {
        // Jei nepaduodamas studento el. paštas, tai gaunama klaida, nes el. paštas yra privalomas.

        // Arrange
        var student = new Student
        {
            FirstName = "John",
            LastName = "Smith",
            StudentNumber = 12345671,
            //Email = null, // Missing email
            DepartmentCode = "CS101"
        };

        // Act
        var result = _studentService.ValidateStudent(student);

        // Assert
        Assert.IsFalse(result.Success);
        Assert.AreEqual("Email is required.", result.ErrorMessage);
    }
    [Test]
    public void ValidateStudent_MissingDepartment_ReturnsError()
    {
        // Jei nepaduodamas studento Departamentas, tai gaunama klaida, nes Departamentas yra privalomas.

        // Arrange
        var student = new Student
        {
            FirstName = "John",
            LastName = "Smith",
            StudentNumber = 12345671,
            Email = "john.smithas@example.com",
            //DepartmentCode = null // Missing department
        };

        // Act
        var result = _studentService.ValidateStudent(student);

        // Assert
        Assert.IsFalse(result.Success);
        Assert.AreEqual("Department is required.", result.ErrorMessage);
    }
    [Test]
    public void ValidateStudent_MissingFirstName_ReturnsError()
    {
        // Jei paduodami studento vardas tuščias "" arba null, tai gaunama klaida, nes vardas yra privalomas laukas.

        // Arrange
        var student = new Student
        {
            FirstName = "", // Empty first name
            LastName = "Smith",
            StudentNumber = 12345671,
            Email = "john.smith@example.com",
            DepartmentCode = "CS101"
        };

        // Act
        var result = _studentService.ValidateStudent(student);

        // Assert
        Assert.IsFalse(result.Success);
        Assert.AreEqual("First name is required.", result.ErrorMessage);
    }
    [Test]
    public void ValidateStudent_NullFirstName_ReturnsError()
    {
        // Jei paduodami studento vardas null, tai gaunama klaida, nes vardas yra privalomas laukas.

        // Arrange
        var student = new Student
        {
            FirstName = null, // Null first name
            LastName = "Smith",
            StudentNumber = 12345671,
            Email = "john.smith@example.com",
            DepartmentCode = "CS101"
        };

        // Act
        var result = _studentService.ValidateStudent(student);

        // Assert
        Assert.IsFalse(result.Success);
        Assert.AreEqual("First name is required.", result.ErrorMessage);
    }
    [Test]
    public void ValidateStudent_DuplicateEmail_ReturnsError()
    {
        // Jei paduodami du studentai su tuo pačiu el. pašto adresu "alice.johnson@example.com", tai gaunama klaida dėl el. pašto unikalumo pažeidimo.

        // Arrange

        var newStudent = new Student
        {
            FirstName = "Bob",
            LastName = "Smith",
            StudentNumber = 12345672,
            Email = "alice.johnson@example.com", // Same email as existing student
            DepartmentCode = "CS102"
        };

        // Act
        var result = _studentService.ValidateStudent(newStudent);

        // Assert
        Assert.IsFalse(result.Success);
        Assert.AreEqual("Email must be unique.", result.ErrorMessage);
    }

}