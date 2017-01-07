using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TilemapTools.Tests
{
    [TestClass]
    public class GridTests
    {
        [TestMethod]
        public void GridBlock_GetBlockCellLocation()
        {
            ShortPoint blockLocation;
            int cellX;
            int cellY;

            GetBlockCellLocation(1, 1, out blockLocation, out cellX, out cellY);
            Assert.AreEqual(blockLocation, new ShortPoint(1, 1));
            Assert.AreEqual(cellX, 0);
            Assert.AreEqual(cellY, 15);

            GetBlockCellLocation(1, -1, out blockLocation, out cellX, out cellY);
            Assert.AreEqual(blockLocation, new ShortPoint(1, -1));
            Assert.AreEqual(cellX, 0);
            Assert.AreEqual(cellY, 0);

            GetBlockCellLocation(-1, -1, out blockLocation, out cellX, out cellY);
            Assert.AreEqual(blockLocation, new ShortPoint(-1, -1));
            Assert.AreEqual(cellX, 15);
            Assert.AreEqual(cellY, 0);

            GetBlockCellLocation(-1, 1, out blockLocation, out cellX, out cellY);
            Assert.AreEqual(blockLocation, new ShortPoint(-1, 1));
            Assert.AreEqual(cellX, 15);
            Assert.AreEqual(cellY, 15);

            GetBlockCellLocation(16, 16, out blockLocation, out cellX, out cellY);
            Assert.AreEqual(blockLocation, new ShortPoint(1, 1));
            Assert.AreEqual(cellX, 15);
            Assert.AreEqual(cellY, 0);

            GetBlockCellLocation(16, -16, out blockLocation, out cellX, out cellY);
            Assert.AreEqual(blockLocation, new ShortPoint(1, -1));
            Assert.AreEqual(cellX, 15);
            Assert.AreEqual(cellY, 15);

            GetBlockCellLocation(-16, -16, out blockLocation, out cellX, out cellY);
            Assert.AreEqual(blockLocation, new ShortPoint(-1, -1));
            Assert.AreEqual(cellX, 0);
            Assert.AreEqual(cellY, 15);

            GetBlockCellLocation(-16, 16, out blockLocation, out cellX, out cellY);
            Assert.AreEqual(blockLocation, new ShortPoint(-1, 1));
            Assert.AreEqual(cellX, 0);
            Assert.AreEqual(cellY, 0);

            GetBlockCellLocation(17, 17, out blockLocation, out cellX, out cellY);
            Assert.AreEqual(blockLocation, new ShortPoint(2, 2));
            Assert.AreEqual(cellX, 0);
            Assert.AreEqual(cellY, 15);

            GetBlockCellLocation(17, -17, out blockLocation, out cellX, out cellY);
            Assert.AreEqual(blockLocation, new ShortPoint(2, -2));
            Assert.AreEqual(cellX, 0);
            Assert.AreEqual(cellY, 0);

            GetBlockCellLocation(-17, -17, out blockLocation, out cellX, out cellY);
            Assert.AreEqual(blockLocation, new ShortPoint(-2, -2));
            Assert.AreEqual(cellX, 15);
            Assert.AreEqual(cellY, 0);

            GetBlockCellLocation(-17, 17, out blockLocation, out cellX, out cellY);
            Assert.AreEqual(blockLocation, new ShortPoint(-2, 2));
            Assert.AreEqual(cellX, 15);
            Assert.AreEqual(cellY, 15);

            GetBlockCellLocation(20, 20, out blockLocation, out cellX, out cellY);
            Assert.AreEqual(blockLocation, new ShortPoint(2, 2));
            Assert.AreEqual(cellX, 3);
            Assert.AreEqual(cellY, 12);

            GetBlockCellLocation(20, -20, out blockLocation, out cellX, out cellY);
            Assert.AreEqual(blockLocation, new ShortPoint(2, -2));
            Assert.AreEqual(cellX, 3);
            Assert.AreEqual(cellY, 3);

            GetBlockCellLocation(-20, -20, out blockLocation, out cellX, out cellY);
            Assert.AreEqual(blockLocation, new ShortPoint(-2, -2));
            Assert.AreEqual(cellX, 12);
            Assert.AreEqual(cellY, 3);

            GetBlockCellLocation(-20, 20, out blockLocation, out cellX, out cellY);
            Assert.AreEqual(blockLocation, new ShortPoint(-2, 2));
            Assert.AreEqual(cellX, 12);
            Assert.AreEqual(cellY, 12);
        }


        [TestMethod]
        public void GridBlock_GetCellLocation()
        {
            ShortPoint blockLocation;
            int cellX;
            int cellY;
            int index;

            blockLocation = new ShortPoint(1, 1);

            GetCellLocation(0, blockLocation, out cellX, out cellY);
            Assert.AreEqual(cellX, 1);
            Assert.AreEqual(cellY, 16);

            GetCellLocation(15, blockLocation, out cellX, out cellY);
            Assert.AreEqual(cellX, 16);
            Assert.AreEqual(cellY, 16);

            GetCellLocation(240, blockLocation, out cellX, out cellY);
            Assert.AreEqual(cellX, 1);
            Assert.AreEqual(cellY, 1);

            GetCellLocation(255, blockLocation, out cellX, out cellY);
            Assert.AreEqual(cellX, 16);
            Assert.AreEqual(cellY, 1);


            blockLocation = new ShortPoint(-1, -1);

            GetCellLocation(0, blockLocation, out cellX, out cellY);
            Assert.AreEqual(cellX, -16);
            Assert.AreEqual(cellY, -1);

            GetCellLocation(15, blockLocation, out cellX, out cellY);
            Assert.AreEqual(cellX, -1);
            Assert.AreEqual(cellY, -1);

            GetCellLocation(240, blockLocation, out cellX, out cellY);
            Assert.AreEqual(cellX, -16);
            Assert.AreEqual(cellY, -16);

            GetCellLocation(255, blockLocation, out cellX, out cellY);
            Assert.AreEqual(cellX, -1);
            Assert.AreEqual(cellY, -16);

        }

        [TestMethod]
        public void Grid_Enumerable()
        {
            var grid = new Grid<int, int>();

            var input = new List<CellLocationPair<int>>()
            {

                new CellLocationPair<int>(1, 1, 1),
                new CellLocationPair<int>(2, 1, -1),
                new CellLocationPair<int>(3, -1, -1),
                new CellLocationPair<int>(4, -1, 1),
                new CellLocationPair<int>(5, 16, 16),
                new CellLocationPair<int>(6, 16, -16),
                new CellLocationPair<int>(7, -16, -16),
                new CellLocationPair<int>(8, -16, 16),
            };

            foreach (var item in input)
            {
                grid[item.X, item.Y] = item.Content;
            }


            var cells = (grid as IEnumerable<CellLocationPair<int>>).ToList();

            var sortedInputContent = input.OrderBy(c => c.X).ThenBy(c => c.Y).Select(c => c.Content);
            var sortedContent = cells.Where(c => c.Content != 0).OrderBy(c => c.X).ThenBy(c => c.Y).Select(c => c.Content);

            Assert.IsTrue(Enumerable.SequenceEqual(sortedInputContent, sortedContent));
        }

        static void GetCellLocation(int index, ShortPoint blockLocation, out int cellX, out int cellY, int blockSize = GridBlock.DefaultBlockSize)
        {
            GridBlock.GetCellLocation(ref index, ref blockSize, ref blockLocation, out cellX, out cellY);
        }

        static void GetBlockCellLocation(int x, int y, out ShortPoint blockLocation, out int cellX, out int cellY, int blockSize = GridBlock.DefaultBlockSize)
        {
            GridBlock.GetBlockCellLocation(ref x, ref y, ref blockSize, out blockLocation, out cellX, out cellY);
        }
    }
}
