namespace Tetris;

public class Shape
{
    public List<Pixel> PixelsInShape = [];
    public int centerX;
    public int centerY;
    
    public Shape(int x,int y)
    {
        centerX = x;
        centerY = y;
        var pixel1 = new Pixel(x,y-1);
        var pixel2 = new Pixel(x, y-2);
        var pixel3 = new Pixel(x, y);
        var pixel4 = new Pixel(x, y+1);
        
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
            var (aspiringX, aspiringY) = pixel.findRotatedCoordinates(direction, centerX, centerY);
            pixel.X = aspiringX;
            pixel.Y = aspiringY;
        }
    }
    
    
    // calculate the rotation first and then refactor so that movement happens from the center of the shape.
}