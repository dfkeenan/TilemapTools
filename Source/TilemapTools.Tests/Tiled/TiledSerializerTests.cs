using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TilemapTools.Tiled;
using TilemapTools.Tiled.Serialization;

namespace TilemapTools.Tests.Tiled
{
    [TestClass]
    public class TiledSerializerTests
    {
        private TiledSerializerOptions options;

        public TiledSerializerTests()
        {
            this.options = new TiledSerializerOptions(File.OpenRead);
        }

        [TestMethod]
        public void TiledSerializer_LoadTileMap_BasicTest()
        {
            //var serializer = new TiledSerializer(options);

            //var map = serializer.LoadTileMap("TestFiles/Dungeon-gzip.tmx");

            //Assert.IsNotNull(map);   

        }

        [TestMethod]
        public void TiledSerializer_ParseEnum()
        {

            Assert.AreEqual(TiledSerializer.ParseEnum<StaggerAxis>("x"), StaggerAxis.X);

            Assert.AreEqual(TiledSerializer.ParseEnum<StaggerAxis>("X"), StaggerAxis.X);

            Assert.AreEqual(TiledSerializer.ParseEnum<RenderOrder>("right-down"), RenderOrder.RightDown);

            Assert.AreEqual(TiledSerializer.ParseEnum<RenderOrder>(null), RenderOrder.RightDown);
        }

    }
}
