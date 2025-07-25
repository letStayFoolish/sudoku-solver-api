namespace sudoku_solver_api.Interfaces;

public interface ICustomConverter
{
  int[,] ToMultidimensional(int[][] jaggedArray);
  int[][] ToJagged(int[,] multidimensionalArray);
}