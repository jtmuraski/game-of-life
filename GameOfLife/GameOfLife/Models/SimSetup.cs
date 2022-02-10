using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace GameOfLife.Models
{
    public class SimSetup
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Height { get; set; }
        public int Width { get; set; }
        public int NumGenerations { get; set; }
        [JsonPropertyName("Board")]
        public List<BoardPosition> Board { get; set; }

        [JsonConstructor]
        public SimSetup() { }
        public SimSetup(Simulation board, string name)
        {
            Height = board.Height;
            Width = board.Width;
            NumGenerations = board.NumGenerations;
            Name = name;
            Board = new List<BoardPosition>();
            for (int x = 0; x < Height; x++)
            {
                for (int y = 0; y < Width; y++)
                {
                    Board.Add(new BoardPosition(0, x, y, board.Board[x, y]));
                }
            }
        }
    }
}
