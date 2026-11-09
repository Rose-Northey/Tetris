using Raylib_cs;

namespace Tetris;

public abstract class Shape
{
    public readonly List<Pixel> pixels = [];
    public Coordinate center;
    protected float xOffset;
    protected float yOffset;
    public Orientation currentOrientation;
    public Dictionary<(Orientation initial, Orientation next), List<Coordinate>> wallKickLookup;
    
    public Shape(Coordinate center)
    {
        this.center = center;
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
        foreach (var pixel in pixels)
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

    public void ActuallyTranslateShape(Coordinate translation)
    {
        center = new Coordinate(center.x+translation.x, center.y+translation.y);
        foreach (var pixel in pixels)
        {
            pixel.coordinate = new Coordinate(pixel.coordinate.x + translation.x, pixel.coordinate.y + translation.y);
        }
    }

    public void ActuallyRotateShape(Direction direction)
    {
        currentOrientation = FindNewOrientation(direction);
        foreach (var pixel in pixels)
        {
            var (aspiringX, aspiringY) = findPixelRotatedCoordinates(direction, pixel.coordinate);
            pixel.coordinate = new Coordinate(aspiringX, aspiringY);
        }
    }
    

    public Orientation FindNewOrientation(Direction direction)
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
    
    private Coordinate findPixelRotatedCoordinates(Direction direction, Coordinate coordinate)
    {
        if (direction == Direction.None) return coordinate;
        
        var calculatedCenterX = center.x + xOffset;
        var calculatedCenterY = center.y + yOffset;
        var xDiff = coordinate.x - calculatedCenterX;
        var yDiff = coordinate.y - calculatedCenterY;
        var aspiringXRaw = calculatedCenterX - yDiff * (int)direction;
        var aspiringYRaw = calculatedCenterY + xDiff * (int)direction;
        var aspiringX = (int)Math.Round(aspiringXRaw); ;
        var aspiringY = (int)Math.Round(aspiringYRaw);

        return new Coordinate(aspiringX, aspiringY);
    }
    
    private Coordinate findTranslatedCoordinatetes(Coordinate translation, Coordinate originalLocation)
    {
        return new Coordinate(originalLocation.x + translation.x, originalLocation.y + translation.y);
    }

    public Coordinate findRotatedTranslatedPixelLocation(Pixel pixel, Coordinate translation,
        Direction rotation)
    {
        var rotatedLocation = findPixelRotatedCoordinates(rotation, pixel.coordinate);
        var rotatedAndTranslatedLocation = findTranslatedCoordinatetes(translation, rotatedLocation);
        return rotatedAndTranslatedLocation;
    }
    

        

    
    private void OShape()
    {
        var pixel1 = new Pixel(new Coordinate(center.x - 1, center.y - 1));
        var pixel2 = new Pixel(center with { y = center.y - 1 });
        var pixel3 = new Pixel(center with { x = center.x - 1 });
        var pixel4 = new Pixel(center);
        
        xOffset = -0.5F;
        yOffset = -0.5F;
        pixels.AddRange(pixel1, pixel2, pixel3, pixel4);
    }

    private void TShape()
    {
        var pixel1 = new Pixel(center with { y = center.y - 1 });
        var pixel2 = new Pixel(center with { x = center.x - 1 });
        var pixel3 = new Pixel(center);
        var pixel4 = new Pixel(center with { x = center.x + 1 });

        pixels.AddRange(pixel1, pixel2, pixel3, pixel4);
        wallKickLookup = findWallKickLookupForJLSTZShapes();
    }

    private void SShape()
    {
        var pixel1 = new Pixel(center with { x = center.x - 1 });
        var pixel2 = new Pixel(center);
        var pixel3 = new Pixel(center with { y = center.y - 1 });
        var pixel4 = new Pixel(new Coordinate(center.x+1, center.y-1));

        pixels.AddRange(pixel1, pixel2, pixel3, pixel4);
        wallKickLookup = findWallKickLookupForJLSTZShapes();
    }
    
    private void ZShape()
    {
        var pixel1 = new Pixel(new Coordinate(center.x - 1, center.y - 1));
        var pixel2 = new Pixel(center with { y = center.y - 1 });
        var pixel3 = new Pixel(center);
        var pixel4 = new Pixel(center with { x = center.x + 1 });
        
        pixels.AddRange(pixel1, pixel2, pixel3, pixel4);
        wallKickLookup = findWallKickLookupForJLSTZShapes();
    }
    
    
    private void LShape()
    {
        var pixel1 = new Pixel(new Coordinate(center.x + 1, center.y - 1));
        var pixel2 = new Pixel(center with { x = center.x - 1 });
        var pixel3 = new Pixel(center);
        var pixel4 = new Pixel(center with { x = center.x + 1 });

        pixels.AddRange(pixel1, pixel2, pixel3, pixel4);
        wallKickLookup = findWallKickLookupForJLSTZShapes();
    }

    private void JShape()
    {
        var pixel1 = new Pixel(new Coordinate(center.x - 1, center.y - 1));
        var pixel2 = new Pixel(center with { x = center.x - 1 });
        var pixel3 = new Pixel(center);
        var pixel4 = new Pixel(center with { x = center.x + 1 });
        
        pixels.AddRange(pixel1, pixel2, pixel3, pixel4);
        wallKickLookup = findWallKickLookupForJLSTZShapes();
    }


    protected abstract Dictionary<(Orientation initial, Orientation next), List<Coordinate>> FindWallKickLookup();
    protected abstract void CreateShape();

}

public enum Orientation{
zero, left, two, right
}




