using SiliconStudio.Xenko.Engine;

namespace TilemapTools.Demo
{
    class TilemapTools_DemoApp
    {
        static void Main(string[] args)
        {
            using (var game = new Game())
            {
                game.Run();
            }
        }
    }
}
