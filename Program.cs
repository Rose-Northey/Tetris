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
        gameState.Reset();
        openTetrisInMiddleOfScreen(windowWidth, windowHeight);
        while (!Raylib.WindowShouldClose())
        {
            gameState.gameTime += Raylib.GetFrameTime();
            Raylib.BeginDrawing();
            Raylib.ClearBackground(colourOfBG);
            // DrawGrid(gameState.ShapesOnGrid.xOrigin,gameState.ShapesOnGrid.yOrigin, gameState.ShapesOnGrid);
            
            PlayTetris(gameState);
            Raylib.EndDrawing();
        }
        Raylib.CloseWindow();
    }

    private static void PlayTetris(GameState gameState)
    {
        gameState.ShapesOnGrid.DrawFrame();
        handleInput(gameState);
        // keyboard press
        if (gameState.gameTime <= gameState.gameSpeed) return;
        gameState.gameTime = 0; 
        gameState.ShapesOnGrid.enactGravity();
        
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
