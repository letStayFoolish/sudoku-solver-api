using sudoku_solver_api.Helpers;
using sudoku_solver_api.Models;

namespace sudoku_solver_api.Services;

public class SudokuService : ISudokuService
{
  private readonly ISudokuSolver _solver;
  private readonly ICustomConverter _converter;

  public SudokuService(ISudokuSolver solver, ICustomConverter converter)
  {
    _solver = solver;
    _converter = converter;
  }

  public int[][] NewGame(Difficulty difficulty)
  {
    // Step 1: Generate a fully solved sudogu grid
    var solvedGrid = _GenerateSolvedGrid();
    
    // Step 2: Remove cells based on the difficulty level
    var puzzleGrid = _RemoveCellsForDifficulty(solvedGrid, difficulty);
    
    // Step 3: Ensure puzzle is solvable and only has one solution
    var(isSolvable, _) = _solver.SolvePuzzle(puzzleGrid);
    while (!isSolvable)
    {
      // If not solvable (edge case), generate a new grid
      return NewGame(difficulty);
    }
    
    // Step 4: Convert the multidimensional array to jagged array format
    var puzzleToJaggedArray = _converter.ToJagged(puzzleGrid);
    return puzzleToJaggedArray;
  }

  private int[,] _GenerateSolvedGrid()
  {
    var grid = _GenerateDefaultGrid();

    bool Solve(int row, int col)
    {
      if(row == 9) return true;
      if(col == 9) return Solve(row + 1, 0);
      if (grid[row, col] != 0) return Solve(row, col + 1);

      var random = new Random();
      var numbers = Enumerable.Range(1, 9).OrderBy(_ => random.Next()).ToArray();
      
      foreach (var num in numbers)
      {
        if (_solver.IsSafe(grid, row, col, num))
        {
          grid[row, col] = num;
          if (Solve(row, col + 1)) return true;
          grid[row, col] = 0;
        }
      }

      return false;
    }

    Solve(0, 0);
    return grid;
  }

  private int[,] _RemoveCellsForDifficulty(int[,] grid, Difficulty difficulty)
  {
    // Clone the grid to ensure the original solved grid is untouched
    var puzzleGridClone = (int[,])grid.Clone();
    var random = new Random();
    
    // Determine the number of cells to remove based on difficulty
    var cellsToRemove = difficulty switch
    {
      Difficulty.Easy => 30,
      Difficulty.Medium => 40,
      Difficulty.Hard => 50,
      _ => 35,
    };
    
    // Remove cells randomly while ensuring at least one solution exists
    for (var i = 0; i < cellsToRemove; i++)
    {
      int row, col;
      do
      {
        row = random.Next(0, 9);
        col = random.Next(0, 9);
      } while (puzzleGridClone[row, col] == 0);

      var temp = puzzleGridClone[row, col];
      puzzleGridClone[row, col] = 0;
      
      // Ensure the puzzle remains solvable
      var (isSolvable, _) = _solver.SolvePuzzle(puzzleGridClone);
      if (!isSolvable)
      {
        // Restore the number if the grid becomes unsolvable
        puzzleGridClone[row, col] = temp;
      }
    }

    return puzzleGridClone;
  }
  

  private static int[,] _GenerateDefaultGrid()
  {
    return new int[9, 9]; // 9x9 with (default) zeros values
  }

  public int[][] GetSolution(int[][] grid)
  {
    var multidimensionalGrid = _converter.ToMultidimensional(grid);

    var (isSolvable, solvedGrid) = _solver.SolvePuzzle((multidimensionalGrid));

    return isSolvable ? solvedGrid : throw new Exception("Sudoku is not solvable.");
  }
}