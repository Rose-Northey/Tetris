using FluentAssertions;

namespace Tetris.Tests;

public class ShapesOnGridTests
{
    [Fact]
    public void LShapeShouldRotateWhenAgainstLeftWall()
    {
        List<Pixel> desiredPixels =
        [
            new (new Coordinate(0,2)),
            new (new Coordinate(1,2)),
            new (new Coordinate(2,2)),
            new (new Coordinate(3,2))
        ];
        var nXSquaresAcrossInWindow = 10;
        var nYSquaresAcrossInWindow = 10;
        var nGridSquaresAcross = 4;
        var shapeLocation = new Coordinate(2,2);
        var fallingShape = new LongShape(shapeLocation);
        fallingShape.currentOrientation = Orientation.zero;
        fallingShape.ActuallyRotateShape(Direction.Clockwise);
        fallingShape.ActuallyTranslateShape(new Coordinate(-2,0));
        List<Pixel> settledPixels = new List<Pixel>();
        var myGrid = new ShapesOnGrid(
            nXSquaresAcrossInWindow * Pixel.Width, 
            nYSquaresAcrossInWindow * Pixel.Width,
            fallingShape,
            settledPixels,
            nGridSquaresAcross,
            nGridSquaresAcross
            );
        myGrid.rotateFallingShape(Direction.Clockwise);
        fallingShape.currentOrientation.Should().Be(Orientation.two);
        fallingShape.pixels.Should()
            .BeEquivalentTo(desiredPixels, (options) => options.Excluding((pixel => pixel.color)));
    }
}