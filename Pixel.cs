namespace Tetris;

public class Pixel
{
    public const int Width = 30;
    public int X { get; set; }
    public int Y { get; set; }

    public Pixel(int x, int y)
    {
        X = x;
        Y = y;

    }

    //calculate x and y based on where the center of the shape is
    //DO this within the 
    public (int, int) findRotatedCoordinates(Direction direction, int centerX, int centerY)
    {
        var xDiff = X - centerX;
        var yDiff = Y - centerY;
        var aspiringX = centerX - yDiff * (int)direction;
        var aspiringY = centerY + xDiff * (int)direction;
        return (aspiringX, aspiringY);
    }
}
