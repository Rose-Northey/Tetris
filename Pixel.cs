using Raylib_cs;

namespace Tetris;

public class Pixel(int x, int y)
{
    public const int Width = 30;
    public Coordinate coordinate = new (x, y);
    public Color color = Color.Blue;

}