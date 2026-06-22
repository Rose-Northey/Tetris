// See https://aka.ms/new-console-template for more information

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
    private static readonly Color colourOfGridFill = Color.FromHSV(222, 0.50f, 0.22f);
    private static readonly Color colourOfGridBorder = Color.FromHSV(188, 0.80f, 0.85f);
    private static readonly Color colourOfGridLines = Color.FromHSV(222, 0.45f, 0.32f);

    public static void Main()
    {
        var gameState = new GameState();
        gameState.ObjectsOnGrid = new ObjectsOnGrid(windowWidth,windowHeight, pixelWidth);
        gameState.Reset();
        openTetrisInMiddleOfScreen(windowWidth, windowHeight);
        while (!Raylib.WindowShouldClose())
        {
            gameState.gameTime += Raylib.GetFrameTime();
            Raylib.BeginDrawing();
            Raylib.ClearBackground(colourOfBG);
            DrawGrid(gameState.ObjectsOnGrid.xOrigin,gameState.ObjectsOnGrid.yOrigin, gameState.ObjectsOnGrid);
            
            PlayTetris(gameState);
            Raylib.EndDrawing();
        }
        Raylib.CloseWindow();
    }

    private static void DrawGrid(int x, int y, ObjectsOnGrid objectsOnGrid)
    {
        const int lineWidth = 1;
        Raylib.DrawRectangle(x, y, objectsOnGrid.widthGrid, objectsOnGrid.heightGrid,colourOfGridFill);
     
        for (var i = x; i <= objectsOnGrid.widthGrid+x; i+=pixelWidth)
        {
            Raylib.DrawRectangle(i, y,lineWidth, objectsOnGrid.heightGrid, colourOfGridLines);
        }
        for (var i = y; i <= objectsOnGrid.heightGrid + y; i += pixelWidth)
        {
            Raylib.DrawRectangle(x, i, objectsOnGrid.widthGrid, lineWidth, colourOfGridLines);
        }
        Raylib.DrawRectangleLines(x-1, y-1, objectsOnGrid.widthGrid+2, objectsOnGrid.heightGrid+2, colourOfGridBorder);
    }

    private static void PlayTetris(GameState gameState)
    {
        gameState.ObjectsOnGrid.DrawObjectFrame();
        if (gameState.gameTime <= gameState.gameSpeed) return;
        gameState.gameTime = 0; 
        gameState.ObjectsOnGrid.enactGravity();
        
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

