using System.Net.Mime;
using ACME.LearningCenterPlatform.API.Publishing.Domain.Model.Queries;
using ACME.LearningCenterPlatform.API.Publishing.Domain.Services;
using ACME.LearningCenterPlatform.API.Publishing.Interfaces.REST.Resources;
using ACME.LearningCenterPlatform.API.Publishing.Interfaces.REST.Transform;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace ACME.LearningCenterPlatform.API.Publishing.Interfaces.REST;

[ApiController]
[Route("api/v1/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
[SwaggerTag("Available Category Endpoints")]
public class CategoriesController(ICategoryCommandService categoryCommandService, ICategoryQueryService categoryQueryService) : ControllerBase
{

    [HttpGet("{categoryId:int}")]
    [SwaggerOperation(
        Summary = "Get Category by Id",
        Description = "Get a category by its unique identifier",
        OperationId = "GetCategoryById")]
    [SwaggerResponse(StatusCodes.Status200OK, "The category was found", typeof(CategoryResource))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "The category was not found")]
    public async Task<IActionResult> GetCategoryById(int categoryId)
    {
        var getCategoryByIdQuery = new GetCategoryByIdQuery(categoryId);
        var category = await categoryQueryService.Handle(getCategoryByIdQuery);
        if (category is null) return NotFound();
        var categoryResource = CategoryResourceFromEntityAssembler.ToResourceFromEntity(category);
        return Ok(categoryResource);
    }

    [HttpPost]
    [SwaggerOperation(
        Summary = "Create a new Category",
        Description = "Create a new category",
        OperationId = "CreateCategory")]
    [SwaggerResponse(StatusCodes.Status201Created, "The category was created", typeof(CategoryResource))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "The category was not created")]
    public async Task<IActionResult> CreateCategory([FromBody] CreateCategoryResource resource)
    {
        var createCategoryCommand = CreateCategoryCommandFromResourceAssembler.ToCommandFromResource(resource);
        var category = await categoryCommandService.Handle(createCategoryCommand);
        if (category is null) return BadRequest();
        var categoryResource = CategoryResourceFromEntityAssembler.ToResourceFromEntity(category);
        return CreatedAtAction(nameof(GetCategoryById), new { categoryId = category.Id }, categoryResource);
    }
    
    [HttpGet]
    [SwaggerOperation(
        Summary = "Get All Categories",
        Description = "Get all categories",
        OperationId = "GetAllCategories")]
    [SwaggerResponse(StatusCodes.Status200OK, "The categories were found", typeof(IEnumerable<CategoryResource>))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "The categories were not found")]
    public async Task<IActionResult> GetAllCategories()
    {
        var categories = await categoryQueryService.Handle(new GetAllCategoriesQuery());
        var categoryResources = categories.Select(CategoryResourceFromEntityAssembler.ToResourceFromEntity);
        return Ok(categoryResources);
    }
}