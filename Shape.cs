namespace Tetris;

public class Shape
{
    public List<Pixel> PixelsInShape = [];

    public Shape(int x,int y)
    {
        var pixel1 = new Pixel(x, y);
        var pixel2 = new Pixel(x + 1, y);
        PixelsInShape.AddRange(pixel1,pixel2);
    }

    public void moveShape(int x, int y)
    {
        foreach (var pixel in PixelsInShape)
        {
            pixel.X += x;
            pixel.Y += y;

        }
    }
}