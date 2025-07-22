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
    return SolvePuzzle(easyPuzzle) ? easyPuzzle : newGrid;
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
        if (grid[row, col] == 0)
        {
          // do something...
          for (int num = 1; num <= 9; num++)
          {
            if (_IsSafe(grid, row, col, num))
            {
              // if all of conditions from above are satisfied, place a number and keep going through the grid
              grid[row, col] = num;

              // Recursive call to solve the next cell
              if (SolvePuzzle(grid))
                return true;
              // Reset cell if no solution is found
              grid[row, col] = 0;
            }
          }

          return false;
        }
      }
    }

    return true; // Puzzle is fully solved
  }

  private bool _IsSafe(int[,] grid, int row, int col, int num)
  {
    // check if num is already in column
    // check if i is already in row
    for (int i = 0; i < 9; i++)
    {
      if (grid[row, i] == num || grid[i, col] == num)
        return false;
    }

    // check if i is already in the 3x3 subgrid
    int startRow = row - (row % 3), startCol = col - (col % 3);
    for (int i = 0; i < 3; i++)
    {
      for (int j = 0; j < 3; j++)
      {
        if (grid[startRow + i, startCol + i] == num)
          return false;
      }
    }

    return true;
  }

  public bool IsValidSolution(int[,] originalGrid, int[,] userGrid)
  {
    throw new NotImplementedException();
  }
}