using CustomExceptions;
using CourseTemplate.Data;

namespace CourseTemplate.Core
{
    public class CreateCourseTemplateDto
    {
        public string Name { get; set; } = null!;
    }

    public class ViewCourseTemplateDto
    {
        public string Name { get; set; } = null!;
        public string Id { get; set; } = null!;
    }

    public class CourseTemplateService
    {
        private readonly ICourseTemplateRepository _repository;

        public CourseTemplateService()
        {
            _repository = new CourseTemplateRepository();
        }
        
        public CourseTemplateService(ICourseTemplateRepository repository)
        {
            _repository = repository;
        }

        public async Task AddAsync(CreateCourseTemplateDto createCourse)
        {
            var dbCourseTemplates = await _repository.ListAsync();

            // Check if a createCourse with the same name already exists
            if (dbCourseTemplates.Any(c => c.CourseName == createCourse.Name))
            {
                throw new ConflictException($"A Course Template with the name '{createCourse.Name}' already exists.");
            }

            // If not, add the new createCourse
            await _repository.AddAsync(new Data.CourseTemplate()
            {
                CourseName = createCourse.Name
            });
        }

        public async Task<List<ViewCourseTemplateDto>> ListAsync()
        {
            var result = new List<ViewCourseTemplateDto>();
            var dbCourseTemplates = await _repository.ListAsync();
            foreach (var courseTemplate in dbCourseTemplates)
            {
                result.Add(new ViewCourseTemplateDto()
                {
                    Name = courseTemplate.CourseName,
                    Id = courseTemplate.Id.ToString(),
                });
            }
            return result;
        }

        public async Task<ViewCourseTemplateDto> GetByIdAsync(int id)
        {
            var courseTemplate = await _repository.GetByIdAsync(id);
            if (courseTemplate == null)
            {
                throw new NotFoundException($"Course with ID '{id}'");
            }
            return new ViewCourseTemplateDto()
            {
                Name = courseTemplate.CourseName,
                Id = courseTemplate.Id.ToString(),
            };
        }
    }
}
