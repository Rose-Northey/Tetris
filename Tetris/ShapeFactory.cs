namespace Tetris;

public static class ShapeFactory
{
    public static Shape Create(Coordinate center)
    {
        var randomNumber = new Random().Next(0, 7);
         return randomNumber switch
        {
            (0) => new SquareShape(center),
            (1) => new LongShape(center),
            (2) => new JShape(center),
            (3) => new LShape(center),
            (4) => new TShape(center),
            (5) => new ZShape(center),
            _ => new SShape(center)
        };
    }
}