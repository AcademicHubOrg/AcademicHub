using System.Collections;
using CourseTemplate.Core;
using CourseTemplate.Data;
using CustomExceptions;
using Moq;

namespace CourseTemplate.UnitTests
{
    public class CourseTemplateUnitTests
    {
        private readonly Mock<ICourseTemplateRepository> _mockRepository;
        private readonly CourseTemplateService _service;

        public CourseTemplateUnitTests()
        {
            _mockRepository = new Mock<ICourseTemplateRepository>();
            _service = new CourseTemplateService(_mockRepository.Object);
        }

        [Fact]
        public async Task AddAsync_WhenCourseDoesNotExist_ShouldAddCourse()
        {
            // Arrange
            _mockRepository.Setup(repo => repo.ListAsync())
                .ReturnsAsync(new List<Data.CourseTemplate>());

            var createCourse = new CreateCourseTemplateDto { Name = "NewCourse" };

            // Act
            await _service.AddAsync(createCourse);

            // Assert
            _mockRepository.Verify(repo => repo.AddAsync(It.IsAny<Data.CourseTemplate>()), Times.Once);
        }

        [Fact]
        public async Task AddAsync_WhenCourseExists_ShouldThrowConflictException()
        {
            // Arrange
            var existingCourse = new Data.CourseTemplate { Id = 1, CourseName = "ExistingCourse" };
            _mockRepository.Setup(repo => repo.ListAsync())
                .ReturnsAsync(new List<Data.CourseTemplate> { existingCourse });

            var createCourse = new CreateCourseTemplateDto { Name = "ExistingCourse" };

            // Act and Assert
            await Assert.ThrowsAsync<ConflictException>(() => _service.AddAsync(createCourse));
        }

        [Theory]
        [ClassData(typeof(CourseTemplateTestData))]
        public async Task ListAsync_ShouldReturnListOfCourseTemplates(List<Data.CourseTemplate> expectedCourseTemplates)
        {
            // Arrange
            _mockRepository.Setup(repo => repo.ListAsync())
                .ReturnsAsync(expectedCourseTemplates);

            // Act
            var result = await _service.ListAsync();

            // Assert
            Assert.Equal(expectedCourseTemplates.Count, result.Count);
            Assert.Equal(expectedCourseTemplates.Select(c => c.Id), result.Select(c => int.Parse(c.Id)));
            Assert.Equal(expectedCourseTemplates.Select(c => c.CourseName), result.Select(c => c.Name));
        }

        [Fact]
        public async Task GetByIdAsync_WhenCourseExists_ShouldReturnViewCourseTemplateDto()
        {
            // Arrange
            var existingCourse = new Data.CourseTemplate { Id = 1, CourseName = "ExistingCourse" };
            _mockRepository.Setup(repo => repo.GetByIdAsync(1))
                .ReturnsAsync(existingCourse);

            // Act
            var result = await _service.GetByIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(existingCourse.Id.ToString(), result.Id);
            Assert.Equal(existingCourse.CourseName, result.Name);
        }

        [Fact]
        public async Task GetByIdAsync_WhenCourseDoesNotExist_ShouldThrowNotFoundException()
        {
            // Arrange
            _mockRepository.Setup(repo => repo.GetByIdAsync(1))
                .ReturnsAsync((Data.CourseTemplate)null);

            // Act and Assert
            await Assert.ThrowsAsync<NotFoundException>(() => _service.GetByIdAsync(1));
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
    }

    public class CourseTemplateTestData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[]
            {
                new List<Data.CourseTemplate>
                {
                    new Data.CourseTemplate { Id = 1, CourseName = "Course1" },
                    new Data.CourseTemplate { Id = 2, CourseName = "Course2" },
                    new Data.CourseTemplate { Id = 3, CourseName = "Course3" }
                }
            };
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
