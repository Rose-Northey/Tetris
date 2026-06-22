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
        
            Raylib.DrawRectangle(fallingObject.X, fallingObject.Y, fallingObject.Width, fallingObject.Width, Color.Red);

            foreach (var obj in objectsOnGrid)
            {
                Raylib.DrawRectangle(obj.X, obj.Y, obj.Width, obj.Width, Color.Blue);
            }
        
    }
    
    public void ResetGrid()
    {
        var newPixel = new Pixel(xOrigin, yOrigin);
        var newPixel2 = new Pixel(xOrigin, yOrigin);
        
        approachingObjects = [newPixel,newPixel2];
        fallingObject= approachingObjects[0];
        objectsOnGrid = [];
    }
    public void enactGravity()
    {
        if (fallingObject.Y == heightGrid+yOrigin-fallingObject.Width)
        {
            objectsOnGrid.Add(fallingObject);
            approachingObjects.RemoveAt(0);
            fallingObject = approachingObjects[0];
        }
        fallingObject.Y+= gridSquareSize;
        
    }

    
}