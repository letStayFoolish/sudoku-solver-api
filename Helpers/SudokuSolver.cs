﻿using sudoku_solver_api.Interfaces;

namespace sudoku_solver_api.Helpers;

public class SudokuSolver : ISudokuSolver
{
  private readonly ICustomConverter _converter;
  public SudokuSolver(ICustomConverter converter)
  {
    _converter = converter;
  }

  public async Task<(bool isSolvable, int[][] solvedPuzzle)> SolvePuzzleAsync(int[,] defaultGrid)
  {
    // **CPU-intensive** operations
    // **(Non-blocking):**
    return await Task.Run(() =>
    {
      // This runs on a BACKGROUND THREAD (good!)
      var clonedGrid = _Clone2DArray(defaultGrid);
      var isSolvable = _Solve(clonedGrid);// without async await Task.Run(() => {}) code: CPU-intensive work blocks request thread | CPU work happens off the request thread

      return (isSolvable,
        isSolvable ? _converter.ToJagged(clonedGrid) : _converter.ToJagged(defaultGrid));
    });
    // **Benefit:** The request thread is freed up to handle other requests while the CPU-intensive work happens in the background.
  }

  public async Task<bool> IsValidSolutionAsync(int[,] userGrid)
  {
    // **CPU-intensive** operations
    return await Task.Run(() => _Solve(userGrid));
  }
  
  private static int[,] _Clone2DArray(int[,] sourceArray)
  {
    var rows = sourceArray.GetLength(0);
    var columns = sourceArray.GetLength(1);
    var clone = new int[rows, columns];

    for (var i = 0; i < rows; i++)
    {
      for (var j = 0; j < columns; j++)
      {
        clone[i, j] = sourceArray[i, j];
      }
    }

    return clone;
  }
  
  private bool _Solve(int[,] grid)
  {
    // find the first cell with 0:
    for (var row = 0; row < 9; row++)
    {
      for (var col = 0; col < 9; col++)
      {
        if (grid[row, col] != 0) continue;

        for (var num = 1; num <= 9; num++)
        {
          if (!IsSafe(grid, row, col, num)) continue;

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

  public bool IsSafe(int[,] grid, int row, int col, int num)
  {
    // check if num is already in column
    // check if num is already in row
    for (var i = 0; i < 9; i++)
    {
      // if (i == col || i == row) continue;
      if (grid[row, i] == num || grid[i, col] == num)
        return false;
    }

    // check if i is already in the 3x3 subgrid
    int startRow = row - (row % 3), startCol = col - (col % 3);
    for (var i = 0; i < 3; i++)
    {
      for (var j = 0; j < 3; j++)
      {
        // if((startRow + i == row || startCol + j == col)) continue;
        if (grid[startRow + i, startCol + j] == num)
          return false;
      }
    }

    return true;
  }
}