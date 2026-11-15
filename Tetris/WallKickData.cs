namespace Tetris;

public class WallKickData
{
   public readonly Orientation initialOrientation;
   public readonly Orientation nextOrientation;
   public readonly List<Coordinate> wallKickCoordinates;

   public WallKickData(Orientation initialOrientation, Orientation nextOrientation, List<Coordinate> wallKickCoordinates)
   {
      this.initialOrientation = initialOrientation;
      this.nextOrientation = nextOrientation;
      this.wallKickCoordinates = wallKickCoordinates;
   }
}

