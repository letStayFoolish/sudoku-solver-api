namespace sudoku_solver_api.Interfaces;

public interface ISudokuSolver
{
  Task<(bool isSolvable, int[][] solvedPuzzle)> SolvePuzzleAsync(int[,] grid);
  bool IsSafe(int[,] grid, int row, int col, int num);
  Task<bool> IsValidSolutionAsync(int[,] userGrid);
}