using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameOfLife.Models
{
    public class Simulation
    {
        private int[,] Board { get; set; }
        private int _height { get; set; }
        private int _width { get; set; }
        private int _numGenerations { get; set; }

        public Simulation(int height, int width, int numGenerations)
        {
            _height = height;
            _width = width;
            _numGenerations = numGenerations;
            Board = new int[height, width];
            var random = new Random();
            for (int i = 0; i < height; i++)
            {
                for (int x = 0; x < width; x++)
                {
                    Board[i, x] = random.Next(0, 1);
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
    }
}
