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
    private Queue<Shape> waitingShapes;
    private int gridSquareSize;
    private List<Pixel> settledPixels;
    private static readonly Color colourOfGridFill = Color.FromHSV(222, 0.50f, 0.22f);
    private static readonly Color colourOfGridBorder = Color.FromHSV(188, 0.80f, 0.85f);
    private static readonly Color colourOfGridLines = Color.FromHSV(222, 0.45f, 0.32f);
    private int singleFrameScore;
    
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

    public void DrawScore()
    {
        var (leftOfGrid, bottomOfGrid) = gridToWindowCoordinates(0, nYPixelsInGrid);
        
        Raylib.DrawText($"Score: {singleFrameScore}", leftOfGrid, bottomOfGrid+15, 10, Color.White);
    }

    public int PlaySingleFrame()
    {
        enactGravity();
        return singleFrameScore;
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
        
        foreach (var pixel in settledPixels)
        {
            var (gridObjX, gridObjY) = gridToWindowCoordinates(pixel.X, pixel.Y);
            Raylib.DrawRectangle(gridObjX, gridObjY, Pixel.Width, Pixel.Width, pixel.color);
        }

  
        const int rightHandEdgeOfGameGrid = nXPixelsInGrid;
        const int topOfGameGrid = 1;
        var(rightHandEdgeOfGameGridCanvas,topOfGameGridCanvas) = gridToWindowCoordinates(rightHandEdgeOfGameGrid, topOfGameGrid);
        var centerX = rightHandEdgeOfGameGridCanvas + 10;
        var centerY = topOfGameGridCanvas - 15;
                    
        foreach (var shape in waitingShapes)
        {
            foreach (var pixel in shape.PixelsInShape)
            {
                var diffX = pixel.X - shape.centerX;
                var diffY = pixel.Y - shape.centerY;
                
                var gridObjX = centerX + diffX*6;
                var gridObjY = centerY + diffY*6;

                Raylib.DrawRectangle(gridObjX, gridObjY, 6, 6, pixel.color);
            }
            centerY += 30;
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
        waitingShapes.Enqueue(newShape);
    }
    
    public void ResetGrid()
    {
        waitingShapes = [];
        SpawnShape();
        SpawnShape();
        SpawnShape();
        SpawnShape();
        SpawnShape();
        fallingShape = waitingShapes.Dequeue();
        settledPixels = [];
    }
    public void enactGravity()
    {
        if (isMovementAllowed(0, 1))
        {
            fallingShape.moveShape(0, 1);
            return;
        }
        setFallingShape();
    }
    
    public void moveFallingShape(int x, int y)
    {
        if(!isMovementAllowed(x, y)) return;
        fallingShape.moveShape(x, y);
    }
    
    public void rotateFallingShape(Direction direction)
    {
        if (isRotationMovementIllegal(direction)) return;
        fallingShape.rotateShape(direction);
    }

    private void setFallingShape()
    {
        foreach (var pixel in fallingShape.PixelsInShape)
        {
            pixel.color.GetHSV(out var h, out var s, out var v);
            pixel.color = Color.FromHSV(h, s - 0.1f, v - 0.1f);
            settledPixels.Add(pixel);
        }
        fallingShape = waitingShapes.Dequeue();
        SpawnShape();
        removeFullRows();
    }

    public bool isGameOver()
    {
        foreach (var pixel in settledPixels)
        {
            if (pixel.Y < 0) return true;
        }
        return false;
    }

    bool isMovementAllowed(int x, int y)
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

    bool isRotationMovementIllegal(Direction direction)
    {
        foreach (var pixel in fallingShape.PixelsInShape)
        {
            var (aspiringX, aspiringY) = fallingShape.findRotatedCoordinates(direction, pixel);
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
            var pixelsInRow = settledPixels.Where((shape) => shape.Y == rowNumber);
            if (isRowFull(pixelsInRow)) continue;
            singleFrameScore += 10;
            settledPixels.RemoveAll((pixel)=>pixelsInRow.Contains(pixel));
            moveSettledPixelsDown(rowNumber);
        }
    }

    bool isRowFull(IEnumerable<Pixel> pixelsInRow)
    {
        return pixelsInRow.Count() != nXPixelsInGrid;
    }

    void moveSettledPixelsDown(int removedRowNumber)
    {
        foreach (var settledPixel in settledPixels)
        {
            if (settledPixel.Y < removedRowNumber)
            {
                settledPixel.Y++;
            }
        }
    }
}

