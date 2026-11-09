namespace Tetris;

public class LongShape:Shape
{
    public LongShape(Coordinate center):base(center)
    {
        CreateShape();
    }

    protected override void CreateShape()
    {
        center = center with { y = center.y - 1 };
        var pixel1 = new Pixel(center with {y=center.y-1});
        var pixel2 = new Pixel(center with {y=center.y-2});
        var pixel3 = new Pixel(center);
        var pixel4 = new Pixel(center with { y = center.y + 1 });
        xOffset = -0.5F;
        yOffset = -0.5F;
        pixels.AddRange(pixel1,pixel2, pixel3,pixel4);
        wallKickLookup = FindWallKickLookup();
    }
    
    protected override Dictionary<(Orientation initial, Orientation next), List<Coordinate>> FindWallKickLookup()
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

}