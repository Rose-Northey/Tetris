using Raylib_cs;

namespace Tetris;

public class ObjectsOnGrid
{
    private int nWidth;
    private int nHeight;
    public int xOrigin;
    public int yOrigin;
    private const int nXPixelsInGrid= 10;
    private const int nYPixelsInGrid= 20;
    public readonly int widthGrid;
    public readonly int heightGrid;
    private Pixel fallingObject;
    private List<Pixel> approachingObjects;
    private int gridSquareSize;
    private List<Pixel> objectsOnGrid;
    
    
    
    public ObjectsOnGrid(int windowWidth, int windowHeight, int pixelWidth)
    {
        this.widthGrid = pixelWidth * nXPixelsInGrid;
        this.heightGrid = pixelWidth * nYPixelsInGrid;
        this.xOrigin = (windowWidth - widthGrid) / 2;
        this.yOrigin = (windowHeight - heightGrid) / 2;
        this.gridSquareSize = pixelWidth;
        ResetGrid();
        
    }
    
    public void DrawObjectFrame()
    {
            Raylib.DrawRectangle(fallingObject.X, fallingObject.Y, Pixel.Width, Pixel.Width, Color.Red);

            foreach (var obj in objectsOnGrid)
            {
                Raylib.DrawRectangle(obj.X, obj.Y, Pixel.Width, Pixel.Width, Color.Blue);
            }
        
    }

    void SpawnObject()
    {
        var spawnPixel = Random.Shared.Next(0,10);
        var newObject = new Pixel(xOrigin + spawnPixel * gridSquareSize, yOrigin - gridSquareSize);
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
        fallingObject.Y+= gridSquareSize;
        // before moving each time, checks if the block underneath is empty. This only happens when the block is about to move
    }

    bool isSettled()
    {
        var spaceBeneath = fallingObject.Y + gridSquareSize;
        if (spaceBeneath > heightGrid+yOrigin-Pixel.Width) return true;
        foreach (var obj in objectsOnGrid)
        {
            if (obj.X == fallingObject.X)
            {
                    return obj.Y == spaceBeneath;
            }
        }

        return false;
    }
}