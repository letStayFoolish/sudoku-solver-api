using Microsoft.AspNetCore.Mvc;
using sudoku_solver_api.Helpers;
using sudoku_solver_api.Models;
using sudoku_solver_api.Services;

namespace sudoku_solver_api.Controllers;

[ApiController]
[Route("api/sudoku")]
public class SudokuController : ControllerBase
{
  private readonly ISudokuService _sudokuService;

  public SudokuController(ISudokuService sudokuService)
  {
    _sudokuService = sudokuService;
  }

  // GET - Generates new sudoku puzzle - /generate?difficulty=easy
  [HttpGet]
  [Route("generate")]
  public ActionResult<int[][]> GenerateSudoku([FromQuery] Difficulty difficulty = Difficulty.Easy)
  {
    var generatedGrid = _sudokuService.GenerateSudoku(difficulty);
    var result = ArrayConverter.ToJagged(generatedGrid);
    // return Ok(generatedGrid.Cast<int>().Select(x => new[] { x }).ToArray());
    return Ok(result);
  }

  [HttpGet]
  [Route("solve")]
  public ActionResult SolvePuzzle([FromBody] int[][] puzzleGrid)
  {
    try
    {
      // Convert jagged array to multidimensional array
      var solution = _sudokuService.GetSolution(puzzleGrid);
      return Ok(solution);
    }
    catch (Exception ex)
    {
      return BadRequest(new { message = ex.Message });
    }
  }
  // POST - Accepts the user's puzzle and return a solution - /solve
  // POST - Check if the current user's solution is correct - /check
  // POST - Returns the original puzzle (you can store it in the memory for the next session if needed) - /reset
}