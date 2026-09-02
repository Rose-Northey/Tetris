using Raylib_cs;

namespace Tetris;

public static class Menus
{
    public static void DrawGameOver()
    {
        Raylib.DrawText("Game Over", 100, 100, 20, Color.Red);
        Raylib.DrawText("Press any key to restart", 100, 150, 10, Color.White);
    }
}