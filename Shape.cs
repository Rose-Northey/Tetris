namespace Tetris;

public class Shape
{
    public List<Pixel> PixelsInShape = [];
    private int centerX;
    private int centerY;
    
    public Shape(int x,int y)
    {
        centerX = x;
        centerY = y;
        var pixel1 = new Pixel(x-1,y);
        var pixel2 = new Pixel(x, y);
        var pixel3 = new Pixel(x + 1, y);
        var pixel4 = new Pixel(x + 2, y);
        
        PixelsInShape.AddRange(pixel1,pixel2,pixel3,pixel4);
    }

    public void moveShape(int x, int y)
    {
        centerX += x;
        centerY += y;
        foreach (var pixel in PixelsInShape)
        {
            pixel.X += x;
            pixel.Y += y;

        }
    }
    
    public void rotateShape(Direction direction)
    {
        foreach (var pixel in PixelsInShape)
        {
            var xDiff = pixel.X - centerX;
            var yDiff = pixel.Y - centerY;
            pixel.X = centerX - yDiff*(int)direction;
            pixel.Y = centerY + xDiff*(int)direction;
        }
    }
    
    // calculate the rotation first and then refactor so that movement happens from the center of the shape.
}