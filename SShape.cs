namespace Tetris;

public class SShape:JLSTZSShape
{
    public SShape(Coordinate center) : base(center)
    {
    }

    protected override void CreateShape()
    {
        var pixel1 = new Pixel(center with { x = center.x - 1 });
        var pixel2 = new Pixel(center);
        var pixel3 = new Pixel(center with { y = center.y - 1 });
        var pixel4 = new Pixel(new Coordinate(center.x+1, center.y-1));

        pixels.AddRange(pixel1, pixel2, pixel3, pixel4);
    }
}