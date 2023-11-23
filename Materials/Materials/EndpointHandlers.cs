using Materials.Core;
using Microsoft.AspNetCore.Mvc;

namespace Materials;

internal static class EndpointHandlers
{
	public static IResult HealthCheck()
	{
		return Results.Ok();
	}
    
	public static async Task<object> ListOfCourseTemplates([FromServices] MaterialService service)
    {
    	var result = await service.ListAsync();
    	return new { Data = result };
    }
    
	public static async Task<object> AddMaterial([FromServices] MaterialService service, MaterialDataDtoAdd material)
    {
    	if (string.IsNullOrEmpty(material.Name))
    	{
    		throw new ArgumentException("Invalid data provided. Material name is required.");
    	}
    	if (string.IsNullOrEmpty(material.CourseId))
    	{
    		throw new ArgumentException("Invalid data provided. CourseId is required.");
    	}
    
    	try
    	{ 
    		Convert.ToInt32(material.CourseId);
    	}
    	catch (Exception e)
    	{
    		throw new ArgumentException("Invalid data provided: CourseId.");
    	}
    
    	await service.AddAsync(material);
    	return new { Message = "Material added successfully." };
    }
    
	public static async Task<object> GetMaterialByMaterialId([FromServices] MaterialService service, int? materialId)
    {
    	if (materialId is null)
    	{
    		throw new ArgumentException("Invalid data provided. Material Id is required.");
    	}
    	return await service.ListByIdAsync(materialId.Value);
    }
    
	public static async Task<object> GetMaterialSByCourseId([FromServices] MaterialService service, int? courseId)
    {
    	if (courseId is null)
    	{
    		throw new ArgumentException("Invalid data provided. Course Id is required.");
    	}
    
    	return await service.ListByCourseIdAsync(courseId.Value);
    }
}