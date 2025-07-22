using sudoku_solver_api.Helpers;
using sudoku_solver_api.Models;

namespace sudoku_solver_api.Services;

public class SudokuService : ISudokuService
{
  public int[,] GenerateSudoku(Difficulty difficulty)
  {
    // Hard-coded EASY Sudoku puzzle with zeros representing empty cells
    // This will be needed in the future to fill random cells with numbers from a previously checked combination
    var newGrid = _GenerateDefaultGrid();
    // if it is Easy, e.g. 25 cells we should fill with real value (from 1 to 9) from a solvable combination!
    // For now, we use a random Easy starter combination from the internet
    var easyPuzzle = new int[,]
    {
      { 0, 0, 0, 8, 0, 0, 0, 0, 9 },
      { 8, 0, 7, 0, 9, 0, 0, 3, 0 },
      { 0, 2, 0, 0, 0, 6, 8, 0, 0 },
      { 5, 0, 0, 0, 4, 3, 6, 0, 0 },
      { 0, 4, 0, 6, 5, 0, 0, 1, 0 },
      { 0, 0, 3, 2, 0, 0, 0, 0, 4 },
      { 0, 0, 1, 4, 0, 0, 0, 2, 0 },
      { 0, 8, 0, 0, 6, 0, 4, 0, 1 },
      { 7, 0, 4, 0, 0, 1, 0, 0, 0 }
    };
    // Here you'd generate a puzzle based on the `difficulty`.
    // But for now, we return the hard-coded grid for testing.
    var (isSolvable, solvedGrid) = SolvePuzzle(easyPuzzle);
    
    return isSolvable ? easyPuzzle : newGrid;
  }

  private static int[,] _GenerateDefaultGrid()
  {
    return new int[9, 9]; // 9x9 with zeros
  }

  private static int[,] _Clone2DArray(int[,] sourceArray)
  {
    var rows = sourceArray.GetLength(0);
    var columns = sourceArray.GetLength(1);
    int[,] clone = new int[rows, columns];
    for (var i = 0; i < rows; i++)
    {
      for (var j = 0; j < columns; j++)
      {
        clone[i, j] = sourceArray[i, j];
      }
    }

    return clone;
  }

  public (bool isSolavble, int[][]?) SolvePuzzle(int[,] defaultGrid)
  {
    var clonedGrid = _Clone2DArray(defaultGrid);
    var isSolavble = _Solve(clonedGrid);

    return (isSolavble, isSolavble ? ArrayConverter.ToJagged(clonedGrid) : null);
  }

  private static bool _Solve(int[,] grid)
  {
    // find the first cell with 0:
    for (var row = 0; row < 9; row++)
    {
      for (var col = 0; col < 9; col++)
      {
        if (grid[row, col] != 0) continue;

        for (var num = 1; num <= 9; num++)
        {
          if (!_IsSafe(grid, row, col, num)) continue;

          // if all of conditions from above are satisfied, place a number and keep going through the grid
          grid[row, col] = num;

          // Recursive call to solve the next cell
          if (_Solve(grid))
          {
            return true;
          }

          // Reset the cell if the number doesn't lead to a solution
          grid[row, col] = 0;
        }

        // If no number is valid in this cell, backtrack
        return false;
      }
    }

    return true; // Puzzle is fully solved
  }

  private static bool _IsSafe(int[,] grid, int row, int col, int num)
  {
    // check if num is already in column
    // check if num is already in row
    for (var i = 0; i < 9; i++)
    {
      if (grid[row, i] == num || grid[i, col] == num)
        return false;
    }

    // check if i is already in the 3x3 subgrid
    int startRow = row - (row % 3), startCol = col - (col % 3);
    for (var i = 0; i < 3; i++)
    {
      for (var j = 0; j < 3; j++)
      {
        if (grid[startRow + i, startCol + j] == num)
          return false;
      }
    }

    return true;
  }

  public bool IsValidSolution(int[,] userGrid)
  {
    return _Solve(userGrid);
  }

  public int[][] GetSolution(int[][] grid)
  {
    var multidimensionalGrid = ArrayConverter.ToMultidimensional(grid);
    
    var (isSolvable, solvedGrid) = SolvePuzzle((multidimensionalGrid));
    
    return isSolvable ? solvedGrid : new int[9][];
  }
}