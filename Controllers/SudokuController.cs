using Microsoft.AspNetCore.Mvc;
using sudoku_solver_api.Interfaces;
using sudoku_solver_api.Models;

namespace sudoku_solver_api.Controllers;

[ApiController]
[Route("api/sudoku")]
public class SudokuController : ControllerBase
{
  private readonly ISudokuService _sudokuService;
  private readonly ILogger<SudokuController> _logger;

  public SudokuController(ISudokuService sudokuService, ILogger<SudokuController> logger)
  {
    _sudokuService = sudokuService;
    _logger = logger;
  }

  // GET - Generates new sudoku puzzle - /generate?difficulty=easy
  [HttpGet]
  [Route("generate")]
  public async Task<ActionResult<ApiResponse<int[][]>>> GenerateSudoku(
    [FromQuery] Difficulty difficulty = Difficulty.Hard)
  {
    try
    {
      _logger.LogInformation("Generating sudoku puzzle with difficulty: {Difficulty}", difficulty);

      var generatedGrid = await _sudokuService.NewGameAsync(difficulty);
      // return Ok(new { newPuzzle = generatedGrid, message = "Sudoku puzzle generated successfully.", status = StatusCode(200), solvable = true  });
      return Ok(new ApiResponse<int[][]>
      {
        Data = generatedGrid,
        Message = "Sudoku puzzle generated successfully.",
        Success = true,
      });
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "Error generating sudoku puzzle");

      return BadRequest(new ApiResponse<int[][]>
      {
        Message = ex.Message,
        Success = false,
      });
    }
  }

  [HttpPost]
  [Route("solve")]
  public async Task<ActionResult<ApiResponse<int[][]>>> SolvePuzzleAsync([FromBody] SudokuGridRequest puzzleRequest)
  {
    // don't need to check `ModelState.IsValid` in each controller action
    // We added config.Filters.Add<ValidationFilter>(); within Program.cs
    // if (!ModelState.IsValid)
    // {
    //   return BadRequest(new ApiResponse<int[][]>
    //   {
    //     Message = "Invalid puzzle grid.",
    //     Success = false,
    //   });
    // }

    try
    {
      _logger.LogInformation("Solving sudoku puzzle");

      // Convert jagged array to multidimensional array
      var solution = await _sudokuService.GetSolutionAsync(puzzleRequest.PuzzleGrid!);
      
      return Ok(new ApiResponse<int[][]>
      {
        Data = solution,
        Message = "Sudoku puzzle solved successfully.",
        Success = true,
      });
    }
    catch (Exception ex)
    {
      _logger.LogInformation(ex, "Error solving sudoku puzzle.");
      
      return BadRequest(new ApiResponse<int[][]>
      {
        Message = ex.Message,
        Success = false,
      });
    }
  }
  // POST - Accepts the user's puzzle and return a solution - /solve
  // POST - Check if the current user's solution is correct - /check
  // POST - Returns the original puzzle (you can store it in the memory for the next session if needed) - /reset
}