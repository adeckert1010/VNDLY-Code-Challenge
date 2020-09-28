using System;
using System.Numerics;
using System.Security.Cryptography.X509Certificates;

namespace VNDLYCodeChallenge
{
    class Program
    {
        static void Main(string[] args)
        {
            CLI cli = new CLI();
            //first decide whether info will come from text file or user input
            bool needAnswer = true;
            do
            {
                string userInput;

                Console.WriteLine("Enter '1' to read from the 'RoverInstructions.txt' file.");
                Console.WriteLine("Enter '2' to send instructions.");
                Console.WriteLine("Enter 'q' to quit.");
                userInput = Console.ReadLine().ToLower();

                switch (userInput)
                {
                    //if text file, start reading the file
                    case "1":
                        Console.Clear();
                        cli.GetInfo();
                        needAnswer = false;
                        break;
                    //if user input, ready user input
                    case "2":
                        Console.Clear();
                        cli.Start();
                        needAnswer = false;
                        break;
                    case "q":
                        Environment.Exit(0);
                        needAnswer = false;
                        break;

                    default:
                        break;
                }


            } while (needAnswer);

            Console.ReadLine();
        }
    }
}
