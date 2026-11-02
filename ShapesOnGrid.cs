using Raylib_cs;

namespace Tetris;

public class ShapesOnGrid
{
    private int nWidth;
    private int nHeight;
    private readonly int xOrigin;
    private readonly int yOrigin;
    private const int nXPixelsInGrid= 5;
    private const int nYPixelsInGrid= 10;
    private Shape fallingShape;
    private Queue<Shape> waitingShapes;
    private List<Pixel> settledPixels;
    private static readonly Color colourOfGridFill = Color.FromHSV(222, 0.50f, 0.22f);
    private static readonly Color colourOfGridBorder = Color.FromHSV(188, 0.80f, 0.85f);
    private static readonly Color colourOfGridLines = Color.FromHSV(222, 0.45f, 0.32f);
    private int singleFrameScore;
    public Action onRowClear;
  
    
    public ShapesOnGrid(int windowWidth, int windowHeight)
    {
        xOrigin = (windowWidth - Pixel.Width * nXPixelsInGrid) / 2;
        yOrigin = (windowHeight - Pixel.Width * nYPixelsInGrid) / 2;
        ResetGrid();
        onRowClear = () => {};
    }
    
    private void DrawGrid()
    {
        var widthGrid = nXPixelsInGrid * Pixel.Width;
        var heightGrid = nYPixelsInGrid * Pixel.Width;
        const int lineWidth = 1;
        Raylib.DrawRectangle(xOrigin, yOrigin, widthGrid, heightGrid,colourOfGridFill);
     
        for (var i = xOrigin; i <= widthGrid+xOrigin; i+= Pixel.Width)
        {
            Raylib.DrawRectangle(i, yOrigin,lineWidth, heightGrid, colourOfGridLines);
        }
        for (var i = yOrigin; i <= heightGrid + yOrigin; i += Pixel.Width)
        {
            Raylib.DrawRectangle(xOrigin, i, widthGrid, lineWidth, colourOfGridLines);
        }
        Raylib.DrawRectangleLines(xOrigin-1, yOrigin-1, widthGrid+2, heightGrid+2, colourOfGridBorder);
    }
    

    public void PlaySingleFrame()
    {
        enactGravity();
    }
 
    private void DrawPixel(Pixel pixel)
    {
        var absolutePosition = gridToWindowCoordinates(pixel.coordinate); 
        Raylib.DrawRectangle(absolutePosition.x, absolutePosition.y, Pixel.Width, Pixel.Width, pixel.color);
    }
    
    public void DrawFrame()
    {
        DrawGrid();
        foreach (var pixel in fallingShape.PixelsInShape)
        { 
            if (pixel.coordinate.y<0) continue;
            DrawPixel(pixel);
        }
        
        foreach (var pixel in settledPixels)
        {
           DrawPixel(pixel);
        }

  
        const int rightHandEdgeOfGameGrid = nXPixelsInGrid;
        const int topOfGameGrid = 1;
        var rightCorner = new Coordinate(rightHandEdgeOfGameGrid, topOfGameGrid);
        var(rightHandEdgeOfGameGridCanvas,topOfGameGridCanvas) = gridToWindowCoordinates(rightCorner);
        var centerX = rightHandEdgeOfGameGridCanvas + 10;
        var centerY = topOfGameGridCanvas - 15;
                    
        foreach (var shape in waitingShapes)
        {
            foreach (var pixel in shape.PixelsInShape)
            {
                var diffX = pixel.coordinate.x - shape.center.x;
                var diffY = pixel.coordinate.y - shape.center.y;
                
                var gridObjX = centerX + diffX*6;
                var gridObjY = centerY + diffY*6;

                Raylib.DrawRectangle(gridObjX, gridObjY, 6, 6, pixel.color);
            }
            centerY += 30;
        }
    }

    private Coordinate gridToWindowCoordinates(Coordinate gridLocation)
    {
        var windowX = gridLocation.x * Pixel.Width + xOrigin;
        var windowY = gridLocation.y * Pixel.Width + yOrigin;
        return new Coordinate(windowX, windowY);
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
        
        if (isShapeMovementAllowed(fallingShape,Direction.None,0, 1))
        {
            fallingShape.moveShape(0, 1);
            return;
        }
        setFallingShape();
    }
    
    public void moveFallingShape(int x, int y)
    {
        if(!isShapeMovementAllowed(fallingShape,Direction.None, x,y)) return;
        fallingShape.moveShape(x, y);
    }
    
    public void rotateFallingShape(Direction direction)
    {
        if (isShapeMovementAllowed(fallingShape, direction,0,0))
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
            var rotatedCoordinate = fallingShape.findRotatedTranslatedPixelLocation(pixel,movementX,movementY,Direction.None);
      
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

    private bool isLocationAllowed(Coordinate location)
    {
        return !isLocatedOutsideGrid(location) && isLocationEmpty(location);
    }

    private bool isShapeMovementAllowed(Shape shape, Direction direction,int xMovement,int yMovement)
    {
        foreach (var pixel in shape.PixelsInShape)
        {
            var aspiringLocation = shape.findRotatedTranslatedPixelLocation(pixel,xMovement,yMovement,direction);
            if (!isLocationAllowed(aspiringLocation)) return false;
        }
        return true;
    }

    private void removeFullRows()
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

    private bool isLocatedOutsideGrid(Coordinate location)
    {
        if (location.x is < 0 or > nXPixelsInGrid - 1) return true;
        return location.y > nYPixelsInGrid - 1;
    }

    private bool isLocationEmpty(Coordinate location)
    {
        return !settledPixels.Any((pixel) => pixel.coordinate.x == location.x && pixel.coordinate.y == location.y);
    }
    
    private bool isRowFull(IEnumerable<Pixel> pixelsInRow)
    {
        return pixelsInRow.Count() != nXPixelsInGrid;
    }

    private void moveSettledPixelsDown(int removedRowNumber)
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

