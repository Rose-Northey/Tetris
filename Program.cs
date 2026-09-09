// See https://aka.ms/new-console-template for more information

using System.Security.AccessControl;
using Raylib_cs;
using Tetris;

internal static class Program
{
    private const int pixelWidth = 30;
    private const int nXPixelsInWindow = 30;
    private const int nYPixelsInWindow = 30;
    private const int windowWidth = pixelWidth * nXPixelsInWindow;
    private const int windowHeight = pixelWidth * nYPixelsInWindow;
 
    private static readonly Color colourOfBG= Color.FromHSV(222, 0.55f, 0.18f);


    public static void Main()
    {
        var gameState = new GameState
        {
            ShapesOnGrid = new ShapesOnGrid(windowWidth,windowHeight, pixelWidth)
        };
        gameState.ShapesOnGrid.onRowClear = gameState.onRowClear;
        gameState.Reset();
        openTetrisInMiddleOfScreen(windowWidth, windowHeight);
        while (!Raylib.WindowShouldClose())
        {
            gameState.gameTime += Raylib.GetFrameTime();
            Raylib.BeginDrawing();
            Raylib.ClearBackground(colourOfBG);
            
            switch (gameState.gameStatus)
            {
                case GameStatus.Playing: PlayTetris(gameState);
                    break;
                case GameStatus.GameOver: GameOverExperience(gameState);
                    break;
                default: throw new ArgumentOutOfRangeException();
            }
            
            
            Raylib.EndDrawing();
        }
        Raylib.CloseWindow();
    }

  

    private static void PlayTetris(GameState gameState)
    {
        gameState.ShapesOnGrid.DrawFrame();
        DrawScore(gameState.score);
        handleInput(gameState);
        if (gameState.gameTime <= gameState.gameSpeed) return;
        gameState.gameTime = 0;
        Raylib.DrawText(gameState.score.ToString(), 100, 150, 10, Color.White);
        gameState.ShapesOnGrid.PlaySingleFrame();
        
        if (gameState.ShapesOnGrid.isGameOver()) gameState.gameStatus = GameStatus.GameOver;
    }

    private static void GameOverExperience(GameState gameState)
    {
        Menus.DrawGameOver();
        if (Raylib.GetKeyPressed() == 0) return;
        gameState.Reset();
        
    }
    public static void DrawScore(int score)
    {
        // var (leftOfGrid, bottomOfGrid) = gridToWindowCoordinates(0, nYPixelsInGrid);
        //
        // Raylib.DrawText($"Score: {singleFrameScore}", leftOfGrid, bottomOfGrid+15, 10, Color.White);
        Raylib.DrawText($"Score: {score}", 0, 0, 10, Color.White);
        
    }

    private static void handleInput(GameState gameState)
    {
        if (Raylib.IsKeyPressed(KeyboardKey.Left)) gameState.ShapesOnGrid.moveFallingShape(-1, 0);
        if (Raylib.IsKeyPressed(KeyboardKey.Right)) gameState.ShapesOnGrid.moveFallingShape(1, 0); 
        if (Raylib.IsKeyPressed(KeyboardKey.Down)) gameState.ShapesOnGrid.moveFallingShape(0, 1);
        if (Raylib.IsKeyPressed(KeyboardKey.Up)) gameState.ShapesOnGrid.rotateFallingShape(Direction.Clockwise);
        if (Raylib.IsKeyPressed(KeyboardKey.Z)) gameState.ShapesOnGrid.rotateFallingShape(Direction.CounterClockwise);
    }

    static void openTetrisInMiddleOfScreen(int windowWidth, int windowHeight)
    {
        Raylib.InitWindow(windowWidth, windowHeight, "Tetris");
        var monitorPos = Raylib.GetMonitorPosition(1);
        var monitorWidth = Raylib.GetMonitorWidth(1);
        var monitorHeight = Raylib.GetMonitorHeight(1);
        Raylib.SetWindowPosition(
            (int)(monitorPos.X + (monitorWidth - windowWidth) / 2),
            (int)(monitorPos.Y + (monitorHeight - windowHeight) / 2)
        );  
    }
}

public enum Direction
{
    Clockwise = 1,
    CounterClockwise = -1,
}
