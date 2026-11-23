using FluentAssertions;

namespace Tetris.Tests;

public class LShapeTest
{
    static List<Pixel> northL =
    [
        new(new Coordinate(0, 1)),
        new(new Coordinate(1, 1)),
        new(new Coordinate(2, 1)),
        new(new Coordinate(2, 0))
    ];

    public static List<Pixel> eastL =
    [
        new(new Coordinate(1, 0)),
        new(new Coordinate(1, 1)),
        new(new Coordinate(1, 2)),
        new(new Coordinate(2, 2))
    ];

    static List<Pixel> westL =
    [
        new(new Coordinate(1, 0)),
        new(new Coordinate(1, 1)),
        new(new Coordinate(1, 2)),
        new(new Coordinate(0, 0))
    ];
        
    [Fact]
    public void EnsureInitialOrientationIsNorthL()
    {
        var lShape = new LShape(new Coordinate(1, 1));
        lShape.currentOrientation = Orientation.zero;
        lShape.pixels.Should().BeEquivalentTo(
            northL,
            (options) => options.Excluding(pixel => pixel.color)
        );
    }

    public static TheoryData<Direction, List<Pixel>, Orientation> RotationCases => new()
    {
        { Direction.Clockwise,        eastL, Orientation.right },
        { Direction.CounterClockwise, westL, Orientation.left  },
    };
    
    [Theory]
    [MemberData(nameof(RotationCases))]
    public void rotationFindsNextOrientation(Direction direction, List<Pixel> desiredShape, Orientation desiredOrientation)
    {
        var longShape = new LongShape(new Coordinate(1,1));
        longShape.currentOrientation = Orientation.zero;
        longShape.ActuallyRotateShape(direction);
        longShape.pixels.Should().BeEquivalentTo(
            desiredShape,
            (options)=>options.Excluding((pixel)=>pixel.color)
        );
        longShape.currentOrientation.Should().Be(desiredOrientation);
    } 
}