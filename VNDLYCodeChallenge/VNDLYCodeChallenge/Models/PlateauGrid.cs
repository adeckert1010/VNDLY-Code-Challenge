using System;
using System.Collections.Generic;
using System.Text;

namespace VNDLYCodeChallenge.Models
{
    public class PlateauGrid
    {
        public char[,] GridMatrix { get; set; }


        public void InitializeGrid<T>(char[,] matrix)
        {
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    //puts the '0' character at each position of the matrix
                    matrix[i, j] = (char)48;
                }
            }
        }

        
        
        public void Print2DGrid(char[,] matrix)
        {

            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = matrix.GetLength(1) - 1; j >= 0; j--)
                {
                    
                    Console.SetCursorPosition(i, j);
                    Console.Write(matrix[i, j]);

                }

            }

            Console.SetCursorPosition(0, matrix.GetLength(1));
        }
    }
}
