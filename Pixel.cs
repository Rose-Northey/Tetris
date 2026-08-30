using Raylib_cs;

namespace Tetris;

public class Pixel(int x, int y)
{
    public const int Width = 30;
    public int X { get; set; } = x;
    public int Y { get; set; } = y;

    public Color color = Color.Blue;

    public float hue;
    public float saturation;
    public float value;


}
