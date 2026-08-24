namespace Tetris;

public class Pixel(int x, int y)
{
    public const int Width = 30;
    public int X { get; set; } = x;
    public int Y { get; set; } = y;

    public (int, int) findRotatedCoordinates(Direction direction, float centerX, float centerY)
    {
        var xDiff = X - centerX;
        var yDiff = Y - centerY;
        var aspiringXRaw = centerX - yDiff * (int)direction;
        var aspiringYRaw = centerY + xDiff * (int)direction;
        var aspiringX = (int)Math.Round(aspiringXRaw); ;
        var aspiringY = (int)Math.Round(aspiringYRaw);
    
        return (aspiringX, aspiringY);
    }
}
