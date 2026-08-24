namespace Tetris;

public class Shape
{
    public readonly List<Pixel> PixelsInShape = [];
    public int centerX;
    public int centerY;
    private float xOffset;
    private float yOffset;

    public Shape(int x, int y)
    {
        centerX = x;
        centerY = y;
        xOffset = 0;
        yOffset = 0;
        var formRandomShape = GenerateShapeRecipe();
        formRandomShape();
    }

    private Action GenerateShapeRecipe()
    {
        var randomNumber = new Random().Next(0, 7);
        return randomNumber switch
        {
            (0) => IShape,
            (1) => OShape,
            (2) => TShape,
            (3) => SShape,
            (4) => ZShape,
            (5) => LShape,
            _ => JShape
        };
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
            var (aspiringX, aspiringY) = pixel.findRotatedCoordinates(direction, centerX + xOffset, centerY + yOffset);
            pixel.X = aspiringX;
            pixel.Y = aspiringY;
        }
    }
        
    private void IShape()
    {
        var pixel1 = new Pixel(centerX, centerY - 1);
        var pixel2 = new Pixel(centerX, centerY-2);
        var pixel3 = new Pixel(centerX, centerY);
        var pixel4 = new Pixel(centerX, centerY+1);   
        xOffset = -0.5F;
        yOffset = -0.5F;
        PixelsInShape.AddRange(pixel1,pixel2, pixel3,pixel4);
    }
    
    public void OShape()
    {
        var pixel1 = new Pixel(centerX - 1, centerY - 1);
        var pixel2 = new Pixel(centerX, centerY - 1);
        var pixel3 = new Pixel(centerX - 1, centerY);
        var pixel4 = new Pixel(centerX, centerY);
        xOffset = -0.5F;
        yOffset = -0.5F;
        PixelsInShape.AddRange(pixel1, pixel2, pixel3, pixel4);
    }

    private void TShape()
    {
        var pixel1 = new Pixel(centerX, centerY - 1);
        var pixel2 = new Pixel(centerX - 1, centerY);
        var pixel3 = new Pixel(centerX, centerY);
        var pixel4 = new Pixel(centerX + 1, centerY);
        PixelsInShape.AddRange(pixel1, pixel2, pixel3, pixel4);
    }

    private void SShape()
    {
        var pixel1 = new Pixel(centerX-1, centerY);
        var pixel2 = new Pixel(centerX, centerY);
        var pixel3 = new Pixel(centerX, centerY-1);
        var pixel4 = new Pixel(centerX + 1, centerY-1);
        PixelsInShape.AddRange(pixel1, pixel2, pixel3, pixel4);
    }
    
    private void ZShape()
    {
        var pixel1 = new Pixel(centerX-1, centerY-1);
        var pixel2 = new Pixel(centerX, centerY-1);
        var pixel3 = new Pixel(centerX, centerY);
        var pixel4 = new Pixel(centerX+1, centerY);
        PixelsInShape.AddRange(pixel1, pixel2, pixel3, pixel4);
    }
    
    
    private void LShape()
    {
        var pixel1 = new Pixel(centerX + 1, centerY - 1);
        var pixel2 = new Pixel(centerX - 1, centerY);
        var pixel3 = new Pixel(centerX, centerY);
        var pixel4 = new Pixel(centerX + 1, centerY);
        PixelsInShape.AddRange(pixel1, pixel2, pixel3, pixel4);
    }

    private void JShape()
    {
        var pixel1 = new Pixel(centerX-1, centerY-1);
        var pixel2 = new Pixel(centerX-1, centerY);
        var pixel3 = new Pixel(centerX, centerY);
        var pixel4 = new Pixel(centerX+1, centerY);
        PixelsInShape.AddRange(pixel1, pixel2, pixel3, pixel4);
    }
}