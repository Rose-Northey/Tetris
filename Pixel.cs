using Raylib_cs;

namespace Tetris;

public class Pixel(Coordinate location)
{
    public const int Width = 30;
    public Coordinate coordinate = location;
    public Color color = Color.Blue;

}