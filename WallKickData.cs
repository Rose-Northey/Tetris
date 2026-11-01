namespace Tetris;

public class WallKickData
{
   public readonly Orientation initialOrientation;
   public readonly Orientation nextOrientation;
   public readonly List<(int x,int y)> wallKickCoordinates;

   public WallKickData(Orientation initialOrientation, Orientation nextOrientation, List<(int x,int y)> wallKickCoordinates)
   {
      this.initialOrientation = initialOrientation;
      this.nextOrientation = nextOrientation;
      this.wallKickCoordinates = wallKickCoordinates;
   }
}

