using CourseStream.Core;
using CourseStream.Data;
using CustomExceptions;
using Moq;


namespace CourseStream.UnitTests;

public class CourseStreamServiceTest
{
    [Fact]
    public async Task AddAsync_CallsAddOnRepository_WithCourseStream()
    {
        // Arrange
        var mockRepo = new Mock<ICourseStreamRepository>();
        var service = new CourseStreamService(mockRepo.Object);
        var courseStreamAddDto = new CourseStreamAddDto {Name = "TestCourse", TemplateId = 1};
        mockRepo.Setup(r => r.ListAsync()).ReturnsAsync(new List<Data.CourseStream>());

        // Act
        await service.AddAsync(courseStreamAddDto);

        // Assert
        mockRepo.Verify(r => r.AddAsync(It.Is<Data.CourseStream>(c => c.CourseName == courseStreamAddDto.Name
                                                                      && c.TemplateId ==
                                                                      courseStreamAddDto.TemplateId)), Times.Once);
    }

    [Fact]
    public async Task AddAsync_WhenCourseNameExists_ThrowsConflictException()
    {
        // Arrange
        var mockRepo = new Mock<ICourseStreamRepository>();
        var service = new CourseStreamService(mockRepo.Object);

        var existingCourseName = "Existing Course";
        var dbCourseStreams = new List<Data.CourseStream>
        {
            new Data.CourseStream {CourseName = existingCourseName, TemplateId = 101}
            // Add more course streams as needed
        };

        var newCourseStream = new CourseStreamAddDto
        {
            Name = existingCourseName, // same name as existing course
            TemplateId = 202
        };

        mockRepo.Setup(r => r.ListAsync()).ReturnsAsync(dbCourseStreams);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<ConflictException>(async () =>
            await service.AddAsync(newCourseStream));
        Assert.Equal($"Course with name '{existingCourseName}' already exists", exception.Message);
    }


    [Fact]
    public async Task EnrollStudentAsync_WhenStudentNotEnrolled_CallsEnrollOnRepository()
    {
        // Arrange
        var mockRepo = new Mock<ICourseStreamRepository>();
        var service = new CourseStreamService(mockRepo.Object);

        var studentId = 2;
        var courseStreamId = 2;
        mockRepo.Setup(r => r.IsStudentEnrolledAsync(studentId, courseStreamId))
            .ReturnsAsync(false);

        // Act
        await service.EnrollStudentAsync(studentId, courseStreamId);

        // Assert
        mockRepo.Verify(r => r.EnrollStudentAsync(studentId, courseStreamId, It.IsAny<DateTime>()), Times.Once);
    }

    [Fact]
    public async Task EnrollStudentAsync_WhenStudentAlreadyEnrolled_ThrowsConflictException()
    {
        // Arrange
        var mockRepo = new Mock<ICourseStreamRepository>();
        var service = new CourseStreamService(mockRepo.Object);

        var studentId = 2;
        var courseStreamId = 2;
        mockRepo.Setup(r => r.IsStudentEnrolledAsync(studentId, courseStreamId))
            .ReturnsAsync(true);

        // Act & Assert
        await Assert.ThrowsAsync<ConflictException>(async () =>
            await service.EnrollStudentAsync(studentId, courseStreamId));
    }

    [Fact]
    public async Task ListAsync_ReturnsMappedCourseStreams()
    {
        // Arrange
        var mockRepo = new Mock<ICourseStreamRepository>();
        var service = new CourseStreamService(mockRepo.Object);

        var dbCourseStreams = new List<Data.CourseStream>
        {
            new Data.CourseStream {Id = 1, CourseName = "Course 1", TemplateId = 101},
            new Data.CourseStream {Id = 2, CourseName = "Course 2", TemplateId = 102}
            // Add more course streams as needed
        };

        mockRepo.Setup(r => r.ListAsync()).ReturnsAsync(dbCourseStreams);

        // Act
        var result = await service.ListAsync();

        // Assert
        Assert.Equal(dbCourseStreams.Count, result.Count);
        for (var i = 0; i < dbCourseStreams.Count; i++)
        {
            Assert.Equal(dbCourseStreams[i].CourseName, result[i].Name);
            Assert.Equal(dbCourseStreams[i].Id.ToString(), result[i].Id);
            Assert.Equal(dbCourseStreams[i].TemplateId.ToString(), result[i].TemplateId);
        }
    }

    [Fact]
    public void CourseStreamService_Constructor_InitializesRepository()
    {
        // Arrange & Act
        var mockRepo = new Mock<ICourseStreamRepository>();
        var service = new CourseStreamService(mockRepo.Object);

        // Assert

        var exception = Record.ExceptionAsync(async () => await service.ListAsync());
        Assert.Null(exception.Exception);
    }

    [Fact]
    public async Task GetByIdAsync_WhenCourseStreamFound_ReturnsCourseStreamShowDto()
    {
        // Arrange
        var mockRepo = new Mock<ICourseStreamRepository>();
        var service = new CourseStreamService(mockRepo.Object);

        int courseId = 1;
        var dbCourseStream = new Data.CourseStream
        {
            Id = courseId,
            CourseName = "Test Course",
            TemplateId = 101
        };

        mockRepo.Setup(r => r.GetByIdAsync(courseId)).ReturnsAsync(dbCourseStream);

        // Act
        var result = await service.GetByIdAsync(courseId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(dbCourseStream.CourseName, result.Name);
        Assert.Equal(dbCourseStream.Id.ToString(), result.Id);
        Assert.Equal(dbCourseStream.TemplateId.ToString(), result.TemplateId);
    }

    [Fact]
    public async Task GetByIdAsync_WhenCourseStreamNotFound_ThrowsNotFoundException()
    {
        // Arrange
        var mockRepo = new Mock<ICourseStreamRepository>();
        var service = new CourseStreamService(mockRepo.Object);

        int courseId = 1;
        mockRepo.Setup(r => r.GetByIdAsync(courseId)).ReturnsAsync((Data.CourseStream) null!);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(async () =>
            await service.GetByIdAsync(courseId));
    }

    [Fact]
    public Task AcademicHubUrl_ShouldEqual_ExpectedValue()
    {
        // Arrange
        var expected = "https://academichub.net/";

        // Act
        var actual = Address.academichuburl;

        // Assert
        Assert.Equal(expected, actual);
        return Task.CompletedTask;
    }
    
    [Fact]
    public async Task DeleteAsync_CallsDeleteOnRepository_WithCourseStream()
    {
        // Arrange
        var mockRepo = new Mock<ICourseStreamRepository>();
        var service = new CourseStreamService(mockRepo.Object);

        var courseId = 1;
        var dbCourseStream = new Data.CourseStream
        {
            Id = courseId,
            CourseName = "Test Course",
            TemplateId = 101
        };

        mockRepo.Setup(r => r.GetByIdAsync(courseId)).ReturnsAsync(dbCourseStream);

        // Act
        await service.DeleteCourseStreamAsync(courseId);

        // Assert
        mockRepo.Verify(r => r.DeleteAsync(It.Is<Data.CourseStream>(c => c.Id == courseId)), Times.Once);
    }
    [Fact]
    public async Task DeleteCourseStreamAsync_CallsGetByIdAsyncAndDeleteOnRepository()
    {
        // Arrange
        var mockRepo = new Mock<ICourseStreamRepository>();
        var service = new CourseStreamService(mockRepo.Object);

        var streamId = 1;
        var dbCourseStream = new Data.CourseStream
        {
            Id = streamId,
            CourseName = "Test Course",
            TemplateId = 101
        };

        mockRepo.Setup(r => r.GetByIdAsync(streamId)).ReturnsAsync(dbCourseStream);

        // Act
        await service.DeleteCourseStreamAsync(streamId);

        // Assert
        mockRepo.Verify(r => r.GetByIdAsync(streamId), Times.Once);
        mockRepo.Verify(r => r.DeleteAsync(It.Is<Data.CourseStream>(c => c.Id == streamId)), Times.Once);
    }
    
    [Fact]
    public async Task DeleteAsync_WhenCourseStreamNotFound_ThrowsNotFoundException()
    {
        // Arrange
        var mockRepo = new Mock<ICourseStreamRepository>();
        var service = new CourseStreamService(mockRepo.Object);

        int streamId = 1;
        mockRepo.Setup(r => r.GetByIdAsync(streamId)).ReturnsAsync((Data.CourseStream)null);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(async () =>
            await service.DeleteCourseStreamAsync(streamId));
    }

}