
public class Rootobject
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int Height { get; set; }
    public int Width { get; set; }
    public int NumGenerations { get; set; }
    public Board[] Board { get; set; }
}

public class Board
{
    public int Id { get; set; }
    public int XPosition { get; set; }
    public int YPosition { get; set; }
    public int Value { get; set; }
}
