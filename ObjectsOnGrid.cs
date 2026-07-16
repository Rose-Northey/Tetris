using Raylib_cs;

namespace Tetris;

public class ObjectsOnGrid
{
    private int nWidth;
    private int nHeight;
    public int xOrigin;
    public int yOrigin;
    public const int nXPixelsInGrid= 10;
    public const int nYPixelsInGrid= 20;
    public Pixel fallingObject;
    private List<Pixel> approachingObjects;
    public int gridSquareSize;
    private List<Pixel> objectsOnGrid;
    
    
    
    public ObjectsOnGrid(int windowWidth, int windowHeight, int pixelWidth)
    {
        xOrigin = (windowWidth - pixelWidth * nXPixelsInGrid) / 2;
        yOrigin = (windowHeight - pixelWidth * nYPixelsInGrid) / 2;
        gridSquareSize = pixelWidth;
        ResetGrid();
    }
    
    public void DrawObjectFrame()
    {
        var (fallingObjX, fallingObjY) = gridToCanvasCoordinates(fallingObject.X, fallingObject.Y);
            Raylib.DrawRectangle(fallingObjX, fallingObjY, Pixel.Width, Pixel.Width, Color.Red);

            foreach (var obj in objectsOnGrid)
            {
                var (gridObjX, gridObjY) = gridToCanvasCoordinates(obj.X, obj.Y);
                Raylib.DrawRectangle(gridObjX, gridObjY, Pixel.Width, Pixel.Width, Color.Blue);
            }
    }

    private (int, int) gridToCanvasCoordinates(int gridX, int gridY)
    {
        var canvasX = gridX * gridSquareSize + xOrigin;
        var canvasY = gridY * gridSquareSize + yOrigin;
        return (canvasX, canvasY);
    }
    

    void SpawnObject()
    {
        var newObject = new Pixel(0 + nXPixelsInGrid / 2, 0);
        approachingObjects.Add(newObject);
    }
    
    public void ResetGrid()
    {
        approachingObjects = [];
        SpawnObject();
        fallingObject= approachingObjects[0];
        objectsOnGrid = [];
    }
    public void enactGravity()
    {
        if (isSettled())
        {
            objectsOnGrid.Add(fallingObject);
            approachingObjects.RemoveAt(0);
            SpawnObject();
            fallingObject = approachingObjects[0];
        }
        fallingObject.Y+= 1;
    }

    bool isSettled()
    {
        var spaceBeneath = fallingObject.Y + 1;
        var bottomOfGrid = 0 + nYPixelsInGrid;
        if (spaceBeneath == bottomOfGrid) return true;
        
        foreach (var obj in objectsOnGrid)
        {
            if (obj.X == fallingObject.X)
            {
                    if (obj.Y == spaceBeneath){return true;}
            }
        }
        return false;
    }
    
    public void moveFallingObject(int x, int y)
    {
        var aspiringX = fallingObject.X + x;
        var aspiringY = fallingObject.Y + y;
        
        
        if (aspiringX is < 0 or > nXPixelsInGrid-1) return;
        if (aspiringY > nYPixelsInGrid - 1) return;
        if (objectsOnGrid.Any((obj) => obj.X == aspiringX && obj.Y == aspiringY)) return;

        fallingObject.X += x ;
        fallingObject.Y += y;
    }
}