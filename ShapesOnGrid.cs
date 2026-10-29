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
    public Action onRowClear;
  
    
    public ShapesOnGrid(int windowWidth, int windowHeight, int pixelWidth)
    {
        xOrigin = (windowWidth - pixelWidth * nXPixelsInGrid) / 2;
        yOrigin = (windowHeight - pixelWidth * nYPixelsInGrid) / 2;
        gridSquareSize = pixelWidth;
        ResetGrid();
        onRowClear = () => {};
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
    

    public void PlaySingleFrame()
    {
        enactGravity();
    }
    
    
    public void DrawFrame()
    {
        DrawGrid();
        foreach (var pixel in fallingShape.PixelsInShape)
        { 
            if (pixel.coordinate.y<0) continue;
            var (pixelX, pixelY) = gridToWindowCoordinates(pixel.coordinate.x, pixel.coordinate.y); 
            Raylib.DrawRectangle(pixelX, pixelY, Pixel.Width, Pixel.Width, pixel.color);
        }
        
        foreach (var pixel in settledPixels)
        {
            var (gridObjX, gridObjY) = gridToWindowCoordinates(pixel.coordinate.x, pixel.coordinate.y);
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
                var diffX = pixel.coordinate.x - shape.centerX;
                var diffY = pixel.coordinate.y - shape.centerY;
                
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

    private void enactGravity()
    {
        if (isShapeMovementAllowed(0, 1))
        {
            fallingShape.moveShape(0, 1);
            return;
        }
        setFallingShape();
    }
    
    public void moveFallingShape(int x, int y)
    {
        if(!isShapeMovementAllowed(x, y)) return;
        fallingShape.moveShape(x, y);
    }
    
    public void rotateFallingShape(Direction direction)
    {
        if (isRotationMovementIllegal(direction))
        {
            fallingShape.rotateShape(direction);
        }
        var currentOrientation = fallingShape.currentOrientation;
        var newOrientation = fallingShape.FindNewOrientation(direction);
        if (fallingShape.wallKickLookup == null) return;
        var wallKickCoordinates = fallingShape.wallKickLookup[(initial:currentOrientation, next: newOrientation)];
        foreach (var coordinate in wallKickCoordinates)
        {
            if (!coordinateIsViable(coordinate, direction)) continue;
            fallingShape.rotateShape(direction);
            fallingShape.moveShape(coordinate.x, coordinate.y);
        }
    }
    //TODO: Is it possible that is coordinate viable method replaces some methods?? supply both movement and rotation

    private bool coordinateIsViable((int x, int y) coordinate,Direction direction )
    {
        foreach (var pixel in fallingShape.PixelsInShape)
        {
            var (movementX, movementY) = coordinate;
            var rotatedCoordinate= fallingShape.findRotatedCoordinates(direction, pixel);
            if (!isPixelMovementAllowed(movementX, movementY, rotatedCoordinate))
            {
                return false;
            }
        }

        return true;
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
            if (pixel.coordinate.y < 0) return true;
        }
        return false;
    }

    bool isShapeMovementAllowed(int x, int y)
    {
        foreach (var pixel in fallingShape.PixelsInShape)
        {
            if (!isPixelMovementAllowed(x, y, pixel.coordinate)) return false;
        }
        return true;
    }

    bool isPixelMovementAllowed(int xMovement, int yMovement, Coordinate pixelLocation)
    {
        var aspiringX = pixelLocation.x+ xMovement;
        var aspiringY = pixelLocation.y + yMovement;

        if (aspiringX is < 0 or > nXPixelsInGrid - 1) return false;
        if (aspiringY > nYPixelsInGrid - 1) return false;
        if (settledPixels.Any((pixel) => pixel.coordinate.x == aspiringX && pixel.coordinate.y == aspiringY)) return false;
        return true;
    }
    
    

    bool isRotationMovementIllegal(Direction direction)
    {
        foreach (var pixel in fallingShape.PixelsInShape)
        {
            var (aspiringX, aspiringY) = fallingShape.findRotatedCoordinates(direction, pixel);
            if (aspiringX is < 0 or > nXPixelsInGrid - 1) return true;
            if (aspiringY > nYPixelsInGrid - 1) return true;
            if (settledPixels.Any((pixel) => pixel.coordinate.x == aspiringX && pixel.coordinate.y == aspiringY)) return true;
        }
        return false;
    }

    void removeFullRows()
    {
        for (var rowNumber = 0; rowNumber < nYPixelsInGrid; rowNumber++)
        {
            var pixelsInRow = settledPixels.Where((pixel) => pixel.coordinate.y == rowNumber);
            if (isRowFull(pixelsInRow)) continue;
            onRowClear();
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
            if (settledPixel.coordinate.y < removedRowNumber)
            {
                settledPixel.coordinate = settledPixel.coordinate with { y = settledPixel.coordinate.y+1 };
            }
        }
    }
}

