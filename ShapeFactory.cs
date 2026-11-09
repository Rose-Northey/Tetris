namespace Tetris;

public static class ShapeFactory
{
    static Shape Create(Coordinate center)
    {
        return new LongShape();
    }
}