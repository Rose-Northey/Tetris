using Raylib_cs;

namespace Tetris;

public class ShapesOnGrid
{
    private int nWidth;
    private int nHeight;
    private int xOrigin;
    private int yOrigin;
    private const int nXPixelsInGrid= 5;
    private const int nYPixelsInGrid= 10;
    private Shape fallingShape;
    private List<Shape> approachingShapes;
    private int gridSquareSize;
    private List<Pixel> settledPixels;
    private static readonly Color colourOfGridFill = Color.FromHSV(222, 0.50f, 0.22f);
    private static readonly Color colourOfGridBorder = Color.FromHSV(188, 0.80f, 0.85f);
    private static readonly Color colourOfGridLines = Color.FromHSV(222, 0.45f, 0.32f);
    
    public ShapesOnGrid(int windowWidth, int windowHeight, int pixelWidth)
    {
        xOrigin = (windowWidth - pixelWidth * nXPixelsInGrid) / 2;
        yOrigin = (windowHeight - pixelWidth * nYPixelsInGrid) / 2;
        gridSquareSize = pixelWidth;
        ResetGrid();
    }
    
    private void DrawGrid()
    {
        var widthGrid = nXPixelsInGrid * gridSquareSize;
        var heightGrid = nYPixelsInGrid * gridSquareSize;
        const int lineWidth = 1;
        Raylib.DrawRectangle(xOrigin, yOrigin, widthGrid, heightGrid,colourOfGridFill);
     
        for (var i = xOrigin; i <= widthGrid+xOrigin; i+= gridSquareSize)
        {
            Raylib.DrawRectangle(i, yOrigin,lineWidth, heightGrid, colourOfGridLines);
        }
        for (var i = yOrigin; i <= heightGrid + yOrigin; i += gridSquareSize)
        {
            Raylib.DrawRectangle(xOrigin, i, widthGrid, lineWidth, colourOfGridLines);
        }
        Raylib.DrawRectangleLines(xOrigin-1, yOrigin-1, widthGrid+2, heightGrid+2, colourOfGridBorder);
    }
    
    
    public void DrawFrame()
    {
        DrawGrid();
        foreach (var pixel in fallingShape.PixelsInShape)
        { 
            if (pixel.Y<0) continue;
            var (pixelX, pixelY) = gridToWindowCoordinates(pixel.X, pixel.Y); 
            Raylib.DrawRectangle(pixelX, pixelY, Pixel.Width, Pixel.Width, pixel.color);
        }
        
        foreach (var obj in settledPixels)
        {
            var (gridObjX, gridObjY) = gridToWindowCoordinates(obj.X, obj.Y);
            Raylib.DrawRectangle(gridObjX, gridObjY, Pixel.Width, Pixel.Width, obj.color);
        }
        
    }

    private (int, int) gridToWindowCoordinates(int gridX, int gridY)
    {
        var windowX = gridX * gridSquareSize + xOrigin;
        var windowY = gridY * gridSquareSize + yOrigin;
        return (windowX, windowY);
    }


    private void SpawnShape()
    {
        var newShape = new Shape(nXPixelsInGrid / 2, -1);
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
                pixel.color.GetHSV(out var h, out var s, out var v);
                pixel.color = Color.FromHSV(h, s - 0.1f, v - 0.1f);
                settledPixels.Add(pixel);
            }
            approachingShapes.RemoveAt(0);
            SpawnShape();
            fallingShape = approachingShapes[0];
            removeFullRows();
            return;
        }
        fallingShape.moveShape(0,1);
    }
    
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

    bool isMovementIllegal(int x, int y)
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

    bool isRotationMovementIllegal(Direction direction)
    {
        foreach (var pixel in fallingShape.PixelsInShape)
        {
            var (aspiringX, aspiringY) = pixel.findRotatedCoordinates(direction, fallingShape.centerX, fallingShape.centerY, fallingShape.xOffset, fallingShape.yOffset);
            //value is less than 0 or something
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
            if (shapesInRow.Count() != nXPixelsInGrid) continue;
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