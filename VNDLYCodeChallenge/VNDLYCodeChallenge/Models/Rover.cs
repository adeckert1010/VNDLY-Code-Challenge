using System;
using System.Collections.Generic;
using System.Text;

namespace VNDLYCodeChallenge.Models
{
    public class Rover
    {

        public char Direction { get; set; }
        public int[] CurrentCoords { get; set; } = new int[2];
        public int[] PreviousCoords { get; set; } = new int[2];
        public string Instructions { get; set; }

        public void RotateLeft()
        {
            switch (Direction)
            {
                case 'N':
                    Direction = 'W';
                    break;
                case 'W':
                    Direction = 'S';
                    break;
                case 'S':
                    Direction = 'E';
                    break;
                case 'E':
                    Direction = 'N';
                    break;
                default:
                    break;
            }
        } 

        public void RotateRight()
        {
            switch (Direction)
            {
                case 'N':
                    Direction = 'E';
                    break;
                case 'W':
                    Direction = 'N';
                    break;
                case 'S':
                    Direction = 'W';
                    break;
                case 'E':
                    Direction = 'S';
                    break;
                default:
                    break;
            }
        }
        public void Move() 
        {
            switch (Direction)
            {
                case 'N':
                    PreviousCoords[1] = CurrentCoords[1];
                    CurrentCoords[1] += 1;
                    break;
                case 'S':
                    PreviousCoords[1] = CurrentCoords[1];
                    CurrentCoords[1] -= 1;
                    break;
                case 'E':
                    PreviousCoords[0] = CurrentCoords[0];
                    CurrentCoords[0] += 1;
                    break;
                case 'W':
                    PreviousCoords[0] = CurrentCoords[0];
                    CurrentCoords[0] -= 1;
                    break;

                default:
                    break;
            }
        }

        //private char SetCardinalDirection(string directionTurned)
        //{
        //    switch (directionTurned)
        //    {

        //        default:
        //            break;
        //    }
        //}
    }
}
