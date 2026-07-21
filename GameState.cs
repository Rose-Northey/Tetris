namespace Tetris;

public class GameState
{
    public ShapesOnGrid ShapesOnGrid;
    public float gameTime;
    public float gameSpeed;
    public void Reset()
    {
        gameTime = 0;
        gameSpeed = 0.2f;
        ShapesOnGrid.ResetGrid();
        
    }
}