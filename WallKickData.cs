namespace Tetris;

public class WallKickData
{
   public Orientation initialOrientation;
   public Orientation nextOrientation;
   public List<(int x,int y)> wallKickCoordinates;

   public WallKickData(Orientation initialOrientation, Orientation nextOrientation, List<(int x,int y)> wallKickCoordinates)
   {
      this.initialOrientation = initialOrientation;
      this.nextOrientation = nextOrientation;
      this.wallKickCoordinates = wallKickCoordinates;
   }
}

