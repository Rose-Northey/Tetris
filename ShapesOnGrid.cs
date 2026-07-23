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
    private Shape fallingShape;
    private List<Shape> approachingShapes;
    public int gridSquareSize;
    private List<Pixel> settledPixels;
    
    
    
    public ShapesOnGrid(int windowWidth, int windowHeight, int pixelWidth)
    {
        xOrigin = (windowWidth - pixelWidth * nXPixelsInGrid) / 2;
        yOrigin = (windowHeight - pixelWidth * nYPixelsInGrid) / 2;
        gridSquareSize = pixelWidth;
        ResetGrid();
    }
    
    public void DrawFrame()
    {
        foreach (var pixel in fallingShape.PixelsInShape)
        {
                    var (pixelX, pixelY) = gridToWindowCoordinates(pixel.X, pixel.Y);
                        Raylib.DrawRectangle(pixelX, pixelY, Pixel.Width, Pixel.Width, Color.Red);
        }
        foreach (var obj in settledPixels)
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
        var newShape = new Shape(0 + nXPixelsInGrid / 2, 0);
        approachingShapes.Add(newShape);
    }
    
    public void ResetGrid()
    {
        approachingShapes = [];
        SpawnShape();
        fallingShape= approachingShapes[0];
        settledPixels = [];
    }
    public void enactGravity()
    {
        if (isMovementPossible(0,1))
        {
            foreach (var pixel in fallingShape.PixelsInShape)
            {
                settledPixels.Add(pixel);
            }
            approachingShapes.RemoveAt(0);
            SpawnShape();
            fallingShape = approachingShapes[0];
            //check for whether complete row of squares at bottom
            removeFullRows();


        }
        fallingShape.moveShape(0,1);
    }
    
    //isSettled is a bit like a moveFallingShape but just for y. but only in the check. take the checks and refactor out to their oen boolean.
    
    public void moveFallingShape(int x, int y)
    {
        isMovementPossible(x, y);
        fallingShape.moveShape(x, y);
    }

    public bool isMovementPossible(int x, int y)
    {
        foreach (var pixel in fallingShape.PixelsInShape)
        {
            var aspiringX = pixel.X + x;
            var aspiringY = pixel.Y + y;

            if (aspiringX is < 0 or > nXPixelsInGrid - 1) return false;
            if (aspiringY > nYPixelsInGrid - 1) return false;
            if (settledPixels.Any((obj) => obj.X == aspiringX && obj.Y == aspiringY)) return false;
        }
        return true;
    }

    void removeFullRows()
    {
        //go from 0 to nYPixelsInGrid
        //check if the row is full
        for (var rowNumber = 0; rowNumber < nYPixelsInGrid; rowNumber++)
        {
            var shapesInRow = settledPixels.Where((shape) => shape.Y == rowNumber);
            if (shapesInRow.Count() == nXPixelsInGrid)
            {
                //delete that row
                //move row above down
                settledPixels.RemoveAll((ss)=>shapesInRow.Contains(ss));
                foreach (var ss in settledPixels)
                {
                    ss.Y++;
                }
            }
        }
    }
}