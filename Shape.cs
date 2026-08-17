namespace Tetris;

public class Shape
{
    public List<Pixel> PixelsInShape = [];
    public int centerX;
    public int centerY;
    public float xOffset;
    public float yOffset;

    public Shape(int x, int y)
    {
        centerX = x;
        centerY = y;
        xOffset = 0;
        yOffset = 0;
        var formRandomShape = GenerateShapeRecipe();
        formRandomShape();
    }

    public Action GenerateShapeRecipe()
    {
        var randomNumber = new Random().Next(0, 7);
        switch (randomNumber)
        {
            case(0):return IShape;
            case(1):return OShape;
            case(2):return TShape;
            case(3):return SShape;
            case(4):return ZShape;
            case(5):return LShape;
            default:return JShape;
        }
        
    }
    
    public void IShape()
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

    public void TShape()
    {
        var pixel1 = new Pixel(centerX, centerY - 1);
        var pixel2 = new Pixel(centerX - 1, centerY);
        var pixel3 = new Pixel(centerX, centerY);
        var pixel4 = new Pixel(centerX + 1, centerY);
        PixelsInShape.AddRange(pixel1, pixel2, pixel3, pixel4);
    }

    public void SShape()
    {
        var pixel1 = new Pixel(centerX-1, centerY);
        var pixel2 = new Pixel(centerX, centerY);
        var pixel3 = new Pixel(centerX, centerY-1);
        var pixel4 = new Pixel(centerX + 1, centerY-1);
        PixelsInShape.AddRange(pixel1, pixel2, pixel3, pixel4);
    }
    
    public void ZShape()
    {
        var pixel1 = new Pixel(centerX-1, centerY-1);
        var pixel2 = new Pixel(centerX, centerY-1);
        var pixel3 = new Pixel(centerX, centerY);
        var pixel4 = new Pixel(centerX+1, centerY);
        PixelsInShape.AddRange(pixel1, pixel2, pixel3, pixel4);
    }
    
    
    public void LShape()
    {
        var pixel1 = new Pixel(centerX + 1, centerY - 1);
        var pixel2 = new Pixel(centerX - 1, centerY);
        var pixel3 = new Pixel(centerX, centerY);
        var pixel4 = new Pixel(centerX + 1, centerY);
        PixelsInShape.AddRange(pixel1, pixel2, pixel3, pixel4);
    }

    public void JShape()
    {
        var pixel1 = new Pixel(centerX-1, centerY-1);
        var pixel2 = new Pixel(centerX-1, centerY);
        var pixel3 = new Pixel(centerX, centerY);
        var pixel4 = new Pixel(centerX+1, centerY);
        PixelsInShape.AddRange(pixel1, pixel2, pixel3, pixel4);
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
}