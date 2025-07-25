using sudoku_solver_api.Constants;
using sudoku_solver_api.Interfaces;
using sudoku_solver_api.Models;

namespace sudoku_solver_api.Services;

public class GridGenerator : IGridGenerator
{
  private readonly ISudokuSolver _solver;
  private readonly Dictionary<Difficulty, int> _cellsToRemove = new()
  {
    { Difficulty.Easy, 30},
    { Difficulty.Medium, 40 },
    { Difficulty.Hard, 50 },
    { Difficulty.Extreme, 65 }
  };

  public GridGenerator(ISudokuSolver solver)
  {
    _solver = solver;
  }

  public async Task<int[,]> GenerateSolvedGridAsync()
  {
    return await Task.Run(() =>
    {
      var grid = _GenerateEmptyGrid();

      bool Solve(int row, int col)
      {
        if (row == SudokuConstants.GridSize) return true;
        if (col == SudokuConstants.GridSize) return Solve(row + 1, 0);
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
    });
  }

  public async Task<int[,]> GeneratePuzzleGridAsync(int[,] solvedGrid, Difficulty difficulty)
  {
    return await Task.Run(async () =>
    {
      // Clone the grid to ensure the original solved grid is untouched
      var puzzle = (int[,])solvedGrid.Clone();
      var random = new Random();

      // Determine the number of cells to remove based on difficulty
      _cellsToRemove.TryGetValue(difficulty, out var cellsToRemove);
      
      // Remove cells randomly while ensuring at least one solution exists
      for (var i = 0; i < cellsToRemove; i++)
      {
        int row, col;
        do
        {
          row = random.Next(0, SudokuConstants.GridSize);
          col = random.Next(0, SudokuConstants.GridSize);
        } while (puzzle[row, col] == 0);

        var temp = puzzle[row, col];
        puzzle[row, col] = 0;

        // Ensure the puzzle remains solvable
        var (isSolvable, _) = await _solver.SolvePuzzleAsync(puzzle);
        if (!isSolvable)
        {
          // Restore the number if the grid becomes unsolvable
          puzzle[row, col] = temp;
        }
      }

      return puzzle;
    });
  }

  private static int[,] _GenerateEmptyGrid() =>
    new int[SudokuConstants.GridSize, SudokuConstants.GridSize]; // 9x9 with (default) zeros values
}