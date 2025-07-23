using sudoku_solver_api.Models;

namespace sudoku_solver_api.Services;

public interface ISudokuService
{
  int[][] GenerateSudoku(Difficulty difficulty);
  // (bool isSolavble, int[][]? solvedGrid) SolvePuzzle(int[,] puzzleGrid);
  // bool IsValidSolution(int[,] userGrid);
  int[][] GetSolution(int[][] grid);
  // POST combination to be solved...
}