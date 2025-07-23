namespace sudoku_solver_api.Helpers;

public interface ISudokuSolver
{
  (bool isSolvable, int[][] solvedPuzzle) SolvePuzzle(int[,] grid);
}