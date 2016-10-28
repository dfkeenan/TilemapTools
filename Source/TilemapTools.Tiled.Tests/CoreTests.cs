using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TilemapTools.Mathematics;
using TilemapTools.Tiled;

namespace TilemapTools.Tiled.Tests
{
    [TestClass]
    public class CoreTests
    {
        [TestMethod]
        public void TiledElementList_Add_Duplicate_Key()
        {
            var list = new TiledElementList<TiledObject>();

            list.Add(new TiledObject { Name = "Foo" });
            list.Add(new TiledObject { Name = "Foo_1" });
            list.Add(new TiledObject { Name = "Foo" });

            Assert.IsTrue(list.Contains("Foo"));
            Assert.IsTrue(list.Contains("Foo_1"));
            Assert.IsTrue(list.Contains("Foo_1__1"));
        }

        [TestMethod]
        public void Color_FromString_AARRGGBB()
        {
            var color = Color.FromString("#01020304");

            Assert.AreEqual(color.Value.A, 1);
            Assert.AreEqual(color.Value.R, 2);
            Assert.AreEqual(color.Value.G, 3);
            Assert.AreEqual(color.Value.B, 4);
        }

        [TestMethod]
        public void Color_FromString_RRGGBB()
        {
            var color = Color.FromString("#020304");

            Assert.AreEqual(color.Value.A, 255);
            Assert.AreEqual(color.Value.R, 2);
            Assert.AreEqual(color.Value.G, 3);
            Assert.AreEqual(color.Value.B, 4);
        }
    }
}
