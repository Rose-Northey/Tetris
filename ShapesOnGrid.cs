using Raylib_cs;

namespace Tetris;

public class ShapesOnGrid
{
    private int nWidth;
    private int nHeight;
    public int xOrigin;
    public int yOrigin;
    public const int nXPixelsInGrid= 5;
    public const int nYPixelsInGrid= 20;
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
        var (fallingShapeX, fallingShapeY) = gridToWindowCoordinates(fallingShape.centerX, fallingShape.centerY);
        Raylib.DrawRectangle(fallingShapeX, fallingShapeY, 5, 5, Color.Green);
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
        if (isMovementIllegal(0,1))
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
        if(isMovementIllegal(x, y)) return;
        fallingShape.moveShape(x, y);
    }
    
    public void rotateFallingShape(Direction direction)
    {
        if (isRotationMovementIllegal(direction)) return;
        fallingShape.rotateShape(direction);
    }

    public bool isMovementIllegal(int x, int y)
    {
        foreach (var pixel in fallingShape.PixelsInShape)
        {
            var aspiringX = pixel.X + x;
            var aspiringY = pixel.Y + y;

            if (aspiringX is < 0 or > nXPixelsInGrid - 1) return true;
            if (aspiringY > nYPixelsInGrid - 1) return true;
            if (settledPixels.Any((obj) => obj.X == aspiringX && obj.Y == aspiringY)) return true;
        }
        return false;
    }

    public bool isRotationMovementIllegal(Direction direction)
    {
        foreach (var pixel in fallingShape.PixelsInShape)
        {
            var (aspiringX, aspiringY) = pixel.findRotatedCoordinates(direction, fallingShape.centerX, fallingShape.centerY);
            if (aspiringX is < 0 or > nXPixelsInGrid - 1) return true;
            if (aspiringY > nYPixelsInGrid - 1) return true;
            if (settledPixels.Any((obj) => obj.X == aspiringX && obj.Y == aspiringY)) return true;
        }
        return false;
    }

    void removeFullRows()
    {
        for (var rowNumber = 0; rowNumber < nYPixelsInGrid; rowNumber++)
        {
            var shapesInRow = settledPixels.Where((shape) => shape.Y == rowNumber);
            if (shapesInRow.Count() == nXPixelsInGrid)
            {
                settledPixels.RemoveAll((ss)=>shapesInRow.Contains(ss));
                foreach (var ss in settledPixels)
                {
                    if (ss.Y < rowNumber)
                    {
                        ss.Y++;
                    }
                }
            }
        }
    }
}