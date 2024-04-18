using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Recipe.Models.DTO;
using Recipe.Models;
using Recipe.Repositories.Implementation;
using Recipe.Repositories.Interface;

namespace Recipe.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecipeTypeController : ControllerBase
    {
        private readonly IRecipeTypeRepository _recipeTypeRepository;
        public RecipeTypeController(IRecipeTypeRepository recipeType)
        {
            _recipeTypeRepository = recipeType;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllRecipeTypesAsync()
        {
            var recipeTypes = await _recipeTypeRepository.GetAllTypes();

            //if (recipeTypes != null) return Ok(recipeTypes);
            //return NotFound();
            var response = new List<RecipeTypeDTO>();
            if (recipeTypes == null)
            {
                return Ok(recipeTypes);
            }
            foreach (var type in recipeTypes)
            {
                response.Add(new RecipeTypeDTO
                {
                    Id = type.Id,
                    Type = type.Type
                });
            }
            return Ok(response);
        }
    }
}
