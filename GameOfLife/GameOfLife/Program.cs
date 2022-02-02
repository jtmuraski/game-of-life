using System;
using System.Drawing;
using Console = Colorful.Console;
using Colorful;
using GameOfLife.Models;

// Author: John Muraski
// Description: This is my take of the Conway Game Of Life as a coding challenge
// Game of Life Rules:
// Cells on the board are represented by 1's and 0's. One represents a live cell, and a 0 represents a dead cell
// Each generation applies the following 4 rules:
// 1) Any live cell with fewer than two live neighbors dies as if caused by under-population.
// 2) Any live cell with two or three live neighbors lives on to the next generation.
// 3) Any live cell with more than three live neighbors dies, as if by over-population.
// 4) Any dead cell with exactly three live neighbors becomes a live cell, as if by reproduction.

namespace GameOfLife
{
    class Program
    {
        static void Main(string[] args)
        {
            bool runApp = true;
            do
            {
                // Display the menu for the application
                Console.WriteAscii("Game Of Life", Color.DarkGreen);
                Console.WriteLine();
                Console.WriteLine("Welcome to the Game Of Life simulation, my take on Conway's Game Of Life simulation");
                Console.WriteLine("Choose from one of the options below to continue");
                Console.WriteLine("1. View Descripiton and Simulation Rules", Color.Blue);
                Console.WriteLine("2. Setup and begin a new simulation", Color.Blue);
                Console.WriteLine("3. Load a previous simulation set up", Color.Blue);
                Console.WriteLine("4. Exit", Color.Red);
                string choice = Console.ReadLine();
                switch(choice)
                {
                    case ("2"):
                        runApp = SetUpSim();
                        break;
                    case ("4"):
                        runApp = false;
                        break;
                        
                }
            } while (runApp);
        }

        private static bool SetUpSim()
        {
            int boardHeight = 0;
            int boardWidth = 0;
            int numGenerations = 0;

            bool validHeight = false;
            while(!validHeight)
            {
                Console.WriteLine("How tall shall the board be?");
                string height = Console.ReadLine();
                validHeight = Int32.TryParse(height, out boardHeight);
                if (!validHeight)
                    Console.WriteLine("You did not enter a valid integer value. Enter a integer value and try again", Color.DarkRed);
            }

            bool validWidth = false;
            while (!validWidth)
            {
                Console.WriteLine("How wide shall the board be?");
                string width = Console.ReadLine();
                validWidth = Int32.TryParse(width, out boardWidth);
                if (!validWidth)
                    Console.WriteLine("You did not enter a valid integer value. Enter a integer value and try again", Color.DarkRed);
            }

            bool validGeneration = false;
            while (!validGeneration)
            {
                Console.WriteLine("How many generations do you want to simulate?");
                string width = Console.ReadLine();
                validGeneration = Int32.TryParse(width, out numGenerations);
                if (!validGeneration)
                    Console.WriteLine("You did not enter a valid integer value. Enter a integer value and try again", Color.DarkRed);
            }

            Console.Clear();
            Console.WriteLine("Generating board...");
            Simulation board = new Simulation(boardHeight, boardWidth, numGenerations);
            if(board == null)
            {
                Console.WriteLine("Sorry, there was an unexpected error that occured. Press enter to return to exit the application");
                return false;
            }
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("Current Generation: " + board.CurrentGeneration);
            Console.WriteLine("Live Cells:" + board.LiveCellCount());
            Console.WriteLine("Dead Cells: " + board.DeadCellCount());
            Console.WriteLine(board.PrintBoard());
            Console.WriteLine("Do you wish to run the simulation with this setup? (Yes / No)  ");
            string answer = Console.ReadLine();
            if (answer.ToLower() == "yes" || answer.ToLower() == "y")
            {
                ExecuteSimulation(board);
                return true;
            }
            else
            {
                return true;
            }                
        }

         public static void ExecuteSimulation(Simulation board)
        {
            bool runSim = true;
            do
            {
                Console.Clear();
                board.NextGeneration();
                Console.WriteLine("Current Generation: " + board.CurrentGeneration);
                Console.WriteLine("Live Cells:" + board.LiveCellCount());
                Console.WriteLine("Dead Cells: " + board.DeadCellCount());
                Console.WriteLine(board.PrintBoard());
                Console.WriteLine("Continue? (Yes/No");
                string answer = Console.ReadLine();
                if (answer.ToLower() == "no" | answer.ToLower() == "n")
                    runSim = false;
            } while (board.CurrentGeneration < board.MaxGenerations || !runSim);
        }
    }
}
