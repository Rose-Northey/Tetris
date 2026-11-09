using Raylib_cs;

namespace Tetris;

public class ShapesOnGrid
{
    private int nWidth;
    private int nHeight;
    private readonly Coordinate origin;
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
        var xOrigin = (windowWidth - Pixel.Width * nXPixelsInGrid) / 2;
        var yOrigin = (windowHeight - Pixel.Width * nYPixelsInGrid) / 2;
        origin = new Coordinate(xOrigin, yOrigin);
        ResetGrid();
        onRowClear = () => {};
    }
    
    private void DrawGrid()
    {
        var widthGrid = nXPixelsInGrid * Pixel.Width;
        var heightGrid = nYPixelsInGrid * Pixel.Width;
        const int lineWidth = 1;
        Raylib.DrawRectangle(origin.x, origin.y, widthGrid, heightGrid,colourOfGridFill);
     
        for (var i = origin.x; i <= widthGrid+origin.x; i+= Pixel.Width)
        {
            Raylib.DrawRectangle(i, origin.y,lineWidth, heightGrid, colourOfGridLines);
        }
        for (var i = origin.y; i <= heightGrid + origin.y; i += Pixel.Width)
        {
            Raylib.DrawRectangle(origin.x, i, widthGrid, lineWidth, colourOfGridLines);
        }
        Raylib.DrawRectangleLines(origin.x-1, origin.y-1, widthGrid+2, heightGrid+2, colourOfGridBorder);
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
        foreach (var pixel in fallingShape.pixels)
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
            foreach (var pixel in shape.pixels)
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
        var windowX = gridLocation.x * Pixel.Width + origin.x;
        var windowY = gridLocation.y * Pixel.Width + origin.y;
        return new Coordinate(windowX, windowY);
    }


    private void SpawnShape()
    {
        var newShape = ShapeFactory.create(new Coordinate(nXPixelsInGrid / 2, -1));
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
        
        if (isShapeMovementAllowed(fallingShape,Direction.None,new Coordinate(0,1)))
        {
            fallingShape.ActuallyTranslateShape(new Coordinate(0, 1));
            return;
        }
        setFallingShape();
    }
    
    public void translateFallingShape(Coordinate movement)
    {
        if(!isShapeMovementAllowed(fallingShape,Direction.None, movement)) return;
        fallingShape.ActuallyTranslateShape(movement);
    }
    
    public void rotateFallingShape(Direction direction)
    {
        var initialOrientation = fallingShape.currentOrientation;
        var nextOrientation = fallingShape.FindNewOrientation(direction);
        if (fallingShape.wallKickLookup == null) return;
        var wallKickCoordinates = fallingShape.wallKickLookup[(initial:initialOrientation, next: nextOrientation)];
        var viableWallKickTranslation = wallKickCoordinates.FirstOrDefault(coordinate => isWallKickViable(coordinate, direction),null);
        if (viableWallKickTranslation is null) return;
        fallingShape.ActuallyRotateShape(direction);
        fallingShape.ActuallyTranslateShape(viableWallKickTranslation);
    }


    private bool isWallKickViable(Coordinate translation,Direction rotation )
    {
        return fallingShape.pixels.All(pixel =>
        {
            var newLocation = fallingShape.findRotatedTranslatedPixelLocation(pixel, translation, rotation);
            return isPlacementAllowed(newLocation);
        });
    }

    private void setFallingShape()
    {
        foreach (var pixel in fallingShape.pixels)
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

    private bool isPlacementAllowed(Coordinate location)
    {
        return isLocatedInsideGrid(location) && isLocationFree(location);
    }

    private bool isShapeMovementAllowed(Shape shape, Direction direction,Coordinate movement)
    {
        return shape.pixels.All(pixel =>
        {
            var aspiringLocation = shape.findRotatedTranslatedPixelLocation(pixel, movement, direction);
            return isPlacementAllowed(aspiringLocation);
        });
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

    private static bool isLocatedInsideGrid(Coordinate location)
    {
        var isXInBounds = location.x >= 0 && location.x < nXPixelsInGrid;
        var isYInBounds = location.y <= nYPixelsInGrid - 1;
        return isXInBounds && isYInBounds;
    }

    private bool isLocationFree(Coordinate location)
    {
        var locationIsTaken = settledPixels.Any((pixel) => pixel.coordinate.x == location.x && pixel.coordinate.y == location.y);
        return !locationIsTaken;
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

