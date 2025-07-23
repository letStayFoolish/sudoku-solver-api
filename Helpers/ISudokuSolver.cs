namespace sudoku_solver_api.Helpers;

public interface ISudokuSolver
{
  (bool isSolvable, int[][] solvedPuzzle) SolvePuzzle(int[,] grid);
  bool IsSafe(int[,] grid, int row, int col, int num);


}