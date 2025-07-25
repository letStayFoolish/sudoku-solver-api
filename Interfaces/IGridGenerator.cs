using sudoku_solver_api.Models;

namespace sudoku_solver_api.Interfaces;

public interface IGridGenerator
{
  Task<int[,]> GenerateSolvedGridAsync();
  Task<int[,]> GeneratePuzzleGridAsync(int[,] solvedGrid, Difficulty difficulty);
}