using sudoku_solver_api.Models;

namespace sudoku_solver_api.Services;

public interface ISudokuService
{
  int[,] GenerateSudoku(Difficulty difficulty);
  bool SolvePuzzle(int[,] puzzleGrid);
  bool IsValidSolution(int[,] originalGrid, int[,] userGrid);
}