namespace Tetris;

public class Pixel
{
    public int Width = 30;
    public int X { get; set; } 
    public int Y { get; set; }
    
    public Pixel(int x, int y)
    {
        X = x;
        Y = y;
    }
}