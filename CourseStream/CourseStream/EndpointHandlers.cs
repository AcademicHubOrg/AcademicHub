using CourseStream.Core;
using CustomExceptions;
using Microsoft.AspNetCore.Mvc;

namespace CourseStream;

internal static class EndpointHandlers
{
    public static IResult HealthCheck()
    {
        return Results.Ok();
    }

    public static async Task<object> ListOfCourseStreams([FromServices] CourseStreamService service)
    {
        var result = await service.ListAsync();
        return new { Data = result };
    }

    public static async Task<object> CheckEnrollments([FromServices] CourseStreamService service, int courseId)
    {
        var result = await service.GetEnrollments(courseId);
        return new { Data = result };
    }

    public static async Task<object> GetEnrollmentsByStudent([FromServices] CourseStreamService service, int studentId)
    {
        var result = await service.GetEnrollmentsByStudent(studentId);
        return new { Data = result };
    }

    public static async Task<object> AddCourse([FromServices] CourseStreamService service,
        [FromServices] IHttpClientFactory httpClientFactory, CourseStreamAddDto courseStream)
    {
        var client = httpClientFactory.CreateClient();
        // Use the service name and internal port here
        var response = await client.GetAsync("http://course-template:80/courseTemplates/" + courseStream.TemplateId);
        if (!response.IsSuccessStatusCode)
        {
            throw new ArgumentException("Invalid data provided. Template does not exist.");
        }

        if (string.IsNullOrEmpty(courseStream.Name))
        {
            throw new ArgumentException("Invalid data provided. Course name is required.");
        }

        await service.AddAsync(courseStream);
        return new { Message = "Course added successfully." };
    }

    public static async Task<object> EnrollStudent([FromServices] CourseStreamService service, int studentId,
        int courseStreamId)
    {
        try
        {
            await service.EnrollStudentAsync(studentId, courseStreamId);
            return new { Message = "Enrollment successful." };
        }
        catch (ConflictException ex)
        {
            return new { Message = ex.Message };
        }
    }


    public static async Task<object> GetCourseStreamById([FromServices] CourseStreamService service, int id)
    {
        var course = await service.GetByIdAsync(id);
        return course;
    }

    public static async Task<object> DeleteCourseStream([FromServices] CourseStreamService service,
        [FromServices] IHttpClientFactory httpClientFactory, int streamId)
    {
        var client = httpClientFactory.CreateClient();
        var response = await client.DeleteAsync("http://materials:80/materials/delete-by-course-id/" + streamId);
        if (!response.IsSuccessStatusCode)
        {
            throw new ArgumentException("Invalid data provided. Course Stream does not exist.");
        }

        try
        {
            await service.DeleteCourseStreamAsync(streamId);
            return new { Message = "Course stream and associated materials deleted successfully." };
        }


        catch (NotFoundException ex)
        {
            return new { Error = ex.Message };
        }
        catch (Exception ex)
        {
            return new { Error = "An error occurred while processing the request." };
        }
    }

    public static async Task<object> DeleteAllStreamsByTemplateId([FromServices] CourseStreamService service,
        [FromServices] IHttpClientFactory httpClientFactory, int id)
    {
        var client = httpClientFactory.CreateClient();
        try
        {
            List<int> courseStreams = await service.DeleteAllStreamsByTemplateId(id);
            foreach (var i in courseStreams)
            {
                var response = await client.DeleteAsync("http://materials:80/materials/delete-by-course-id/" + i);
            }

            return new { Message = "Course streams for associated template deleted successfully." };
        }
        catch (NotFoundException ex)
        {
            return new { Error = ex.Message };
        }
    }
}