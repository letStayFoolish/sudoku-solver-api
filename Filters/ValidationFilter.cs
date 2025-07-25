using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace sudoku_solver_api.Filters;

public class ValidationFilter : IActionFilter
  
{
  public void OnActionExecuting(ActionExecutingContext context)
  {
    if (!context.ModelState.IsValid)
    {
      var errors = context.ModelState.Where(e => e.Value?.Errors.Count > 0)
        .ToDictionary(
          kvp => kvp.Key,
          kvp => kvp.Value.Errors.Select(x => x.ErrorMessage).ToArray()
          );

      context.Result = new BadRequestObjectResult(new
      {
        Message = "Validation Failed",
        Errors = errors
      });
      
    }
  }
  public void OnActionExecuted(ActionExecutedContext context) { }
}