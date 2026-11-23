using System.Runtime.InteropServices;
using FluentAssertions;

namespace Tetris.Tests;

public class LongShapeTest
{
    [Fact]
    public void EnsureZeroOrientationIsHorizontal()
    {
        //on a 4x4 grid, 0 at top left, center is at 2,2
        List<Pixel> desiredPixels =
        [
            new (new Coordinate(0, 1)),
            new (new Coordinate(1, 1)),
            new (new Coordinate(2, 1)),
            new (new Coordinate(3, 1))
        ];
        
        var longShape = new LongShape(new Coordinate(2, 2));
        longShape.currentOrientation = Orientation.zero;
        longShape.pixels.Should().BeEquivalentTo(
            desiredPixels,
            (options)=>options.Excluding((pixel)=>pixel.color)
            );
    }

    //TODO: fix other shapes initial orientations 
    
    [Theory]
    [InlineData(Direction.Clockwise, 2, Orientation.right)]
    [InlineData(Direction.CounterClockwise, 1, Orientation.left)]
    public void rotationFindsNextOrientation(Direction direction, int desiredPixelX, Orientation desiredOrientation)
    {
        List<Pixel> desiredPixels =
        [
            new (new Coordinate(desiredPixelX, 0)),
            new (new Coordinate(desiredPixelX, 1)),
            new (new Coordinate(desiredPixelX, 2)),
            new (new Coordinate(desiredPixelX, 3))
        ];
        var longShape = new LongShape(new Coordinate(2, 2));
        longShape.currentOrientation = Orientation.zero;
        longShape.ActuallyRotateShape(direction);
        longShape.pixels.Should().BeEquivalentTo(
            desiredPixels,
            (options)=>options.Excluding((pixel)=>pixel.color)
        );
        longShape.currentOrientation.Should().Be(desiredOrientation);
    } 
}