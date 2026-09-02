namespace Tetris;

public class GameState
{
    public ShapesOnGrid ShapesOnGrid;
    public float gameTime;
    public float gameSpeed;
    public GameStatus gameStatus;
    public void Reset()
    {
        gameTime = 0;
        gameSpeed = 1f;
        ShapesOnGrid.ResetGrid();
        gameStatus = GameStatus.Playing;
    }
}

public enum GameStatus { Playing, GameOver }

