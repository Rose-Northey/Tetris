namespace Tetris;

public abstract class JLSTZSShape:Shape
{
    protected JLSTZSShape(Coordinate center) : base(center)
    {
        
    }

    protected override Dictionary <(Orientation initial, Orientation next), List<Coordinate>> FindWallKickLookup()
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