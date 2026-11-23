using FluentAssertions;

namespace Tetris.Tests;

public class ShapeTests
{
    [Fact]
    public void FindNewOrientationCanRotateClockwise()
    {
        Shape myShape = new JShape(new Coordinate(0, 0));
        myShape.currentOrientation = Orientation.zero;
        var newOrientation = myShape.FindNewOrientation(Direction.Clockwise);
        newOrientation.Should().Be(Orientation.right);
    }
    
    [Fact]
    public void FindNewOrientationCanRotateCounterClockwise()
    {
        Shape myShape = new JShape(new Coordinate(0, 0));
        myShape.currentOrientation = Orientation.zero;
        var newOrientation = myShape.FindNewOrientation(Direction.CounterClockwise);
        newOrientation.Should().Be(Orientation.left);
    }
}