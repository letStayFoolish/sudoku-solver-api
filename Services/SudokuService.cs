using sudoku_solver_api.Models;

namespace sudoku_solver_api.Services;

public class SudokuService : ISudokuService
{
  public int[,] GenerateSudoku(Difficulty difficulty)
  {
    // Hard-coded EASY Sudoku puzzle with zeros representing empty cells
    var newGrid = _GenerateDefaultGrid();
    var easyPuzzle = new int[,]
    {
      { 4, 0, 0, 3, 5, 0, 2, 8, 0 },
      { 8, 0, 0, 6, 7, 2, 0, 0, 0 },
      { 0, 0, 7, 0, 0, 4, 1, 0, 3 },
      { 0, 0, 0, 0, 2, 8, 0, 7, 0 },
      { 0, 0, 7, 3, 0, 0, 2, 0, 4 },
      { 0, 0, 0, 4, 0, 0, 9, 1, 6 },
      { 0, 9, 2, 8, 0, 0, 7, 3, 0 },
      { 4, 0, 5, 7, 6, 3, 0, 0, 0 },
      { 0, 3, 0, 0, 0, 9, 0, 5, 1 }
    };
    // Here you'd generate a puzzle based on the `difficulty`.
    // But for now, we return the hard-coded grid for testing.
    return easyPuzzle;
  }

  private int[,] _GenerateDefaultGrid()
  {
    return new int[9, 9]; // 9x9 with zeros
  }

  public bool SolvePuzzle(int[,] grid)
  {
    // find first cell with 0:
    for (var row = 0; row < 9; row++)
    {
      for (var col = 0; col < 9; col++)
      {
        int[] cellIndex = [row, col];
        
        if (grid[row, col] == 0)
        {
          // do something...
          for (int i = 1; i <= 9; i++)
          {
            // check if i is already in column

            // check if i is already in row

            // check if i is already in section
          }
        }
      }
    }
    
    throw new NotImplementedException();
  }

  public bool IsValidSolution(int[,] originalGrid, int[,] userGrid)
  {
    throw new NotImplementedException();
  }
}