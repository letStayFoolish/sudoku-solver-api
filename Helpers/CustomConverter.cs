using sudoku_solver_api.Interfaces;

namespace sudoku_solver_api.Helpers;

public class CustomConverter : ICustomConverter
{
  public int[,] ToMultidimensional(int[][] jaggedArray)
  {
    if (jaggedArray == null || jaggedArray.Length == 0)
    {
      throw new ArgumentException("The jagged array cannot be null or empty.", nameof(jaggedArray));
    }

    var rows = jaggedArray.Length;
    var columns = jaggedArray[0]?.Length ?? throw new ArgumentException("The jagged array rows cannot be null.", nameof(jaggedArray));
    int[,] multidimensionalArray = new int[rows, columns];

    // iterating through the rows
    for (var i = 0; i < rows; i++)
    {
      // iterating through the columns
      for (var j = 0; j < columns; j++)
      {
        multidimensionalArray[i, j] = jaggedArray[i][j];
      }
    }

    return multidimensionalArray;
  }

  public int[][] ToJagged(int[,] multidimensionalArray)
  {
    if (multidimensionalArray == null)
    {
      throw new ArgumentNullException(nameof(multidimensionalArray), "The multidimensional array cannot be null.");
    }
    
    var rows = multidimensionalArray.GetLength(0);
    var columns = multidimensionalArray.GetLength(1);
    int[][] jaggedArray = new int[rows][];

    for (var i = 0; i < rows; i++)
    {
      // Jagged arrays rows are allocated (created) separately, so for each row we have to set columns (count)
      jaggedArray[i] = new int[columns];

      for (var j = 0; j < columns; j++)
      {
        jaggedArray[i][j] = multidimensionalArray[i, j];
      }
    }

    return jaggedArray;
  }
}