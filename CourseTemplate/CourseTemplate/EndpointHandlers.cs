using CourseTemplate.Core;
using CustomExceptions;

namespace CourseTemplate;

using Microsoft.AspNetCore.Mvc;
using System.Net.Http;

internal static class EndpointHandlers
{
    public static IResult HealthCheck()
    {
        return Results.Ok();
    }

    public static async Task<object> ListOfCourseTemplates([FromServices] CourseTemplateService service)
    {
        var result = await service.ListAsync();
        return new { Data = result };
    }

    public static async Task<object> AddCourse([FromServices] CourseTemplateService service,
        CreateCourseTemplateDto courseTemplate)
    {
        if (string.IsNullOrEmpty(courseTemplate.Name))
        {
            throw new ArgumentException("Invalid data provided. Course name is required.");
        }

        await service.AddAsync(courseTemplate);
        return new { Message = "Course added successfully." };
    }

    public static async Task<object> GetCourseById([FromServices] CourseTemplateService service, int id)
    {
        var course = await service.GetByIdAsync(id);
        return course;
    }

    public static async Task<object> DeleteCourseTemplate([FromServices] CourseTemplateService service,
        [FromServices] IHttpClientFactory httpClientFactory, int id)
    {
        var client = httpClientFactory.CreateClient();
        //var link = "http://course-stream:80/courseStreams/delete-all-by/" + id;
        var link = "http://course-stream:80/courseStreams/delete-all-by/{streamTemplateId}?id=" + id;
        var link2 = "http://materials:80/materials/delete-by-template-id/" + id;
        //courseStreams/delete-all-by/{streamTemplateId}?id=1
        var response = await client.DeleteAsync(link);
        var response2 = await client.DeleteAsync(link2);
        if (!response.IsSuccessStatusCode)
        {
            throw new ArgumentException("Invalid data provided. Course Template does not exist. " + link);
        }

        if (!response2.IsSuccessStatusCode)
        {
            throw new ArgumentException("Invalid data provided" + link);
        }

        try
        {
            await service.DeleteCourseAsync(id);
            return new { Message = "Course template deleted successfully." };
        }
        catch (NotFoundException ex)
        {
            return new { Error = ex.Message };
        }
        catch (Exception ex)
        {
            return new { Error = "An error occurred while deleting the course template." };
        }
    }
}