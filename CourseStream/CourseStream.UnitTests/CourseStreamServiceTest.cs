using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CourseStream.Core;
using CourseStream.Data;
using CustomExceptions;
using Moq;
using Xunit;

namespace CourseStream.UnitTests
{
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
                                                                   && c.TemplateId == courseStreamAddDto.TemplateId)), Times.Once);
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
                new Data.CourseStream { CourseName = existingCourseName, TemplateId = 101 }
                // Add more course streams as needed
            };

            var newCourseStream = new CourseStreamAddDto
            {
                Name = existingCourseName,  // same name as existing course
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
                new Data.CourseStream { Id = 1, CourseName = "Course 1", TemplateId = 101 },
                new Data.CourseStream { Id = 2, CourseName = "Course 2", TemplateId = 102 }
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
            var service = new CourseStreamService();

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
            mockRepo.Setup(r => r.GetByIdAsync(courseId)).ReturnsAsync((Data.CourseStream)null);

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(async () =>
                await service.GetByIdAsync(courseId));
        }




    }
}
