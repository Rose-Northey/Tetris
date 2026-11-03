using Raylib_cs;

namespace Tetris;

public class Shape
{
    public readonly List<Pixel> PixelsInShape = [];
    public Coordinate center;
    private float xOffset;
    private float yOffset;
    public readonly Orientation currentOrientation;
    public Dictionary<(Orientation initial, Orientation next), List<Coordinate>> wallKickLookup;
    
    public Shape(int x, int y)
    {
        center = new Coordinate(x, y);
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

    public void moveShape(Coordinate translation)
    {
        center = new Coordinate(center.x+translation.x, center.y+translation.y);
        foreach (var pixel in PixelsInShape)
        {
            pixel.coordinate = new Coordinate(pixel.coordinate.x + translation.x, pixel.coordinate.y + translation.y);
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
    
    private Coordinate findTranslatedCoordinatetes(int xMovement, int yMovement, Coordinate coordinate)
    {
        return new Coordinate(coordinate.x + xMovement, coordinate.y + yMovement);
    }

    public Coordinate findRotatedTranslatedPixelLocation(Pixel pixel, int xMovement, int yMovement,
        Direction direction)
    {
        var rotatedLocation = findPixelRotatedCoordinates(direction, pixel.coordinate);
        var rotatedAndTranslatedLocation = findTranslatedCoordinatetes(xMovement, yMovement, rotatedLocation);
        return rotatedAndTranslatedLocation;
    }
    

        
    private void IShape()
    {
        center = center with { y = center.y - 1 };
        var pixel1 = new Pixel(center with {y=center.y-1});
        var pixel2 = new Pixel(center with {y=center.y-2});
        var pixel3 = new Pixel(center);
        var pixel4 = new Pixel(center with { y = center.y + 1 });
        xOffset = -0.5F;
        yOffset = -0.5F;
        PixelsInShape.AddRange(pixel1,pixel2, pixel3,pixel4);
        wallKickLookup = findWallKickLookupForIShape();
    }
    
    private void OShape()
    {
        var pixel1 = new Pixel(new Coordinate(center.x - 1, center.y - 1));
        var pixel2 = new Pixel(center with { y = center.y - 1 });
        var pixel3 = new Pixel(center with { x = center.x - 1 });
        var pixel4 = new Pixel(center);
        
        xOffset = -0.5F;
        yOffset = -0.5F;
        PixelsInShape.AddRange(pixel1, pixel2, pixel3, pixel4);
    }

    private void TShape()
    {
        var pixel1 = new Pixel(center with { y = center.y - 1 });
        var pixel2 = new Pixel(center with { x = center.x - 1 });
        var pixel3 = new Pixel(center);
        var pixel4 = new Pixel(center with { x = center.x + 1 });

        PixelsInShape.AddRange(pixel1, pixel2, pixel3, pixel4);
        wallKickLookup = findWallKickLookupForJLSTZShapes();
    }

    private void SShape()
    {
        var pixel1 = new Pixel(center with { x = center.x - 1 });
        var pixel2 = new Pixel(center);
        var pixel3 = new Pixel(center with { y = center.y - 1 });
        var pixel4 = new Pixel(new Coordinate(center.x+1, center.y-1));

        PixelsInShape.AddRange(pixel1, pixel2, pixel3, pixel4);
        wallKickLookup = findWallKickLookupForJLSTZShapes();
    }
    
    private void ZShape()
    {
        var pixel1 = new Pixel(new Coordinate(center.x - 1, center.y - 1));
        var pixel2 = new Pixel(center with { y = center.y - 1 });
        var pixel3 = new Pixel(center);
        var pixel4 = new Pixel(center with { x = center.x + 1 });
        
        PixelsInShape.AddRange(pixel1, pixel2, pixel3, pixel4);
        wallKickLookup = findWallKickLookupForJLSTZShapes();
    }
    
    
    private void LShape()
    {
        var pixel1 = new Pixel(new Coordinate(center.x + 1, center.y - 1));
        var pixel2 = new Pixel(center with { x = center.x - 1 });
        var pixel3 = new Pixel(center);
        var pixel4 = new Pixel(center with { x = center.x + 1 });

        PixelsInShape.AddRange(pixel1, pixel2, pixel3, pixel4);
        wallKickLookup = findWallKickLookupForJLSTZShapes();
    }

    private void JShape()
    {
        var pixel1 = new Pixel(new Coordinate(center.x - 1, center.y - 1));
        var pixel2 = new Pixel(center with { x = center.x - 1 });
        var pixel3 = new Pixel(center);
        var pixel4 = new Pixel(center with { x = center.x + 1 });
        
        PixelsInShape.AddRange(pixel1, pixel2, pixel3, pixel4);
        wallKickLookup = findWallKickLookupForJLSTZShapes();
    }

    private Dictionary<(Orientation initial, Orientation next), List<Coordinate>> findWallKickLookupForIShape()
    {
        var data = new List<WallKickData>
        {
            new (initialOrientation: Orientation.zero,  Orientation.right, [new Coordinate(0,0), new Coordinate(-2,0), new Coordinate(1,0),  new Coordinate(-2,-1), new Coordinate(1,2)]),
            new (initialOrientation: Orientation.right, Orientation.zero,  [new Coordinate(0,0), new Coordinate(2,0),  new Coordinate(-1,0), new Coordinate(2,1),   new Coordinate(-1,-2)]),
            new (initialOrientation: Orientation.right, Orientation.two,   [new Coordinate(0,0), new Coordinate(-1,0), new Coordinate(2,0),  new Coordinate(-1,2),  new Coordinate(2,-1)]),
            new (initialOrientation: Orientation.two,   Orientation.right, [new Coordinate(0,0), new Coordinate(1,0),  new Coordinate(-2,0), new Coordinate(1,-2),  new Coordinate(-2,1)]),
            new (initialOrientation: Orientation.two,   Orientation.left,  [new Coordinate(0,0), new Coordinate(2,0),  new Coordinate(-1,0), new Coordinate(2,1),   new Coordinate(-1,-2)]),
            new (initialOrientation: Orientation.left,  Orientation.two,   [new Coordinate(0,0), new Coordinate(-2,0), new Coordinate(1,0),  new Coordinate(-2,-1), new Coordinate(1,2)]),
            new (initialOrientation: Orientation.left,  Orientation.zero,  [new Coordinate(0,0), new Coordinate(1,0),  new Coordinate(-2,0), new Coordinate(1,-2),  new Coordinate(-2,1)]),
            new (initialOrientation: Orientation.zero,  Orientation.left,  [new Coordinate(0,0), new Coordinate(-1,0), new Coordinate(2,0),  new Coordinate(-1,2),  new Coordinate(2,-1)]),
        };
        return data.ToDictionary(x => (initial: x.initialOrientation, next: x.nextOrientation),x => x.wallKickCoordinates);
    }
    

    private Dictionary <(Orientation initial, Orientation next), List<Coordinate>> findWallKickLookupForJLSTZShapes()
    {
        var data = new List<WallKickData>
        {
            new (initialOrientation: Orientation.zero,  Orientation.right, [new Coordinate(0,0), new Coordinate(-1,0), new Coordinate(-1,1),  new Coordinate(0,-2), new Coordinate(-1,-2)]),
            new (initialOrientation: Orientation.right, Orientation.zero,  [new Coordinate(0,0), new Coordinate(1,0),  new Coordinate(1,-1),  new Coordinate(0,2),  new Coordinate(1,2)]),
            new (initialOrientation: Orientation.right, Orientation.two,   [new Coordinate(0,0), new Coordinate(1,0),  new Coordinate(1,-1),  new Coordinate(0,2),  new Coordinate(1,2)]),
            new (initialOrientation: Orientation.two,   Orientation.right, [new Coordinate(0,0), new Coordinate(-1,0), new Coordinate(-1,1),  new Coordinate(0,-2), new Coordinate(-1,-2)]),
            new (initialOrientation: Orientation.two,   Orientation.left,  [new Coordinate(0,0), new Coordinate(1,0),  new Coordinate(1,1),   new Coordinate(0,-2), new Coordinate(1,-2)]),
            new (initialOrientation: Orientation.left,  Orientation.two,   [new Coordinate(0,0), new Coordinate(-1,0), new Coordinate(-1,-1), new Coordinate(0,2),  new Coordinate(-1,2)]),
            new (initialOrientation: Orientation.left,  Orientation.zero,  [new Coordinate(0,0), new Coordinate(-1,0), new Coordinate(-1,-1), new Coordinate(0,2),  new Coordinate(-1,2)]),
            new (initialOrientation: Orientation.zero,  Orientation.left,  [new Coordinate(0,0), new Coordinate(1,0),  new Coordinate(1,1),   new Coordinate(0,-2), new Coordinate(1,-2)]),
        };
        return data.ToDictionary(x => (initial: x.initialOrientation, next: x.nextOrientation),x => x.wallKickCoordinates);
    }

}

public enum Orientation{
zero, left, two, right
}




