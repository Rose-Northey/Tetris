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

    public (int, int) findRotatedCoordinates(Direction direction, float centerX, float centerY, float xOffset, float yOffset)
    {
        var calculatedCenterX = centerX + xOffset;
        var calculatedCenterY = centerY + yOffset;
        var xDiff = X - calculatedCenterX;
        var yDiff = Y - calculatedCenterY;
        var aspiringXRaw = calculatedCenterX - yDiff * (int)direction;
        var aspiringYRaw = calculatedCenterY + xDiff * (int)direction;
        var aspiringX = (int)Math.Round(aspiringXRaw); ;
        var aspiringY = (int)Math.Round(aspiringYRaw);
    
        return (aspiringX, aspiringY);
    }
}
