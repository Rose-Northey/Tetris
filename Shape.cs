using Raylib_cs;

namespace Tetris;

public class Shape
{
    public readonly List<Pixel> PixelsInShape = [];
    public int centerX;//grid coordinates
    public int centerY;
    private float xOffset;
    private float yOffset;
    private Orientation currentOrientation;
    public readonly Dictionary<WallKickData, Orientation> wallKickLookup;

    public Shape(int x, int y)
    {
        centerX = x;
        centerY = y;
        xOffset = 0;
        yOffset = 0;
        var buildShape = GenerateShapeRecipe();
        buildShape();
        colorShape();
        currentOrientation = Orientation.zero;

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
        var prospectiveOrientation = FindNewOrientation(direction);
        //find prospective so we can use it in the wallkick data
        //figure out whether the rotation will work first
        //if the rotation won't work, try each wall kick translation otherwise, do nothing
        foreach (var pixel in PixelsInShape)
        {
            var (aspiringX, aspiringY) = findRotatedCoordinates(direction, pixel);
            pixel.X = aspiringX;
            pixel.Y = aspiringY;
        }
        
    }

    private Orientation FindNewOrientation(Direction direction)
    {
        Orientation newOrientation;
        if (direction == Direction.Clockwise)
        {
            newOrientation = currentOrientation switch
            {
                Orientation.zero => Orientation.right,
                Orientation.right => Orientation.two,
                Orientation.two => Orientation.left,
                _ => Orientation.zero
            };
        } else {
            newOrientation = currentOrientation switch
            {
                Orientation.zero => Orientation.left,
                Orientation.left => Orientation.two,
                Orientation.two => Orientation.right,
                _ => Orientation.zero
            };
        }
        return newOrientation;
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
        wallKickLookup = findWallKickLookupForIShape();
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
        wallKickLookup = findWallKickLookupForJLSTZShapes();
    }

    private void SShape()
    {
        var pixel1 = new Pixel(centerX-1, centerY);
        var pixel2 = new Pixel(centerX, centerY);
        var pixel3 = new Pixel(centerX, centerY-1);
        var pixel4 = new Pixel(centerX + 1, centerY-1);
        PixelsInShape.AddRange(pixel1, pixel2, pixel3, pixel4);
        wallKickLookup = findWallKickLookupForJLSTZShapes();
    }
    
    private void ZShape()
    {
        var pixel1 = new Pixel(centerX-1, centerY-1);
        var pixel2 = new Pixel(centerX, centerY-1);
        var pixel3 = new Pixel(centerX, centerY);
        var pixel4 = new Pixel(centerX+1, centerY);
        PixelsInShape.AddRange(pixel1, pixel2, pixel3, pixel4);
        wallKickLookup = findWallKickLookupForJLSTZShapes();
    }
    
    
    private void LShape()
    {
        var pixel1 = new Pixel(centerX + 1, centerY - 1);
        var pixel2 = new Pixel(centerX - 1, centerY);
        var pixel3 = new Pixel(centerX, centerY);
        var pixel4 = new Pixel(centerX + 1, centerY);
        PixelsInShape.AddRange(pixel1, pixel2, pixel3, pixel4);
        wallKickLookup = findWallKickLookupForJLSTZShapes();
    }

    private void JShape()
    {
        var pixel1 = new Pixel(centerX-1, centerY-1);
        var pixel2 = new Pixel(centerX-1, centerY);
        var pixel3 = new Pixel(centerX, centerY);
        var pixel4 = new Pixel(centerX+1, centerY);
        PixelsInShape.AddRange(pixel1, pixel2, pixel3, pixel4);
        wallKickLookup = findWallKickLookupForJLSTZShapes();
    }

    private Dictionary<WallKickData, Orientation> findWallKickLookupForIShape()
    {
        var data = new List<WallKickData>
        {
            new (Orientation.zero,  Orientation.right, [(0,0),(-2,0),(1,0),(-2,-1),(1,2)]),
            new (Orientation.right, Orientation.zero,  [(0,0),(2,0),(-1,0),(2,1),(-1,-2)]),
            new (Orientation.right, Orientation.two,   [(0,0),(-1,0),(2,0),(-1,2),(2,-1)]),
            new (Orientation.two,   Orientation.right, [(0,0),(1,0),(-2,0),(1,-2),(-2,1)]),
            new (Orientation.two,   Orientation.left,  [(0,0),(2,0),(-1,0),(2,1),(-1,-2)]),
            new (Orientation.left,  Orientation.two,   [(0,0),(-2,0),(1,0),(-2,-1),(1,2)]),
            new (Orientation.left,  Orientation.zero,  [(0,0),(1,0),(-2,0),(1,-2),(-2,1)]),
            new (Orientation.zero,  Orientation.left,  [(0,0),(-1,0),(2,0),(-1,2),(2,-1)]),
        };
        return data.ToDictionary(x => x, x => x.nextOrientation);
    }

    private Dictionary<WallKickData, Orientation> findWallKickLookupForJLSTZShapes()
    {
        var data = new List<WallKickData>
        {
            new (Orientation.zero,  Orientation.right, [(0,0),(-1,0),(-1,1),(0,-2),(-1,-2)]),
            new (Orientation.right, Orientation.zero,  [(0,0),(1,0),(1,-1),(0,2),(1,2)]),
            new (Orientation.right, Orientation.two,   [(0,0),(1,0),(1,-1),(0,2),(1,2)]),
            new (Orientation.two,   Orientation.right, [(0,0),(-1,0),(-1,1),(0,-2),(-1,-2)]),
            new (Orientation.two,   Orientation.left,  [(0,0),(1,0),(1,1),(0,-2),(1,-2)]),
            new (Orientation.left,  Orientation.two,   [(0,0),(-1,0),(-1,-1),(0,2),(-1,2)]),
            new (Orientation.left,  Orientation.zero,  [(0,0),(-1,0),(-1,-1),(0,2),(-1,2)]),
            new (Orientation.zero,  Orientation.left,  [(0,0),(1,0),(1,1),(0,-2),(1,-2)]),
        };
        return data.ToDictionary(x => x, x => x.nextOrientation);
    }

}

public enum Orientation{
zero, left, two, right
}




