using System.ComponentModel.DataAnnotations;

namespace sudoku_solver_api.Models;

public class SudokuGridRequest
{
  [Required]
  [MinLength(9)]
  [MaxLength(9)]
  public int[][]? PuzzleGrid { get; set; }
}