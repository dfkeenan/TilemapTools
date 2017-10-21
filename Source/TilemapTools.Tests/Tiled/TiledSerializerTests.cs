using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TilemapTools.Tiled;
using TilemapTools.Tiled.Serialization;

namespace TilemapTools.Tests.Tiled
{
    [TestClass]
    public class TiledSerializerTests
    {
        private TiledDeserializerOptions options;

        public TiledSerializerTests()
        {
            this.options = new TiledDeserializerOptions(File.OpenRead);
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

            Assert.AreEqual(TiledDeserializer.ParseEnum<StaggerAxis>("x"), StaggerAxis.X);

            Assert.AreEqual(TiledDeserializer.ParseEnum<StaggerAxis>("X"), StaggerAxis.X);

            Assert.AreEqual(TiledDeserializer.ParseEnum<RenderOrder>("right-down"), RenderOrder.RightDown);

            Assert.AreEqual(TiledDeserializer.ParseEnum<RenderOrder>(null), RenderOrder.RightDown);
        }

    }
}
