namespace sudoku_solver_api.Models;

public class ApiResponse<T>
{
  public T Data { get; set; }
  public string Message { get; set; } = string.Empty;
  public bool Success { get; set; }
}