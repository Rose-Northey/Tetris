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
    private int fallingObject;
    private Pixel[] objects;
    
    
    public ObjectsOnGrid(int windowWidth, int windowHeight, int pixelWidth)
    {
        this.widthGrid = pixelWidth * nXPixelsInGrid;
        this.heightGrid = pixelWidth * nYPixelsInGrid;
        this.xOrigin = (windowWidth - widthGrid) / 2;
        this.yOrigin = (windowHeight - heightGrid) / 2;
        ResetGrid();
        
    }
    
    public void DrawObjects()
    {
        foreach (Pixel pixel in this.objects)
        {
            Raylib.DrawRectangle(xOrigin, yOrigin, pixel.Width, pixel.Width, Color.Red);
        }
        //draw square at 00 on grid
        //initialise falling object and draw it on grid
        //then think about fall
        
    }
    
    public void ResetGrid()
    {
        var newPixel = new Pixel(xOrigin, yOrigin);
        this.objects = [newPixel];
    }

    
}