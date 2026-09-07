namespace Tetris;

public class GameState
{
    public ShapesOnGrid ShapesOnGrid;
    public float gameTime;
    public float gameSpeed;
    public GameStatus gameStatus;
    public int score;
    public void Reset()
    {
        gameTime = 0;
        gameSpeed = 1f;
        ShapesOnGrid.ResetGrid();
        gameStatus = GameStatus.Playing;
        score = 0;
    }
}
//the thing I should do with my life is to make a game because I want to, and then I will be the best
//i can solve my problems by making games, and then I will be the best
//my relationship with my family is going to be great because I will be the best
//I will be the best

public enum GameStatus { Playing, GameOver }

