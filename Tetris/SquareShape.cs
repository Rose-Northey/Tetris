namespace Tetris;

public class SquareShape:Shape
{
    public SquareShape(Coordinate center):base(center)
    {
    }
    
    protected override void CreateShape()
    {
        var pixel1 = new Pixel(new Coordinate(center.x - 1, center.y - 1));
        var pixel2 = new Pixel(center with { y = center.y - 1 });
        var pixel3 = new Pixel(center with { x = center.x - 1 });
        var pixel4 = new Pixel(center);
        
        xOffset = -0.5F;
        yOffset = -0.5F;
        pixels.AddRange(pixel1, pixel2, pixel3, pixel4);
    }
    
    protected override Dictionary<(Orientation initial, Orientation next), List<Coordinate>> FindWallKickLookup()
    {
        var data = new List<WallKickData>
        {
            new (initialOrientation: Orientation.zero,  Orientation.right, [new Coordinate(0,0), new Coordinate(0,0), new Coordinate(0,0),  new Coordinate(0,0), new Coordinate(0,0)]),
            new (initialOrientation: Orientation.right, Orientation.zero,  [new Coordinate(0,0), new Coordinate(0,0), new Coordinate(0,0),  new Coordinate(0,0), new Coordinate(0,0)]),
            new (initialOrientation: Orientation.right, Orientation.two,   [new Coordinate(0,0), new Coordinate(0,0), new Coordinate(0,0),  new Coordinate(0,0), new Coordinate(0,0)]),
            new (initialOrientation: Orientation.two,   Orientation.right, [new Coordinate(0,0), new Coordinate(0,0), new Coordinate(0,0),  new Coordinate(0,0), new Coordinate(0,0)]),
            new (initialOrientation: Orientation.two,   Orientation.left,  [new Coordinate(0,0), new Coordinate(0,0), new Coordinate(0,0),  new Coordinate(0,0), new Coordinate(0,0)]),
            new (initialOrientation: Orientation.left,  Orientation.two,   [new Coordinate(0,0), new Coordinate(0,0), new Coordinate(0,0),  new Coordinate(0,0), new Coordinate(0,0)]),
            new (initialOrientation: Orientation.left,  Orientation.zero,  [new Coordinate(0,0), new Coordinate(0,0), new Coordinate(0,0),  new Coordinate(0,0), new Coordinate(0,0)]),
            new (initialOrientation: Orientation.zero,  Orientation.left,  [new Coordinate(0,0), new Coordinate(0,0), new Coordinate(0,0),  new Coordinate(0,0), new Coordinate(0,0)]),
        };
        return data.ToDictionary(x => (initial: x.initialOrientation, next: x.nextOrientation),x => x.wallKickCoordinates);
    }

}