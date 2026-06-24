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
        //TODO: when I'm drawing the object, that's when I use the actual pixels but before that I use grid squares
            Raylib.DrawRectangle(fallingObject.X, fallingObject.Y, Pixel.Width, Pixel.Width, Color.Red);

            foreach (var obj in objectsOnGrid)
            {
                Raylib.DrawRectangle(obj.X, obj.Y, Pixel.Width, Pixel.Width, Color.Blue);
            }
        
    }

    void SpawnObject()
    {
        var newObject = new Pixel(xOrigin + widthGrid / 2, yOrigin);
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
    }

    bool isSettled()
    {
        var spaceBeneath = fallingObject.Y + gridSquareSize;
        var bottomOfGrid = yOrigin + heightGrid;
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
}