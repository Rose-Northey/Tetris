using Raylib_cs;

namespace Tetris;

public class Shape
{
    public readonly List<Pixel> PixelsInShape = [];
    public int centerX;//grid coordinates
    public int centerY;
    private float xOffset;
    private float yOffset;
    private Orientation orientation;

    public Shape(int x, int y)
    {
        centerX = x;
        centerY = y;
        xOffset = 0;
        yOffset = 0;
        var buildShape = GenerateShapeRecipe();
        buildShape();
        colorShape();
        orientation = Orientation.zero;
    }

    private Color GenerateColor()
    {
        var randomHue = new Random().Next(180, 360);
        var randomSaturation = GenerateRandomFloat(0.7f, 0.9f);
        var randomValue = GenerateRandomFloat(0.8f, 0.9f);
        return Color.FromHSV(randomHue, randomSaturation, randomValue);
    }

    private float GenerateRandomFloat(float min, float max)
    {
        return new Random().NextSingle() * (max - min) + min;
    }


    private void colorShape()
    {
        var randomColor = GenerateColor();
        foreach (var pixel in PixelsInShape)
        {
            pixel.color = randomColor;
        }
    }
    
    static float NextFloat(Random random)
    {
        double mantissa = (random.NextDouble() * 2.0) - 1.0;
        // choose -149 instead of -126 to also generate subnormal floats (*)
        double exponent = Math.Pow(2.0, random.Next(-126, 128));
        return (float)(mantissa * exponent);
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
            var (aspiringX, aspiringY) = findRotatedCoordinates(direction, pixel);
            pixel.X = aspiringX;
            pixel.Y = aspiringY;
        }
        ChangeOrientation(direction);
    }

    public void ChangeOrientation(Direction direction)
    {
        if (direction == Direction.Clockwise)
        {
            orientation = orientation switch
            {
                Orientation.zero => Orientation.right,
                Orientation.right => Orientation.two,
                Orientation.two => Orientation.left,
                _ => Orientation.zero
            };
            return;
        }

        orientation = orientation switch
        {
            Orientation.zero => Orientation.left,
            Orientation.left => Orientation.two,
            Orientation.two => Orientation.right,
            _ => Orientation.zero
        };
    }
    
    public (int, int) findRotatedCoordinates(Direction direction, Pixel pixel)
    {
        var calculatedCenterX = centerX + xOffset;
        var calculatedCenterY = centerY + yOffset;
        var xDiff = pixel.X - calculatedCenterX;
        var yDiff = pixel.Y - calculatedCenterY;
        var aspiringXRaw = calculatedCenterX - yDiff * (int)direction;
        var aspiringYRaw = calculatedCenterY + xDiff * (int)direction;
        var aspiringX = (int)Math.Round(aspiringXRaw); ;
        var aspiringY = (int)Math.Round(aspiringYRaw);
    
        return (aspiringX, aspiringY);
    }
        
    private void IShape()
    {
        centerY -= 1;
        var pixel1 = new Pixel(centerX, centerY - 1);
        var pixel2 = new Pixel(centerX, centerY-2);
        var pixel3 = new Pixel(centerX, centerY);
        var pixel4 = new Pixel(centerX, centerY+1);   
        xOffset = -0.5F;
        yOffset = -0.5F;
        PixelsInShape.AddRange(pixel1,pixel2, pixel3,pixel4);
    }
    
    private void OShape()
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

public enum Orientation{
zero, left, two, right
}