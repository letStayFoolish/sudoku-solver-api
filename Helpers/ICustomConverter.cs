namespace sudoku_solver_api.Helpers;

public interface ICustomConverter
{
  int[,] ToMultidimensional(int[][] jaggedArray);
  int[][] ToJagged(int[,] multidimensionalArray);
}