using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using VNDLYCodeChallenge.Models;

namespace VNDLYCodeChallenge
{
    public class CLI
    {
        public int[] plateauSize = new int[] { 0, 0 };
        private string inputType;
        private string plateauSizeString;
        public PlateauGrid grid = new PlateauGrid();
        private int maxX;
        private int maxY;

        private int tempCoord1;
        private int tempCoord2;
        private char tempDirection;
        private List<Rover> rovers = new List<Rover>();

        public void Start()
        {
            Console.Clear();
            Console.WriteLine("Enter the upper-right coordinates of the plateu in format '5 5'.");
            plateauSizeString = Console.ReadLine();
            inputType = "userSize";

            CheckInput(plateauSizeString, inputType);
            PrintPlateauGrid(plateauSize);

            //all same functions as text reader but with input
            ChooseStart();
            MoveRovers(rovers);
            DisplayResults();

        }

        public void ChooseStart()
        {
            bool needsResponse = true;
            Rover rover = new Rover();
            do
            {
                Console.WriteLine("Enter starting positions as 'x y D' where x is the row, y is the column and D is the cardinal direction the rover is facing:");
                string userInput = Console.ReadLine();
                try
                {
                    int xStart = int.Parse(userInput.Substring(0, 1));
                    int yStart = int.Parse(userInput.Substring(2, 1));
                    char dStart = char.Parse(userInput.Substring(4, 1).ToUpper());
                    rover.CurrentCoords[0] = xStart;
                    rover.CurrentCoords[1] = yStart;
                    rover.Direction = dStart;

                    needsResponse = AddInstructions(rover);

                }
                catch (Exception)
                {

                    Console.WriteLine("Incorrect format. Try again.");
                }
            } while (needsResponse);

            rovers.Add(rover);
            return;
        }

        public bool AddInstructions(Rover rover)
        {
            bool isInCorrect = false;
            string userInstructions = "";
            do
            {


                Console.WriteLine("Create a list of instructions using only the letters 'L' for rotate left, 'R' for rotate right, and 'M' for move forward: ");
                userInstructions = Console.ReadLine().ToUpper();

                isInCorrect = !CheckInput(userInstructions, "userInstructions");
            } while (isInCorrect);

            rover.Instructions = userInstructions;
            return isInCorrect;
        }

        public void AddPlateuCoordsToMatrix(string inputString)
        {
            plateauSize[0] = int.Parse(inputString.Substring(0, 1));
            plateauSize[1] = int.Parse(inputString.Substring(2, 1));
            return;
        }

        public void GetInfo()
        {
            ReadyData();
            MoveRovers(rovers);
            DisplayResults();
        }

        private void DisplayResults()
        {
            //display output

            Console.WriteLine("Output: ");
            for (int i = 0; i < rovers.Count; i++)
            {
                int[] actualCoords = new int[2];
                int xCoord = rovers[i].CurrentCoords[0];
                int yCoord = rovers[i].CurrentCoords[1];
                char direction = rovers[i].Direction;
                actualCoords[0] = xCoord;
                actualCoords[1] = yCoord;
                Console.WriteLine($"Rover {i}: ");
                Console.WriteLine($"{xCoord} {yCoord} {direction}");
            }

        }

        private void MoveRovers(List<Rover> rovers)
        {

            //read instructions one character at a time

            int cursorLeft = Console.CursorLeft;
            int cursorTop = Console.CursorTop;

            for (int i = 0; i < rovers.Count; i++)
            {
                Rover rover = rovers[i];
                string instructionChars = "";
                string instructions = rover.Instructions;
                int[] coords = rover.CurrentCoords;
                foreach (char c in instructions)
                {
                    Console.Write(instructionChars += c);
                    //Make rover do that move
                    switch (c)
                    {
                        case 'L':
                            rover.RotateLeft();
                            UpdateGrid(rover);
                            Console.SetCursorPosition(cursorLeft, cursorTop);
                            break;
                        case 'R':
                            rover.RotateRight();
                            UpdateGrid(rover);
                            Console.SetCursorPosition(cursorLeft, cursorTop);
                            break;
                        case 'M':

                            rover.PreviousCoords[0] = rover.CurrentCoords[0];
                            rover.PreviousCoords[1] = rover.CurrentCoords[1];
                            rover.Move();
                            UpdateGrid(rover);
                            Console.SetCursorPosition(cursorLeft, cursorTop);
                            break;
                        default:
                            UpdateGrid(rover);
                            Console.SetCursorPosition(cursorLeft, cursorTop);
                            break;
                    }

                    //update the visual

                    Console.ReadLine();

                }
                cursorTop++;
            }
        }

        public void UpdateGrid(Rover rover)
        {
            int roverX = rover.CurrentCoords[0];
            //display the location in a flipped order for better viewing
            int roverY = grid.GridMatrix.GetUpperBound(1) - rover.CurrentCoords[1];
            int prevX = rover.PreviousCoords[0];
            int prevY = grid.GridMatrix.GetUpperBound(1) - rover.PreviousCoords[1];
            //replace previous position with '0'

            grid.GridMatrix[roverX, roverY] = rover.Direction;
            grid.GridMatrix[prevX, prevY] = (char)48;

            Console.SetCursorPosition(0, 0);
            grid.Print2DGrid(grid.GridMatrix);
            return;
        }

        public void ReadyData()
        {
            string fileDirectory = Environment.CurrentDirectory;
            string fileName = "RoverInstructions.txt";
            string fullPathOfFile = Path.Combine(fileDirectory, fileName);
            int roverCount = 0;

            try
            {
                using (StreamReader sr = new StreamReader(fullPathOfFile))
                {
                    string sizeLine = sr.ReadLine();
                    CheckInput(sizeLine, "textSize");
                    PrintPlateauGrid(plateauSize);

                    Console.WriteLine();
                    Console.WriteLine("Your input data:");
                    Console.WriteLine();

                    Console.WriteLine("Top right coordinates: " + sizeLine);
                    Console.WriteLine();

                    while (!sr.EndOfStream)
                    {
                        string roverLocation = sr.ReadLine();
                        CheckInput(roverLocation, "roverLocation");
                        Rover rover = new Rover();
                        roverCount++;
                        rover.CurrentCoords[0] = this.tempCoord1;
                        rover.CurrentCoords[1] = this.tempCoord2;
                        rover.Direction = this.tempDirection;

                        rovers.Add(rover);
                        Console.WriteLine("Rover " + roverCount + "'s starting location: " + roverLocation);

                        string roverInstructions = sr.ReadLine();
                        CheckInput(roverInstructions, "textInstructions");
                        rover.Instructions = roverInstructions;
                        Console.WriteLine("Rover " + roverCount + "'s instructions: " + roverInstructions);
                        Console.WriteLine();

                        Console.ReadLine();
                        int cursorLeft = Console.CursorLeft;
                        int cursorTop = Console.CursorTop;
                        UpdateGrid(rover);
                        Console.SetCursorPosition(cursorLeft, cursorTop);
                    }

                }

                return;
            }
            catch (Exception e)
            {

                Console.WriteLine("The process failed: {0}", e.ToString());
                Console.ReadLine();
                Environment.Exit(0);
            }

        }


        public void PrintPlateauGrid(int[] sizeArr)
        {
            int maxXCoord = sizeArr[0];
            int maxYCoord = sizeArr[1];

            maxX = maxXCoord + 1;
            maxY = maxYCoord + 1;

            grid.GridMatrix = new char[maxX, maxY];
            grid.InitializeGrid<char>(grid.GridMatrix);
            grid.Print2DGrid(grid.GridMatrix);
            return;
        }

        public bool CheckInput(string inputString, string inputType)
        {
            inputString = inputString.ToUpper();
            switch (inputType)
            {
                case "userSize":
                    var r = new Regex("^[0-9]\\s[0-9]$");
                    if (r.IsMatch(inputString))
                    {
                        AddPlateuCoordsToMatrix(inputString);
                    }
                    else
                    {
                        plateauSize[0] = 0;
                        plateauSize[1] = 0;
                        Console.Clear();
                        Console.WriteLine("Invalid format, please enter two digits separated by a space.");
                        Console.WriteLine("Returing to start in 3 seconds.");
                        Thread.Sleep(3000);

                        Start();
                    }
                    break;
                case "textSize":
                    r = new Regex("^[0-9]\\s[0-9]$");
                    if (r.IsMatch(inputString))
                    {
                        AddPlateuCoordsToMatrix(inputString);
                        return true;
                    }
                    else
                    {
                        IncorrectTextFormat();
                    }
                    break;

                case "roverLocation":
                    try
                    {
                        string[] inputArr = inputString.Split(" ");
                        tempCoord1 = int.Parse(inputArr[0]);
                        tempCoord2 = int.Parse(inputArr[1]);
                        tempDirection = char.Parse(inputArr[2]);

                    }
                    catch (Exception e)
                    {

                        IncorrectTextFormat();
                    }

                    break;

                case "textInstructions":
                    r = new Regex("^[LMR]+$");
                    if (r.IsMatch(inputString))
                    {

                        return true;
                    }
                    else
                    {
                        IncorrectTextFormat();
                    }

                    break;
                case "userInstructions":
                    r = new Regex("^[LMR]+$");
                    if (r.IsMatch(inputString))
                    {

                        return true;
                    }
                    else
                    {
                        Console.WriteLine("Incorrect format try again");
                        return false;
                    }

                default:
                    break;
            }



            return true;
        }

        private static void IncorrectTextFormat()
        {
            Console.Clear();
            Console.WriteLine("Invalid format, please check that your input file is formatted correctly.");
            Console.ReadLine();
            Environment.Exit(0);
        }
    }
}
