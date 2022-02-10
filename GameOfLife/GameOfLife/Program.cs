using System;
using System.Drawing;
using Console = Colorful.Console;
using Colorful;
using GameOfLife.Models;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

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
                Console.Clear();
                Console.WriteAscii("Game Of Life", Color.DarkGreen);
                Console.WriteLine();
                Console.WriteLine("Welcome to the Game Of Life simulation, my take on Conway's Game Of Life simulation");
                Console.WriteLine("Choose from one of the options below to continue");
                Console.WriteLine("1. View Descripiton and Simulation Rules", Color.Blue);
                Console.WriteLine("2. Setup and begin a new simulation", Color.Blue);
                Console.WriteLine("3. Load previous saved setup", Color.Blue);
                Console.WriteLine("4. Exit", Color.Red);
                string choice = Console.ReadLine();
                switch(choice)
                {
                    case ("1"):
                        DisplayInstructions();
                        break;
                    case ("2"):
                        runApp = SetUpSim();
                        break;
                    case ("3"):
                        runApp = UseLastSetup();
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
            // Stylesheet for console colors
            StyleSheet styleSheet = new StyleSheet(Color.Green);
            styleSheet.AddStyle("0", Color.Red);
            styleSheet.AddStyle("1", Color.Blue);
            Console.WriteLineStyled(board.PrintBoard(), styleSheet);
            Console.WriteLine();
            Console.WriteLine("Do you wish to save these set up options and starting board (Yes / No)?");
            bool validSave = false;
            while(!validSave)
            {
                string save = Console.ReadLine();
                if (save.ToLower() == "yes" || save.ToLower() == "y")
                {
                    bool saveSuccess = SaveSetup(board);
                    if (saveSuccess)
                    {
                        Console.WriteLine("Setup saved!");
                        validSave = true;
                    }
                    else
                    {
                        Console.WriteLine("Unfortunatly the setup was unable to be saved. Please exit and try again, or continue the simulation. Press any key to continue.");
                        Console.ReadLine();
                        validSave = true;
                    }
                }
                else if (save.ToLower() == "no" || save.ToLower() == "n")
                    validSave = true;
                else
                    Console.WriteLine("You did not enter a valid choice. PLease enter either yes or no");                   

            }
            
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
                // Stylesheet for console colors
                StyleSheet styleSheet = new StyleSheet(Color.Green);
                styleSheet.AddStyle("0", Color.Red);
                styleSheet.AddStyle("1", Color.Blue);
                Console.WriteLineStyled(board.PrintBoard(), styleSheet);
                if(board.CurrentGeneration == board.NumGenerations)
                {
                    Console.WriteLine("The Simulation is complete. Press any key to return to the main menu");
                    Console.ReadLine();
                    runSim = false;
                }
                else
                {
                    Console.WriteLine("Continue? (Yes/No");
                    string answer = Console.ReadLine();
                    if (answer.ToLower() == "no" | answer.ToLower() == "n")
                    {
                        Console.WriteLine("Exiting this simulation run...");
                        Console.WriteLine("Press enter to return to the main menu");
                        runSim = false;
                    }
                }  
            } while (runSim);
            return;
        }
        public static void DisplayInstructions()
        {
            Console.Clear();
            string[] doc = File.ReadAllLines("..\\..\\..\\Instructions.txt"); // Need to figure out the relative path for this
            int count = 0;
            foreach(string line in doc)
            {
                if(count == 0)
                {
                    Console.WriteAscii(line, Color.Green);
                }
                else if (line == "Instructions")
                    Console.WriteLine(line, Color.Cyan);
                else
                    Console.WriteLine(line);

                count++;
            }
            Console.WriteLine();
            Console.WriteLine("Press any key to return to the main menu");
            Console.ReadLine();
            return;
        }
        public static bool SaveSetup(Simulation board)
        {
            Console.WriteLine("PLease enter a name for this Simulation Board: ");
            string name = Console.ReadLine();
            Console.WriteLine("Saving board " + name + "...");
            var options = new JsonSerializerOptions { WriteIndented = true };
            SimSetup setup = new SimSetup(board, name);
            string json = JsonSerializer.Serialize<SimSetup>(setup, options);
            string fileName = "..\\..\\..\\BoardSetup.json";
            File.WriteAllText(fileName, json);
            return true;
        }
        public static bool UseLastSetup()
        {
            Console.Clear();
            string fileName = "..\\..\\..\\BoardSetup.json";
            string jsonString = File.ReadAllText(fileName);
            var previousSetup = JsonSerializer.Deserialize<Rootobject>(jsonString);
            Console.WriteLine("Loading previous setup...");
            Console.WriteLine("Generating board...");
            var board = new Simulation(previousSetup);
            if (board == null)
            {
                Console.WriteLine("Sorry, there was an unexpected error that occured. Press enter to return to exit the application");
                return false;
            }
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("Current Generation: " + board.CurrentGeneration);
            Console.WriteLine("Generation to Simulate: " + board.NumGenerations);
            Console.WriteLine("Live Cells:" + board.LiveCellCount());
            Console.WriteLine("Dead Cells: " + board.DeadCellCount());
            // Stylesheet for console colors
            StyleSheet styleSheet = new StyleSheet(Color.Green);
            styleSheet.AddStyle("0", Color.Red);
            styleSheet.AddStyle("1", Color.Blue);
            Console.WriteLineStyled(board.PrintBoard(), styleSheet);
            Console.WriteLine();
            Console.WriteLine("Do you wish to run the simulation with this setup? (Yes / No)  ");
            string simulate = Console.ReadLine();
            if (simulate.ToLower() == "yes" || simulate.ToLower() == "y")
            {
                ExecuteSimulation(board);
                return true;
            }
            else
            {
                return true;
            }
        }
    }
}
