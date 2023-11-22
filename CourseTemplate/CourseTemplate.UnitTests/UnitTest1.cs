using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CourseTemplate.Core;
using CourseTemplate.Data;
using CustomExceptions;
using Moq;
using Xunit;

namespace CourseTemplate.UnitTests
{
	public class UnitTest1
	{
		[Fact]
		public async Task AddAsync_WhenCourseDoesNotExist_ShouldAddCourse()
		{
			// Arrange
			var mockRepository = new Mock<ICourseTemplateRepository>();
			mockRepository.Setup(repo => repo.ListAsync())
				.ReturnsAsync(new List<Data.CourseTemplate>());

			var service = new CourseTemplateService(mockRepository.Object);
			var createCourse = new CreateCourseTemplateDto { Name = "NewCourse" };

			// Act
			await service.AddAsync(createCourse);

			// Assert
			mockRepository.Verify(repo => repo.AddAsync(It.IsAny<Data.CourseTemplate>()), Times.Once);
		}

		[Fact]
		public async Task AddAsync_WhenCourseExists_ShouldThrowConflictException()
		{
			// Arrange
			var existingCourse = new Data.CourseTemplate { Id = 1, CourseName = "ExistingCourse" };
			var mockRepository = new Mock<ICourseTemplateRepository>();
			mockRepository.Setup(repo => repo.ListAsync())
				.ReturnsAsync(new List<Data.CourseTemplate> { existingCourse });

			var service = new CourseTemplateService(mockRepository.Object);
			var createCourse = new CreateCourseTemplateDto { Name = "ExistingCourse" };

			// Act and Assert
			await Assert.ThrowsAsync<ConflictException>(() => service.AddAsync(createCourse));
		}
		
		[Fact]
		public async Task ListAsync_ShouldReturnListOfCourseTemplates()
		{
			// Arrange
			var expectedCourseTemplates = new List<Data.CourseTemplate>
			{
				new Data.CourseTemplate { Id = 1, CourseName = "Course1" },
				new Data.CourseTemplate { Id = 2, CourseName = "Course2" },
				new Data.CourseTemplate { Id = 3, CourseName = "Course3" }
			};

			var mockRepository = new Mock<ICourseTemplateRepository>();
			mockRepository.Setup(repo => repo.ListAsync())
				.ReturnsAsync(expectedCourseTemplates);

			var service = new CourseTemplateService(mockRepository.Object);

			// Act
			var result = await service.ListAsync();

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
			var mockRepository = new Mock<ICourseTemplateRepository>();
			mockRepository.Setup(repo => repo.GetByIdAsync(1))
				.ReturnsAsync(existingCourse);

			var service = new CourseTemplateService(mockRepository.Object);

			// Act
			var result = await service.GetByIdAsync(1);

			// Assert
			Assert.NotNull(result);
			Assert.Equal(existingCourse.Id.ToString(), result.Id);
			Assert.Equal(existingCourse.CourseName, result.Name);
		}
		
		[Fact]
		public async Task GetByIdAsync_WhenCourseDoesNotExist_ShouldThrowNotFoundException()
		{
			// Arrange
			var mockRepository = new Mock<ICourseTemplateRepository>();
			mockRepository.Setup(repo => repo.GetByIdAsync(1))
				.ReturnsAsync((Data.CourseTemplate)null);

			var service = new CourseTemplateService(mockRepository.Object);

			// Act and Assert
			await Assert.ThrowsAsync<NotFoundException>(() => service.GetByIdAsync(1));
		}
	}
}