using sudoku_solver_api.Models;

namespace sudoku_solver_api.Interfaces;

public interface ISudokuService
{
  Task<int[][]> NewGameAsync(Difficulty difficulty);
  Task<int[][]> GetSolutionAsync(int[][] grid);
  // POST combination to be solved...
}