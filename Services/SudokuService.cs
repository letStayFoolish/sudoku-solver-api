using sudoku_solver_api.Helpers;
using sudoku_solver_api.Models;

namespace sudoku_solver_api.Services;

public class SudokuService : ISudokuService
{
private readonly ISudokuSolver _solver;
  public SudokuService(ISudokuSolver solver)
  {
    _solver = solver;
  }
  
  public int[][] GenerateSudoku(Difficulty difficulty)
  {
    // Hard-coded EASY Sudoku puzzle with zeros representing empty cells
    // This will be needed in the future to fill random cells with numbers from a previously checked combination
    var newGrid = _GenerateDefaultGrid();
    // If it is Easy, e.g., 25 cells we should fill with real value (from 1 to 9) from a solvable combination!
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
    var (isSolvable, solvedGrid) = _solver.SolvePuzzle(easyPuzzle);
    var generatedPuzzleToJaggedArr = ArrayConverter.ToJagged(easyPuzzle);
    var defaultPuzzleToJaggedArr = ArrayConverter.ToJagged(newGrid);
    return isSolvable ? generatedPuzzleToJaggedArr : defaultPuzzleToJaggedArr;
  }

  private static int[,] _GenerateDefaultGrid()
  {
    return new int[9, 9]; // 9x9 with (default) zeros values
  }

  public int[][] GetSolution(int[][] grid)
  {
    var multidimensionalGrid = ArrayConverter.ToMultidimensional(grid);
    
    var (isSolvable, solvedGrid) = _solver.SolvePuzzle((multidimensionalGrid));
    
    return isSolvable ? solvedGrid! : throw new Exception("Sudoku is not solvable.");
  }
}