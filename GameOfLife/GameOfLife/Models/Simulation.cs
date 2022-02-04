using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameOfLife.Models
{
    public class Simulation
    {
        // class fields
        private int[,] Board { get; set; }
        private int _height { get; set; }
        private int _width { get; set; }
        private int _numGenerations { get; set; }
        private int _currentGeneration { get; set; }

        // class Properties
        public int CurrentGeneration
        {
            get { return _currentGeneration; }
        }
        public int MaxGenerations
        {
            get { return _numGenerations; }
        }

        public Simulation(int height, int width, int numGenerations)
        {
            _height = height;
            _width = width;
            _numGenerations = numGenerations;
            _currentGeneration = 0;
            Board = new int[height, width];
            var random = new Random();
            for (int i = 0; i < height; i++)
            {
                for (int x = 0; x < width; x++)
                {
                    Board[i, x] = random.Next(0, 2);
                }
            }
        }

        // Return a String that contains a printout of the board
        public string PrintBoard()
        {
            // Set up strings for the borders and empty spaces within the baord
            StringBuilder board = new StringBuilder();
            string topper = "";
            for(int i = 0; i < _width; i++)
            {
                topper += "------";
            }

            string topInnerRow = "";
            for (int i = 0; i < (_width - 1); i++)
            {
                topInnerRow += "|     ";
            }
            topInnerRow += "|    |";
            board.AppendLine(topper);

            // Generate each row of the board
            for (int y = 0; y < _height; y++)
            {
                string row = "";
                for(int x = 0; x < _width; x++)
                {
                    if (x < (_width - 1))
                        row += "|  " + Board[y, x] + "  ";
                    else
                        row += "|  " + Board[y, x] + " |";
                }
                board.AppendLine(topInnerRow);
                board.AppendLine(row);
                board.AppendLine(topInnerRow);
                board.AppendLine(topper);
            }

            return board.ToString();
        }

        // Return a count of the numbe of live cells on the board
        public int LiveCellCount()
        {
            int count = 0;
            for(int x = 0; x < _height; x++)
            {
                for(int y = 0; y < _width; y++)
                {
                    if (Board[x, y] == 1)
                        count++;
                }
            }
            return count;
        }

        // return the number of dead cells on the board
        public int DeadCellCount()
        {
            int count = 0;
            for (int x = 0; x < _height; x++)
            {
                for (int y = 0; y < _width; y++)
                {
                    if (Board[x, y] == 0)
                        count++;
                }
            }

            return count;
        }

        // Run the simulation to the next generation
        public void NextGeneration()
        {
            // Rules for easy reference:
            // 1) Any live cell with fewer than two live neighbors dies as if caused by under-population.
            // 2) Any live cell with two or three live neighbors lives on to the next generation.
            // 3) Any live cell with more than three live neighbors dies, as if by over-population.
            // 4) Any dead cell with exactly three live neighbors becomes a live cell, as if by reproduction.
            // Board needs to be updated simulteanously

            // create a new board array to hold the new values
            int[,] newBoard = new int[_height, _width];

            // Cycle through each cell and check the rules
            for(int x = 0; x < _height; x++)
            {
                for(int y = 0; y < _width; y++)
                {
                    int livingNeighbors = 0;
                    //cycle through the values around the current cell
                    #region CornerCheck
                    //Check if we are working on a corner
                    // Upper left
                    if (x == 0 && y == 0)
                    {
                        livingNeighbors += Board[x + 1, y];
                        livingNeighbors += Board[x + 1, y + 1];
                        livingNeighbors += Board[x, y + 1];
                    }
                    // Upper Right
                    else if (x == 0 && y == (_width - 1))
                    {
                        livingNeighbors += Board[x, y - 1];
                        livingNeighbors += Board[x + 1, y - 1];
                        livingNeighbors += Board[x + 1, y];
                    }
                    // Lower Left
                    else if (x == (_height - 1) && y == 0)
                    {
                        livingNeighbors += Board[x - 1, y];
                        livingNeighbors += Board[x, y + 1];
                        livingNeighbors += Board[x - 1, y + 1];
                    }
                    // Lower Right
                    else if (x == (_height - 1) && y == (_width - 1))
                    {
                        livingNeighbors += Board[x - 1, y];
                        livingNeighbors += Board[x - 1, y - 1];
                        livingNeighbors += Board[x, y - 1];
                    }
                    #endregion
                    #region Edges
                    // There are 4 special case arrays - the first array and the last array, y == 0 and when y == _width
                    // First Array
                    else if (x == 0)
                    {
                        for (int a = 0; a <= 1; a++)
                        {
                            for (int b = -1; b <= 1; b++)
                            {
                                if (a == 0 && b == 0)
                                    livingNeighbors += 0;
                                else
                                    livingNeighbors += Board[x + a, y + b];
                            }
                        }
                    }
                    // Last Array
                    else if (x == (_height - 1))
                    {
                        for (int a = -1; a <= 0; a++)
                        {
                            for (int b = -1; b <= 1; b++)
                            {
                                if (a == 0 && b == 0)
                                    livingNeighbors += 0;
                                else
                                    livingNeighbors += Board[x + a, y + b];
                            }
                        }
                    }
                    // y == 0
                    else if(y == 0)
                    {
                        for(int a = -1; a <= 1; a++)
                        {
                            for(int b = 0; b <= 1; b++)
                            {
                                if (a == 0 && b == 0)
                                    livingNeighbors += 0;
                                else
                                    livingNeighbors += Board[x + a, y + b];
                            }
                        }
                    }
                    // y == _width
                    else if(y == (_width - 1))
                    {
                        for(int a = -1; a <= 1; a++)
                        {
                            for(int b = -1; b <= 0; b++)
                            {
                                if (a == 0 && b == 0)
                                    livingNeighbors += 0;
                                else
                                    livingNeighbors += Board[x + a, y + b];
                            }
                        }
                    }
                    #endregion
                    // The middel cells
                    else
                    {
                        for (int a = -1; a <= 1; a++)
                        {
                            for (int b = -1; b <= 1; b++)
                            {
                                if (a == 0 && b == 0)
                                    livingNeighbors += 0;
                                else
                                    livingNeighbors += Board[x + a, y + b];
                            }
                        }
                    }

                    if (livingNeighbors < 2)
                        newBoard[x, y] = 0;
                    else if (Board[x, y] == 0 && livingNeighbors == 3)
                        newBoard[x, y] = 1;
                    else if (livingNeighbors == 2 || livingNeighbors == 3)
                        newBoard[x, y] = Board[x, y];
                    else if (livingNeighbors > 3)
                        newBoard[x, y] = 0;
                    
                }
            }
            Board = newBoard;
            _currentGeneration++;
            return;
        }
    }
}
