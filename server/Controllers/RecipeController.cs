using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace RecipeApp.Controllers;

[ApiController]
[Route("api/recipes")]

public class RecipeController : ControllerBase{
    private readonly RecipeService recipeService;
    private readonly Auth0Provider auth;
    public RecipeController(Auth0Provider auth, RecipeService recipeService){
        this.auth = auth;
        this.recipeService = recipeService;
    }

    [HttpPost]
    [Authorize]
    public async Task<ActionResult<Recipes>> CreateRecipe([FromBody] Recipes recipeData){
        try
        {
            Account userInfo = await auth.GetUserInfoAsync<Account>(HttpContext);
            recipeData.CreatorId = userInfo.Id;
            Recipes recipe = recipeService.CreateRecipe(recipeData);
            return Ok(recipe);
        }
        catch (Exception error)
        {
            
            return BadRequest(error.Message);
        }
    }

    [HttpGet]
    public ActionResult<List<Recipes>> GetRecipes(){
        try
        {
        List<Recipes> recipes = recipeService.GetRecipes();
        return Ok(recipes);
        }
        catch (Exception error)
        {
            
            return BadRequest(error.Message);
        }
    }

    [HttpGet("{recipeId}")]
    public ActionResult<Recipes> GetRecipeById(int recipeId){
        try
        {
            Recipes recipes = recipeService.GetRecipeById(recipeId);
            return Ok(recipes);
        }
        catch (Exception error)
        {
            
            return BadRequest(error.Message);
        }
    }

    [HttpPut("{recipeId}")]
    [Authorize]
    public ActionResult<Recipes> EditRecipe([FromBody] Recipes updateData, int recipeId){
        try
        {
            updateData.Id = recipeId;
            Recipes newRecipes = recipeService.EditRecipe(updateData, recipeId);
            return Ok(newRecipes);
        }
        catch (Exception error)
        {
            
            return BadRequest(error.Message);
        }
    }

    [HttpDelete("{recipeId}")]
    [Authorize]
    public ActionResult<string> RemoveRecipe(int recipeId){
        try
        {
            string message = recipeService.RemoveRecipe(recipeId);
            return Ok(message);
        }
        catch (Exception error)
        {
            return BadRequest(error.Message);
        }
    }
}