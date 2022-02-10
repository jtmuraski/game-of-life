using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace GameOfLife.Models
{
    public class BoardPosition
    {
        private int _xpos;
        private int _ypos;
        private int _value;
        private int _id;
        [JsonPropertyName("Id")]
        public int Id {
            get { return _id; }
        }
        [JsonPropertyName("XPosition")]
        public int XPosition
        {
            get { return _xpos; }
        }
        [JsonPropertyName("YPosition")]
        public int YPosition
        {
            get { return _ypos; }
        }
        [JsonPropertyName("Value")]
        public int Value
        {
            get { return _value; }
        }

        [JsonConstructor]
        public BoardPosition() { }
        public BoardPosition(int id, int X, int y, int value)
        {
            this._xpos = X;
            this._ypos = y;
            this._value = value;
            this._id = Id;
        }

    }
}
