using System.Text.Json;
using Microsoft.Extensions.Caching.Memory;
using sudoku_solver_api.Interfaces;
using sudoku_solver_api.Models;

namespace sudoku_solver_api.Services;

public class SudokuService : ISudokuService
{
  private readonly IGridGenerator _gridGenerator;
  private readonly ISudokuSolver _solver;
  private readonly ICustomConverter _converter;
  private readonly IMemoryCache _cache;
  private readonly ILogger<SudokuService> _logger;

  public SudokuService(IGridGenerator gridGenerator, ISudokuSolver solver, ICustomConverter converter, IMemoryCache cache, ILogger<SudokuService> logger)
  {
    _gridGenerator = gridGenerator;
    _solver = solver;
    _converter = converter;
    _cache = cache;
    _logger = logger;
  }

  public async Task<int[][]> NewGameAsync(Difficulty difficulty)
  {
    // Step 0: Use in-memory caching to store previously computed grids and solutions.
    string cacheKey = $"Sudoku_{difficulty}";

    _logger.LogInformation("SudokuService:Generating new sudoku puzzle with difficulty: {Difficulty}", difficulty);

    try
    {
      if (_cache.TryGetValue(cacheKey, out var cachedPuzzle))
      {
        _logger.LogInformation("SudokuService: Retrieved puzzle from cache for difficulty: {Difficulty}", difficulty);
        return cachedPuzzle as int[][];
      }

      // Step 1: Generate a fully solved sudogu grid
      var solvedGrid = await _gridGenerator.GenerateSolvedGridAsync();
      // Step 2: Remove cells based on the difficulty level
      var puzzleGrid = await _gridGenerator.GeneratePuzzleGridAsync(solvedGrid, difficulty);
      // Step 3: Convert the multidimensional array to jagged array format
      var jaggedPuzzle = _converter.ToJagged(puzzleGrid);

      _cache.Set(cacheKey, jaggedPuzzle, TimeSpan.FromSeconds(2)); // Cache for 2 seconds

      _logger.LogInformation("SudokuService: Generated and cached new puzzle for difficulty: {Difficulty}", difficulty);
      return jaggedPuzzle;
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "SudokuService: Error generating new puzzle for difficulty: {Difficulty}", difficulty);
      throw;
    }
  }
  
  public async Task<int[][]> GetSolutionAsync(int[][] grid)
  {
    var serializedGrid = JsonSerializer.Serialize(grid);
    string cacheKey = $"Solution_ID{serializedGrid.GetHashCode()}";
    
    _logger.LogInformation("SudokuService: Cache key created: {cacheKey}", cacheKey);
    _logger.LogInformation("SudokuService: Solving Sudoku Puzzle");

    try
    {
      if (_cache.TryGetValue(cacheKey, out int[][] cachedSolution))
      {
        _logger.LogInformation("SudokuService: Retrieved solution from cache");
        return cachedSolution;
      }
    
      var multidimensionalGrid = _converter.ToMultidimensional(grid);
      var (isSolvable, solvedGrid) = await _solver.SolvePuzzleAsync((multidimensionalGrid));

      if (!isSolvable)
      {
        _logger.LogWarning("SudokuService: Sudoku puzzle is not solvable");
        throw new InvalidOperationException("Sudoku is not solvable.");
      }

      _cache.Set(cacheKey, solvedGrid, TimeSpan.FromMinutes(10)); // Cache solution for 10 minutes
      
      _logger.LogInformation("SudokuService: Solved and cached sudoku puzzle");
      return solvedGrid;
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "SudokuService: Error solving sudoku puzzle");
      throw;
    }
  }
}