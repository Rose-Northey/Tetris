using Raylib_cs;

namespace Tetris;

public class ShapesOnGrid
{
    private int nWidth;
    private int nHeight;
    public int xOrigin;
    public int yOrigin;
    public const int nXPixelsInGrid= 5;
    public const int nYPixelsInGrid= 10;
    private Pixel fallingShape;
    private List<Pixel> approachingShapes;
    public int gridSquareSize;
    private List<Pixel> settledShapes;
    
    
    
    public ShapesOnGrid(int windowWidth, int windowHeight, int pixelWidth)
    {
        xOrigin = (windowWidth - pixelWidth * nXPixelsInGrid) / 2;
        yOrigin = (windowHeight - pixelWidth * nYPixelsInGrid) / 2;
        gridSquareSize = pixelWidth;
        ResetGrid();
    }
    
    public void DrawFrame()
    {
        var (fallingObjX, fallingObjY) = gridToWindowCoordinates(fallingShape.X, fallingShape.Y);
            Raylib.DrawRectangle(fallingObjX, fallingObjY, Pixel.Width, Pixel.Width, Color.Red);

            foreach (var obj in settledShapes)
            {
                var (gridObjX, gridObjY) = gridToWindowCoordinates(obj.X, obj.Y);
                Raylib.DrawRectangle(gridObjX, gridObjY, Pixel.Width, Pixel.Width, Color.Blue);
            }
    }

    private (int, int) gridToWindowCoordinates(int gridX, int gridY)
    {
        var windowX = gridX * gridSquareSize + xOrigin;
        var windowY = gridY * gridSquareSize + yOrigin;
        return (windowX, windowY);
    }
    

    void SpawnShape()
    {
        var newObject = new Pixel(0 + nXPixelsInGrid / 2, 0);
        approachingShapes.Add(newObject);
    }
    
    public void ResetGrid()
    {
        approachingShapes = [];
        SpawnShape();
        fallingShape= approachingShapes[0];
        settledShapes = [];
    }
    public void enactGravity()
    {
        if (isSettled())
        {
            settledShapes.Add(fallingShape);
            approachingShapes.RemoveAt(0);
            SpawnShape();
            fallingShape = approachingShapes[0];
            //check for whether complete row of squares at bottom
            removeFullRows();


        }
        fallingShape.Y+= 1;
    }

    bool isSettled()
    {
        var spaceBeneath = fallingShape.Y + 1;
        var bottomOfGrid = nYPixelsInGrid;
        if (spaceBeneath == bottomOfGrid) return true;
        
        foreach (var obj in settledShapes)
        {
            if (obj.X == fallingShape.X)
            {
                    if (obj.Y == spaceBeneath){return true;}
            }
        }
        return false;
    }
    
    public void moveFallingShape(int x, int y)
    {
        var aspiringX = fallingShape.X + x;
        var aspiringY = fallingShape.Y + y;
        
        if (aspiringX is < 0 or > nXPixelsInGrid-1) return;
        if (aspiringY > nYPixelsInGrid - 1) return;
        if (settledShapes.Any((obj) => obj.X == aspiringX && obj.Y == aspiringY)) return;
   
        fallingShape.X += x;
        fallingShape.Y += y;
   
    }

    void removeFullRows()
    {
        //go from 0 to nYPixelsInGrid
        //check if the row is full
        for (var rowNumber = 0; rowNumber < nYPixelsInGrid; rowNumber++)
        {
            var shapesInRow = settledShapes.Where((shape) => shape.Y == rowNumber);
            if (shapesInRow.Count() == nXPixelsInGrid)
            {
                //delete that row
                //move row above down
                settledShapes.RemoveAll((ss)=>shapesInRow.Contains(ss));
                foreach (var ss in settledShapes)
                {
                    ss.Y++;
                }
            }
        }
    }
}